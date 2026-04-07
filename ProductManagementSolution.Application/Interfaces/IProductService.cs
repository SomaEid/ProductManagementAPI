using ProductManagementSolution.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementSolution.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync(PaginationParams @params);
        Task<ProductDto> CreateProductAsync(CreateProductDto dto);
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto?> updateProduct(ProductDto dto);
        Task<bool> DeleteProduct(int id);
    }
}
