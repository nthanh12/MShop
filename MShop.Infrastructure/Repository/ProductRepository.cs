using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MShop.Application.DTOs;
using MShop.Application.Enums;
using MShop.Application.Interfaces;
using MShop.Domain.Entities;
using MShop.Infrastructure.Data;

namespace MShop.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MShopDbContext _context;
        public ProductRepository(MShopDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PagedResult<Product>> Search_Paging(string searchValue, EnumSearchType searchType, EnumOrderBy orderBy, bool isDescendingOrder, int pageNumber, int pageSize)
        {
            var query = _context.Products.AsQueryable();

            // Thực hiện check điều kiện search
            switch (searchType)
            {
                case EnumSearchType.All:
                    break;

                case EnumSearchType.Name:
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        query = query.Where(p => p.Name.Contains(searchValue));
                    }
                    break;

                default:
                    break;
            }

            // Tính tổng số bản ghi trước khi phân trang
            var totalCount = await query.CountAsync();

            // Thực hiện sắp xếp
            query = orderBy switch
            {
                EnumOrderBy.Name => isDescendingOrder ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                EnumOrderBy.Price => isDescendingOrder ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                _ => isDescendingOrder ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id)
            };

            // Áp dụng phân trang
            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Trả về kết quả phân trang
            return new PagedResult<Product>
            {
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                Items = products
            };
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

    }
}
