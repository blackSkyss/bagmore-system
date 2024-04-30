using BAL.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Validator
{
    public class SupplierCreatedProductValidator : AbstractValidator<SupplierCreatedProductViewModel>
    {
        public SupplierCreatedProductValidator()
        {
            #region supplierId
            // CategoryId
            RuleFor(p => p.SupplierId)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull().WithMessage("Please enter supplier id");
            #endregion
        }
    }
}
