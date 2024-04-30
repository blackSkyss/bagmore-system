using BAL.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Validator
{
    public class ProductValidator : AbstractValidator<CreatedProductViewModel>
    {
        public ProductValidator()
        {
            #region Name
            //Name
            RuleFor(p => p.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter name of product")
                .Length(2, 100).WithMessage("{PropertyName} must be between {MinLength}..{MaxLength} characters")
                .NotNull().WithMessage("Please enter name of product");
            #endregion

            #region Composition
            // Composition
            RuleFor(p => p.Composition)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("Please enter composition of product")
               .NotNull().WithMessage("Please enter composition of product");
            #endregion

            #region Description
            // Description
            RuleFor(p => p.Description)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("Please enter description of product")
               .NotNull().WithMessage("Please enter description of product");
            #endregion

            #region Discount
            // Discount
            //lay discount 100
            // neu nguoi dung k nhap discount thi mac dinh == 0
            RuleFor(p => p.Discount)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must be between {From} and {To}")
               .LessThanOrEqualTo(100).WithMessage("{PropertyName} must be between {From} and {To}");
            #endregion

            #region BrandId
            // BrandId
            RuleFor(p => p.BrandId)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull().WithMessage("Please enter brand id of product");
            #endregion

            #region CategoryId
            // CategoryId
            RuleFor(p => p.CategoryId)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull().WithMessage("Please enter category id of product");
            #endregion

            #region DescribeProducts
            //DescribeProducts
            RuleForEach(p => p.DescribeProducts).SetValidator(new DescribeProductsValidator());
            #endregion

        }
    }
}
