using BAL.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Validator
{
    public class DescribeProductsValidator : AbstractValidator<DescribleProductForCreateNewViewModel>
    {
        public DescribeProductsValidator()
        {
            #region ColorId
            //ColorId
            RuleFor(d => d.ColorId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter {PropertyName}")
                .NotNull().WithMessage("Please enter {PropertyName}");
            #endregion

            #region SizeId
            //SizeId
            RuleFor(d => d.SizeId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter {PropertyName}")
                .NotNull().WithMessage("Please enter {PropertyName}");
            #endregion

            #region Price
            //Price
            RuleFor(d => d.Price)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter {PropertyName}")
                .NotNull().WithMessage("Please enter {PropertyName}");
            #endregion

            #region Amount
            // Amount
            RuleFor(d => d.Price)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter {PropertyName}")
                .NotNull().WithMessage("Please enter {PropertyName}");
            #endregion

            #region OriginalPrice
            // Amount
            RuleFor(d => d.OriginalPrice)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter {PropertyName}")
                .NotNull().WithMessage("Please enter {PropertyName}");
            #endregion
        }
    }
}
