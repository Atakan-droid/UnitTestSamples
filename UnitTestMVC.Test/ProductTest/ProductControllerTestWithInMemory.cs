using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnitTestMVC.Controllers;
using UnitTestMVC.Models;
using UnitTestMVC.Repositories;
using Xunit;

namespace UnitTestMVC.Test.ProductTest
{
    public class ProductControllerTestWithInMemory : ProductControleTest
    {
        public ProductControllerTestWithInMemory()
        {
            setContextOptions(new DbContextOptionsBuilder<DbForUntTestContext>().UseInMemoryDatabase("UnitTestDbInMemory").Options);
            Seed();
        }

        [Fact]
        public async Task Create_ModelValidProduct_ReturnsRedirectToActionWithProduct()
        {
            var newProduct = new Product { Name = "Kalem", Price = 200, Stock = 30 };

            using (var context = new DbForUntTestContext(_contextOptions))
            {
                var category = context.Categories.First();
                newProduct.CategoryId = category.Id;
                var repository = new Repository<Product>(context);
                var controller = new ProductsController(repository);

                var result = await controller.Create(newProduct);

                var redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);

            }
            using (var context = new DbForUntTestContext(_contextOptions))
            {
                var product = context.Products.FirstOrDefault(x => x.Name == newProduct.Name);

                Assert.NotNull(product);
                Assert.Equal(newProduct.Name, product.Name);
            }

        }
    }
}
