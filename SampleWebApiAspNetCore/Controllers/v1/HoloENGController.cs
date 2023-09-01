using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Services;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Repositories;
using System.Text.Json;

namespace SampleWebApiAspNetCore.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HoloENGController : ControllerBase
    {
        private readonly IHoloENRepository _HoloENRepository;
        private readonly IMapper _mapper;
        private readonly ILinkService<HoloENGController> _linkService;

        public HoloENGController(
            IHoloENRepository HoloENRepository,
            IMapper mapper,
            ILinkService<HoloENGController> linkService)
        {
            _HoloENRepository = HoloENRepository;
            _mapper = mapper;
            _linkService = linkService;
        }

        [HttpGet(Name = nameof(GetAllHoloENG))]
        public ActionResult GetAllHoloENG(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            List<HoloENEntity> HoloENItems = _HoloENRepository.GetAll(queryParameters).ToList();

            var allItemCount = _HoloENRepository.Count();

            var paginationMetadata = new
            {
                totalCount = allItemCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allItemCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = _linkService.CreateLinksForCollection(queryParameters, allItemCount, version);
            var toReturn = HoloENItems.Select(x => _linkService.ExpandSingleHoloENItem(x, x.Id, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleHoloEN))]
        public ActionResult GetSingleHoloEN(ApiVersion version, int id)
        {
            HoloENEntity HoloENItem = _HoloENRepository.GetSingle(id);

            if (HoloENItem == null)
            {
                return NotFound();
            }

            HoloENDto item = _mapper.Map<HoloENDto>(HoloENItem);

            return Ok(_linkService.ExpandSingleHoloENItem(item, item.Id, version));
        }

        [HttpPost(Name = nameof(AddHoloEN))]
        public ActionResult<HoloENDto> AddHoloEN(ApiVersion version, [FromBody] HoloENCreateDto HoloENCreateDto)
        {
            if (HoloENCreateDto == null)
            {
                return BadRequest();
            }

            HoloENEntity toAdd = _mapper.Map<HoloENEntity>(HoloENCreateDto);

            _HoloENRepository.Add(toAdd);

            if (!_HoloENRepository.Save())
            {
                throw new Exception("Creating a HoloENitem failed on save.");
            }

            HoloENEntity newHoloENItem = _HoloENRepository.GetSingle(toAdd.Id);
            HoloENDto HoloENDto = _mapper.Map<HoloENDto>(newHoloENItem);

            return CreatedAtRoute(nameof(GetSingleHoloEN),
                new { version = version.ToString(), id = newHoloENItem.Id },
                _linkService.ExpandSingleHoloENItem(HoloENDto, HoloENDto.Id, version));
        }

        [HttpPatch("{id:int}", Name = nameof(PartiallyUpdateHoloEN))]
        public ActionResult<HoloENDto> PartiallyUpdateHoloEN(ApiVersion version, int id, [FromBody] JsonPatchDocument<HoloENUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            HoloENEntity existingEntity = _HoloENRepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            HoloENUpdateDto HoloENUpdateDto = _mapper.Map<HoloENUpdateDto>(existingEntity);
            patchDoc.ApplyTo(HoloENUpdateDto);

            TryValidateModel(HoloENUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(HoloENUpdateDto, existingEntity);
            HoloENEntity updated = _HoloENRepository.Update(id, existingEntity);

            if (!_HoloENRepository.Save())
            {
                throw new Exception("Updating a HoloENitem failed on save.");
            }

            HoloENDto HoloENDto = _mapper.Map<HoloENDto>(updated);

            return Ok(_linkService.ExpandSingleHoloENItem(HoloENDto, HoloENDto.Id, version));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(RemoveHoloEN))]
        public ActionResult RemoveHoloEN(int id)
        {
            HoloENEntity HoloENItem = _HoloENRepository.GetSingle(id);

            if (HoloENItem == null)
            {
                return NotFound();
            }

            _HoloENRepository.Delete(id);

            if (!_HoloENRepository.Save())
            {
                throw new Exception("Deleting a HoloENitem failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(UpdateHoloEN))]
        public ActionResult<HoloENDto> UpdateHoloEN(ApiVersion version, int id, [FromBody] HoloENUpdateDto HoloENUpdateDto)
        {
            if (HoloENUpdateDto == null)
            {
                return BadRequest();
            }

            var existingHoloENItem = _HoloENRepository.GetSingle(id);

            if (existingHoloENItem == null)
            {
                return NotFound();
            }

            _mapper.Map(HoloENUpdateDto, existingHoloENItem);

            _HoloENRepository.Update(id, existingHoloENItem);

            if (!_HoloENRepository.Save())
            {
                throw new Exception("Updating a HoloENitem failed on save.");
            }

            HoloENDto HoloENDto = _mapper.Map<HoloENDto>(existingHoloENItem);

            return Ok(_linkService.ExpandSingleHoloENItem(HoloENDto, HoloENDto.Id, version));
        }

        [HttpGet("GetRandomVtuber", Name = nameof(GetRandomVtuber))]
        public ActionResult GetRandomVtuber()
        {
            ICollection<HoloENEntity> HoloENItems = _HoloENRepository.GetRandomVtuber();

            IEnumerable<HoloENDto> dtos = HoloENItems.Select(x => _mapper.Map<HoloENDto>(x));

            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(Url.Link(nameof(GetRandomVtuber), null), "self", "GET"));

            return Ok(new
            {
                value = dtos,
                links = links
            });
        }
    }
}
