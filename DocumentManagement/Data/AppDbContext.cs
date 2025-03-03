using DocumentManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
    }
}
