using AutoMapper;
using Moq;
using ProductManagementSolution.Application.DTOs;
using ProductManagementSolution.Application.Interfaces;
using ProductManagementSolution.Application.Services;
using ProductManagementSolution.Domain.Entities;

namespace ProductManagementSolution.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ProductService _productService;
        private readonly Mock<IMapper> _mapperMock;
        public ProductServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _productService = new ProductService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact] // دي علامة إن دي ميثود اختبار
        public async Task GetProductById_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange: بنجهز الحالة (ID مش موجود)
            int productId = 999;
            _unitOfWorkMock.Setup(u => u.Products.GetByIdAsync(productId))
                           .ReturnsAsync((Product)null);

            // Act: بننفذ الميثود فعلياً
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert: بنتأكد إن النتيجة فعلاً Null زي ما توقعنا
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnNull_WhenProductDoesNotExist()
        {
            var ProductDto = new ProductDto
            {
                Id = 3,
                ProductName = "Laptop Dell",
                CreatedBy = "Soma",
                CreatedOn = DateTime.UtcNow,
            };

            _unitOfWorkMock.Setup(u => u.Products.GetByIdAsync(ProductDto.Id)).ReturnsAsync((Product)null); 


            var result = await _productService.updateProduct(ProductDto);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnProductDto_WhenProductExist()
        {
            var ProductDto = new ProductDto
            {
                Id = 2,
                ProductName = "Laptop Dell",
                CreatedBy = "Soma",
                CreatedOn = DateTime.UtcNow,
            };


            var mockProduct = new Product
            {
                Id = 2,
                ProductName = "old Laptop Dell",
                CreatedBy = "Soma",
                CreatedOn = DateTime.UtcNow,
            };

            _unitOfWorkMock.Setup(u => u.Products.GetByIdAsync(ProductDto.Id)).ReturnsAsync(mockProduct);

            _mapperMock.Setup(m => m.Map<ProductDto>(mockProduct)).Returns(ProductDto);

            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _productService.updateProduct(ProductDto);

            Assert.NotNull(result);
            Assert.Equal(ProductDto.Id , result.Id);

            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.Products.Update(It.IsAny<Product>()), Times.Once);

        }

        [Fact]
        public async Task GetProductById_ShouldReturnProductDto_WhenProductExists()
        {
            int validId = 2;
            var mockProduct = new Product
            {
                Id = validId,
                ProductName = "Laptop Dell",
                CreatedBy = "Soma",
                CreatedOn = DateTime.UtcNow,
            };

            var expectedDto = new ProductDto
            {
                Id = validId,
                ProductName = "Laptop Dell",
                CreatedBy = "Soma",
                CreatedOn = DateTime.UtcNow,
            };

            _unitOfWorkMock.Setup(u => u.Products.GetByIdAsync(validId)).ReturnsAsync(mockProduct);

            _mapperMock.Setup(m => m.Map<ProductDto>(mockProduct)).Returns(expectedDto);

            var result = await _productService.GetProductByIdAsync(validId);

            Assert.NotNull(result);
            Assert.Equal(validId, result.Id);
            Assert.Equal("Laptop Dell", result.ProductName);

            _unitOfWorkMock.Verify(u => u.Products.GetByIdAsync(validId), Times.Once);

        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnTrue_WhenProductDeleted()
        {
            var validId = 2;

            var mockProduct = new Product
            {
                Id = validId,
                ProductName = "Laptop Dell",
                CreatedBy = "Soma",
                CreatedOn = DateTime.UtcNow,
            };

            _unitOfWorkMock.Setup(u => u.Products.GetByIdAsync(validId)).ReturnsAsync(mockProduct);

            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result =await _productService.DeleteProduct(validId);

            Assert.True(result);

            _unitOfWorkMock.Verify(u => u.Products.Delete(mockProduct), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }
        [Fact]
        public async Task DeleteProduct_ShouldReturnFalse_WhenProductNotExist()
        {
            var notValidId = 1;

            var mockProduct = new Product
            {
                Id = notValidId,
                ProductName = "Laptop Dell",
                CreatedBy = "Soma",
                CreatedOn = DateTime.UtcNow,
            };

            _unitOfWorkMock.Setup(u => u.Products.GetByIdAsync(notValidId)).ReturnsAsync((Product)null);


            var result = await _productService.DeleteProduct(notValidId);

            Assert.False(result);

            _unitOfWorkMock.Verify(u => u.Products.Delete(It.IsAny<Product>()), Times.Never);

            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateProduct_ReturnProductDto_WhenCreateProductDtoIsValid()
        {
            var createProductDto = new CreateProductDto
            {
                ProductName = "Test",
                CreatedBy = "Soma Eid"
            };
            var mockProduct = new Product
            {
                ProductName = "Test",
                CreatedBy = "Soma Eid",
                CreatedOn = DateTime.UtcNow
            };
            var expectedDto = new ProductDto { 
                Id = 3,
                ProductName = "Test",
                CreatedBy = "Soma Eid",
                CreatedOn = DateTime.UtcNow
            };

            _mapperMock.Setup(m => m.Map<Product>(createProductDto)).Returns(mockProduct);
            _mapperMock.Setup(m => m.Map<ProductDto>(mockProduct)).Returns(expectedDto);

            _unitOfWorkMock.Setup(u => u.Products.AddAsync(mockProduct));

            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result =await _productService.CreateProductAsync(createProductDto);

            Assert.NotNull(result);
            Assert.Equal("Test", result.ProductName);

            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.Products.AddAsync(mockProduct), Times.Once);

        }

    }
}
