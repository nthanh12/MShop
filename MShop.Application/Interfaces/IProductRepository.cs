using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Application.DTOs;
using MShop.Application.Enums;
using MShop.Domain.Entities;

namespace MShop.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedResult<Product>> Search_Paging(string searchValue, EnumSearchType searchType, EnumOrderBy orderBy, bool isDescendingOrder, int pageNumber, int pageSize);
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}
