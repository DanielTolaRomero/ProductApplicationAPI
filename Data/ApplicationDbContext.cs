using Microsoft.EntityFrameworkCore;
using WebApplicationPractica.Models;

namespace WebApplicationPractica.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }

    }
}
