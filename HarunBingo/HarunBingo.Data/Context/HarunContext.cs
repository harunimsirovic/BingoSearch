using HarunBingo.Models;
using Microsoft.EntityFrameworkCore;

namespace HarunBingo.Context
{
    public partial class HarunContext : DbContext
    {

        public HarunContext(DbContextOptions<HarunContext> options) : base(options)
        {

        }

        public DbSet<ShopLocation> ShopLocations { get; set; }


    }
}
