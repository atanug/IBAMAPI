using Microsoft.EntityFrameworkCore;
using IBAM.API.Models;

namespace IBAM.API.Data{
    public class DataContext : DbContext{
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Country> Countries {get;set;}
        public DbSet<State> States {get;set;}
        public DbSet<Member> Members{get;set;}
        public DbSet<User> Users{get;set;}
        public DbSet<PaymentType> PaymentTypes{get;set;}
        public DbSet<MembershipType> MembershipTypes{get;set;}
        public DbSet<Membership> Memberships{get;set;}
        public DbSet<Event> Events{get;set;}
        public DbSet<RegistrationType> RegistrationTypes{get;set;}
        public DbSet<Registration> Registrations{get;set;}


    }
}