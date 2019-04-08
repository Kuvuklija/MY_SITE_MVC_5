using System.Data.Entity;
using MyFirstSite.Domain.Entities;

namespace MyFirstSite.Domain.Concrete{

    //for assosiated model Product with base
    public class EFDbContext : DbContext{
        public DbSet<Product> Products { get; set; } //get all repository? is it good?
    }
}
