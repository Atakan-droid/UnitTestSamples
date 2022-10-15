using Microsoft.EntityFrameworkCore;
using UnitTestMVC.Models;

namespace UnitTestMVC.Test.ProductTest
{
    public class ProductControleTest
    {
        protected DbContextOptions<DbForUntTestContext> _contextOptions { get; private set; }

        public void setContextOptions(DbContextOptions<DbForUntTestContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }

        public void Seed()
        {
            using (DbForUntTestContext context = new DbForUntTestContext(_contextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Categories.AddRange(
                    new Category() { Name = "Kalemler" },
                    new Category() { Name = "Defterler" }
                    );
                context.SaveChanges();
                context.Products.AddRange(
                    new Product()
                    { CategoryId = 1, Name = "Kalem 10", Price = 100, Stock = 100, Color = "Kırmızı" },
                    new Product()
                    { CategoryId = 1, Name = "Kalem 20", Price = 100, Stock = 100, Color = "Kırmızı" },
                    new Product()
                    { CategoryId = 1, Name = "Kalem 30", Price = 100, Stock = 100, Color = "Mavi" }
                    );
                context.SaveChanges();
            }
        }
    }
}
