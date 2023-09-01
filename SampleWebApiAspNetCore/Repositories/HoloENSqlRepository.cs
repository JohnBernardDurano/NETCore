using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Models;
using System.Linq.Dynamic.Core;

namespace SampleWebApiAspNetCore.Repositories
{
    public class HoloENSqlRepository : IHoloENRepository
    {
        private readonly HoloENDbContext _HoloENDbContext;

        public HoloENSqlRepository(HoloENDbContext HoloENDbContext)
        {
            _HoloENDbContext = HoloENDbContext;
        }

        public HoloENEntity GetSingle(int id)
        {
            return _HoloENDbContext.HoloENItems.FirstOrDefault(x => x.Id == id);
        }

        public void Add(HoloENEntity item)
        {
            _HoloENDbContext.HoloENItems.Add(item);
        }

        public void Delete(int id)
        {
            HoloENEntity HoloENItem = GetSingle(id);
            _HoloENDbContext.HoloENItems.Remove(HoloENItem);
        }

        public HoloENEntity Update(int id, HoloENEntity item)
        {
            _HoloENDbContext.HoloENItems.Update(item);
            return item;
        }

        public IQueryable<HoloENEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<HoloENEntity> _allItems = _HoloENDbContext.HoloENItems.OrderBy(queryParameters.OrderBy,
              queryParameters.IsDescending());

            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.Generation.ToString().Contains(queryParameters.Query.ToLowerInvariant())
                    || x.Name.ToLowerInvariant().Contains(queryParameters.Query.ToLowerInvariant()));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

        public int Count()
        {
            return _HoloENDbContext.HoloENItems.Count();
        }

        public bool Save()
        {
            return (_HoloENDbContext.SaveChanges() >= 0);
        }

        public ICollection<HoloENEntity> GetRandomVtuber()
        {
            List<HoloENEntity> toReturn = new List<HoloENEntity>();

            toReturn.Add(GetRandomItem("Council"));
            toReturn.Add(GetRandomItem("Advent"));
            toReturn.Add(GetRandomItem("Myth"));

            return toReturn;
        }

        private HoloENEntity GetRandomItem(string type)
        {
            return _HoloENDbContext.HoloENItems
                .Where(x => x.Type == type)
                .OrderBy(o => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}
