using AutoMapper;
using ProductManagementSolution.Application.DTOs;
using ProductManagementSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementSolution.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Product, ProductDto>().ReverseMap();
               

            CreateMap<CreateProductDto, Product>();
        }
    }
}
