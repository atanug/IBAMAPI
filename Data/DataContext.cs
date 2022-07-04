using Microsoft.EntityFrameworkCore;
using IBAM.API.Models;

namespace IBAM.API.Data{
    public class DataContext : DbContext{
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Country> Countries {get;set;}
        public DbSet<State> States {get;set;}
    }
}