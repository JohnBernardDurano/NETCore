using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Services
{
    public interface ILinkService<T>
    {
        object ExpandSingleHoloENItem(object resource, int identifier, ApiVersion version);

        List<LinkDto> CreateLinksForCollection(QueryParameters queryParameters, int totalCount, ApiVersion version);

    }
}
