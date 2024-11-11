using AutoMapper;
using MShop.Application.DTOs;
using MShop.Application.Enums;
using MShop.Application.Interfaces;
using MShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task AddProductAsync(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto); // Chuyển đổi từ Dto sang Enity để thực hiện Add
            await _productRepository.AddAsync(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return false;
            }

            return await _productRepository.DeleteAsync(id);
        }

        public async Task<PagedResult<ProductDto>> GetProductsAsync(string searchValue = "", EnumSearchType searchType = EnumSearchType.All, EnumOrderBy orderBy = EnumOrderBy.ID, bool isDescending = false, int pageNumber = 1, int pageSize = 10)
        {
            var pagedResult = await _productRepository.Search_Paging(searchValue, searchType, orderBy, isDescending, pageNumber, pageSize);

            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(pagedResult.Items);

            return new PagedResult<ProductDto>
            {
                TotalCount = pagedResult.TotalCount,
                TotalPages = pagedResult.TotalPages,
                Items = productDtos
            };
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return null;
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }

        public async Task<bool> UpdateProductAsync(int id, ProductDto productDto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return false;
            }
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;
            product.Description = productDto.Description;

            await _productRepository.UpdateAsync(product);
            return true;
        }
    }
}
