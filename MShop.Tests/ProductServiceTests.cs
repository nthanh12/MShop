using AutoMapper;
using Moq;
using MShop.Application.DTOs;
using MShop.Application.Interfaces;
using MShop.Application.Services;
using MShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Tests
{
    public class ProductServiceTests
    {
        private readonly ProductService _productService;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IMapper> _mockMapper;

        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();

            _productService = new ProductService( _mockProductRepository.Object, _mockMapper.Object );
        }

        [Fact]
        public async Task AddProductAsync_ShouldAddProduct_WhenValidProductDtoProvided()
        {
            var productDto = new ProductDto { Name = "Test Product", Price = 100.0M }; // Dữ liệu mẫu
            var product = new Product { Id = 1, Name = "Test Product", Price = 100.0M }; // Kết quả mong đợi sau khi map

            // Giả lập hành vi của Mapper
            _mockMapper.Setup(m => m.Map<Product>(productDto)).Returns(product);

            // Giả lập repository không thêm vào Db
            _mockProductRepository.Setup(r => r.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var service = new ProductService(_mockProductRepository.Object, _mockMapper.Object );

            // Act
            await service.AddProductAsync(productDto);

            // Assert
            _mockProductRepository.Verify(r => r.AddAsync(It.Is<Product>(p => p.Name == product.Name && p.Price == product.Price)), Times.Once);
        }
    }
}
