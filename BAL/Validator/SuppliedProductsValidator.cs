using BAL.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Validator
{
    public class SuppliedProductsValidator : AbstractValidator<SuppliedProductViewModel>
    {
        public SuppliedProductsValidator()
        {
            #region SupplierProducts
            //SupplierProducts
            RuleFor(s => s.Supplier).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter {PropertyName}")
                .NotNull().WithMessage("Please enter {PropertyName}");
            #endregion



        }
    }
}
