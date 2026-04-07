using FluentValidation;
using ProductManagementSolution.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementSolution.Application.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.ProductName)
                .NotEmpty().WithMessage("Product Name is Required")
                .MaximumLength(255).WithMessage("This Name is too long ! , Maximum length is 255 character");

            RuleFor(p => p.CreatedBy)
                .NotEmpty().WithMessage("This Field is Required");
        }
    }
}
