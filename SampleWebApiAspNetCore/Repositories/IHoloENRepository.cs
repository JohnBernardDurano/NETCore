using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Repositories
{
    public interface IHoloENRepository
    {
        HoloENEntity GetSingle(int id);
        void Add(HoloENEntity item);
        void Delete(int id);
        HoloENEntity Update(int id, HoloENEntity item);
        IQueryable<HoloENEntity> GetAll(QueryParameters queryParameters);
        ICollection<HoloENEntity> GetRandomVtuber();
        int Count();
        bool Save();
    }
}
