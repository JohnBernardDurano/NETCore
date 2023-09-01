using Microsoft.EntityFrameworkCore;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories
{
    public class HoloENDbContext : DbContext
    {
        public HoloENDbContext(DbContextOptions<HoloENDbContext> options)
            : base(options)
        {
        }

        public DbSet<HoloENEntity> HoloENItems { get; set; } = null!;
    }
}
