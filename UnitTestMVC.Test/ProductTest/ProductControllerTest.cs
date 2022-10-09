using Microsoft.AspNetCore.Mvc;
using Moq;
using UnitTestMVC.Controllers;
using UnitTestMVC.Models;
using UnitTestMVC.Repositories;
using Xunit;

namespace UnitTestMVC.Test.ProductTest
{
    public class ProductControllerTest
    {
        private readonly Mock<IRepository<Product>> _repository;
        private readonly ProductsController _productsController;

        private List<Product> _products;
        public ProductControllerTest()
        {
            _repository = new Mock<IRepository<Product>>();
            _productsController = new ProductsController(_repository.Object);

            _products = new List<Product>()
            {
                new Product() { Id = 1, Name = "Pencil",Price = 100,Color = "Blue",Stock = 10},new Product() { Id = 2, Name = "Car",Price = 10,Color = "Red",Stock = 15},new Product() { Id = 3, Name = "Eraser",Price = 1,Color = "Green",Stock = 100},new Product() { Id = 4, Name = "Edible",Price = 150,Color = "Invok",Stock = 11},
            };
        }
        [Fact]
        public async Task Index_ActionExecutes_ReturnView()
        {
            var result = await _productsController.Index();

            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async Task Index_ActionExecutes_ReturnProductList()
        {
            _repository.Setup(repo => repo.GeAll()).ReturnsAsync(_products);
            var result = await _productsController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            var productList = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);

            Assert.Equal<int>(4, productList.Count());
        }
        [Fact]
        public async Task Details_IdIsNull_ReturnRedirectToAction()
        {
            var result = await _productsController.Details(null);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", viewResult.ActionName);
        }
        [Fact]
        public async Task Details_IdInvalid_ReturnNotFound()
        {
            Product product = null;
            _repository.Setup(x => x.GetById(0)).ReturnsAsync(product);

            var result = await _productsController.Details(0);

            var viewResult = Assert.IsType<NotFoundResult>(result);

        }
        [Fact]
        public async Task Details_IdInvalid_ReturnProduct()
        {
            _repository.Setup(x => x.GetById(1)).ReturnsAsync(_products[0]);

            var result = await _productsController.Details(1);

            var viewResult = Assert.IsType<ViewResult>(result);

            var resultProduct = Assert.IsAssignableFrom<Product>(viewResult.Model);
            Assert.Equal(_products[0].Name, resultProduct.Name);

        }
        [Fact]
        public void Create_ActionExecuted_ReturnView()
        {
            var result = _productsController.Create();

            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async Task Create_InvalidModelState()
        {
            _productsController.ModelState.AddModelError("Name", "Name alanı boş olamaz");
            var result = await _productsController.Create(_products.First());

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.IsType<Product>(viewResult.Model);
        }
        [Fact]
        public async Task Create_ValidModelState_ReturnRedirectToIndexAction()
        {
            var result = await _productsController.Create(_products.First());

            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", viewResult.ActionName);
        }
        [Fact]
        public async Task Create_ValidModelState_CreateMethodExecute()
        {
            Product product = null;

            _repository.Setup(x => x.Create(It.IsAny<Product>())).Callback<Product>(x => product = x);

            var result = await _productsController.Create(_products.First());

            _repository.Verify(repo => repo.Create(It.IsAny<Product>()), times: Times.Once);

            Assert.Equal(_products.First().Id, product.Id);
        }
        [Fact]
        public async Task Create_InvalidModelState_NeverCreateExecute()
        {
            _productsController.ModelState.AddModelError("Name", "");

            var result = await _productsController.Create(_products.First());

            _repository.Verify(repo => repo.Create(It.IsAny<Product>()), times: Times.Never);
        }
        [Fact]
        public async void Edit_IdIsNull_ReturnRedirecttoActionView()
        {
            var result = await _productsController.Edit(null);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
        [Fact]
        public async Task Edit_IdInvalid_ReturnNotFound()
        {
            var product = new Product();
            _repository.Setup(x => x.GetById(0)).ReturnsAsync(product);

            var result = await _productsController.Edit(-1);

            var redirect = Assert.IsType<NotFoundResult>(result);

            Assert.Equal<int>(404, redirect.StatusCode);

        }
        [Fact]
        public async Task Edit_ActionExecute_ReturnProduct()
        {
            var product = _products.First(x => x.Id == 1);

            _repository.Setup(x => x.GetById(1)).ReturnsAsync(product);

            var result = await _productsController.Edit(1);

            var viewResult = Assert.IsType<ViewResult>(result);

            var resultPorudtc = Assert.IsAssignableFrom<Product>(viewResult.Model);

            Assert.Equal(_products.First(x => x.Id == 1).Name, product.Name);

        }
        [Fact]
        public async Task EditPOST_IdIsNotEquel_ReturnNotFound()
        {
            var product = _products.First(x => x.Id == 2);

            var result = await _productsController.Edit(1, product);

            var viewResult = Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task EditPOST_ModelNotValidState_ReturnView()
        {
            _productsController.ModelState.AddModelError("Name", "");

            var result = await _productsController.Edit(1, _products.First(x => x.Id == 1));

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.IsType<Product>(viewResult.Model);
        }
        [Fact]
        public async Task EditPOST_ValidModelState_ReturnRedirectToIndex()
        {
            var result = await _productsController.Edit(1, _products.First(x => x.Id == 1));

            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", viewResult.ActionName);
        }
        [Fact]
        public async Task EditPOST_ValidModelState_ReturnUpdateMethodExecute()
        {
            var prodouct = _products.First(x => x.Id == 1);
            _repository.Setup(x => x.Update(prodouct));

            var result = await _productsController.Edit(1, _products.First(x => x.Id == 1));

            _repository.Verify(y => y.Update(It.IsAny<Product>()), Times.Once);
        }
        [Fact]
        public async Task Delete_ReturnDeleteMethodExecute()
        {
            var prodouct = _products.First(x => x.Id == 1);
            _repository.Setup(x => x.Delete(prodouct));

            var result = await _productsController.Delete(1);

            _repository.Verify(y => y.Delete(It.IsAny<Product>()), Times.Once);
        }
    }
}
