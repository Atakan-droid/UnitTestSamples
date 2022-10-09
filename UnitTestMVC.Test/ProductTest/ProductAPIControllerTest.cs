using Microsoft.AspNetCore.Mvc;
using Moq;
using UnitTestMVC.Controllers;
using UnitTestMVC.Models;
using UnitTestMVC.Repositories;
using Xunit;

namespace UnitTestMVC.Test.ProductTest
{
    public class ProductAPIControllerTest
    {
        private readonly Mock<IRepository<Product>> _repository;
        private readonly ProductsAPIController _productAPIController;

        private List<Product> _products;

        public ProductAPIControllerTest()
        {
            _repository = new Mock<IRepository<Product>>();
            _productAPIController = new ProductsAPIController(_repository.Object);

            _products = new List<Product>()
            {
                new Product() { Id = 1, Name = "Pencil",Price = 100,Color = "Blue",Stock = 10},new Product() { Id = 2, Name = "Car",Price = 10,Color = "Red",Stock = 15},new Product() { Id = 3, Name = "Eraser",Price = 1,Color = "Green",Stock = 100},new Product() { Id = 4, Name = "Edible",Price = 150,Color = "Invok",Stock = 11},
            };
        }
        [Fact]
        public async void GetProducts_ActionExecutes_ReturnOkResultWithProducts()
        {
            _repository.Setup(x => x.GeAll()).ReturnsAsync(_products);

            var result = await _productAPIController.GetProducts();

            var okResult = Assert.IsType<OkObjectResult>(result);

            var returnProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);

            Assert.Equal<int>(4, returnProducts.ToList().Count);
        }
        [Fact]
        public async void GetProduct_IdIsNull_ReturnNotFound()
        {
            Product product = null;
            _repository.Setup(x => x.GetById(0)).ReturnsAsync(product);

            var result = await _productAPIController.GetProduct(0);

            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async void GetProduct_IdValid_ReturnOkResult()
        {
            _repository.Setup(x => x.GetById(1)).ReturnsAsync(_products.First(x => x.Id == 1));

            var result = await _productAPIController.GetProduct(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var product = Assert.IsAssignableFrom<Product>(okResult.Value);
        }
        [Fact]
        public async void PuProduct_IdIsNotEqualProduct_ReturnBadRequestResult()
        {
            var result = _productAPIController.PutProduct(2, product: _products.First(x => x.Id == 1));

            var badrequest = Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async void PutProduct_ActionExecuted_NoContent()
        {
            var product = _products.First(x => x.Id == 1);
            _repository.Setup(x => x.Update(product));

            var result = _productAPIController.PutProduct(1, product: product);

            _repository.Verify(x => x.Update(product), Times.Once);


            var noContentResult = Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async void PostProduct_ActionExecuted_ReturnCreatedAction()
        {
            var product = _products.First();
            _repository.Setup(x => x.Create(product)).Returns(Task.CompletedTask);

            var result = await _productAPIController.PostProduct(product);

            _repository.Verify(x => x.Create(product), Times.Once);

            var createdActionResult = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal("GetProduct", createdActionResult.ActionName);
        }

        [Fact]
        public async void DeleteProduct_IdNotFound_ReturnsNotFound()
        {
            var result = await _productAPIController.DeleteProduct(10);

            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void DeleteProduct_ActionExecutes_ReturnsNoContent()
        {
            var product = _products.First();
            _repository.Setup(x => x.GetById(product.Id)).ReturnsAsync(product);

            _repository.Setup(x => x.Delete(product));
            var result = await _productAPIController.DeleteProduct(1);

            _repository.Verify(x => x.GetById(product.Id), Times.Once);

            _repository.Verify(x => x.Delete(product), Times.Once);

            var NoContent = Assert.IsType<NoContentResult>(result);
        }
    }
}
