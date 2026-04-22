using DeutschArtikelLearnApp.Model;
using Microsoft.EntityFrameworkCore;

namespace DeutschArtikelLearnApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<RightForm> RightForms { get; set; }

    }
}
