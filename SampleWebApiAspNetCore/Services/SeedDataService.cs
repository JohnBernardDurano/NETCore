using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public class SeedDataService : ISeedDataService
    {
        public void Initialize(HoloENDbContext context)
        {
            context.HoloENItems.Add(new HoloENEntity() { Generation = 2, Type = "Council", Name = "Mumei", Created = DateTime.Now });
            context.HoloENItems.Add(new HoloENEntity() { Generation = 2, Type = "Council", Name = "Kronii", Created = DateTime.Now });
            context.HoloENItems.Add(new HoloENEntity() { Generation = 3, Type = "Advent", Name = "Shiori", Created = DateTime.Now });
            context.HoloENItems.Add(new HoloENEntity() { Generation = 1, Type = "Myth", Name = "Ina", Created = DateTime.Now });
            context.HoloENItems.Add(new HoloENEntity() { Generation = 3, Type = "Advent", Name = "Nerissa", Created = DateTime.Now });
            context.HoloENItems.Add(new HoloENEntity() { Generation = 1, Type = "Myth", Name = "Amelia", Created = DateTime.Now });

            context.SaveChanges();
        }
    }
}
