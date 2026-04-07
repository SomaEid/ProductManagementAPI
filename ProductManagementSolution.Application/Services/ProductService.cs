using AutoMapper;
using ProductManagementSolution.Application.DTOs;
using ProductManagementSolution.Application.Interfaces;
using ProductManagementSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProductManagementSolution.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(PaginationParams @params)
        {
            var products = await _unitOfWork.Products
                .Find(p => true)
                .AsNoTracking()
                .Include(p => p.Items)
                .Skip((@params.PageNumber - 1) * @params.PageSize)
                .Take(@params.PageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            product.CreatedOn = DateTime.Now;
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ProductDto>(product);
        }
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return null;
            return _mapper.Map<ProductDto>(product);
        }


        public async Task<ProductDto?> updateProduct(ProductDto dto)
        {
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(dto.Id);
            if (existingProduct == null) return null;

            _mapper.Map(dto, existingProduct);

            existingProduct.ModifiedOn = DateTime.Now;

            _unitOfWork.Products.Update(existingProduct);

            var roweffected = await _unitOfWork.CompleteAsync();
            if (roweffected == 0) return null;

            return _mapper.Map<ProductDto>(existingProduct);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);
           
            if(existingProduct != null)
            {
                _unitOfWork.Products.Delete(existingProduct);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            return false;
        }
    }
}
