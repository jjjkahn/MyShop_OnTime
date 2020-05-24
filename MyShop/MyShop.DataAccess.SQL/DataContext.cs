using MyShop.Core.Models;
using System.Data.Entity;

namespace MyShop.DataAccess.SQL
{
    public class DataContext : DbContext
    {
        //Underline DB to check DB conectionstrings
        public DataContext() : base("DefaultConnection")
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

    }
}
