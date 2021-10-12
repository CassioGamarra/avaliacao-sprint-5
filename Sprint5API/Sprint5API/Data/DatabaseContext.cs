using Microsoft.EntityFrameworkCore;
using Sprint5API.Models; 

namespace Sprint5API.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> opt) : base (opt)
        {

        }  
    }
}
