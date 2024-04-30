using BAL.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Validator
{
    public class SuppliedCreatedProductValidator : AbstractValidator<SuppliedProductCreatedViewModel>
    {
        public SuppliedCreatedProductValidator()
        {
            #region 
            //SuppliedProducts
            RuleFor(p => p.Supplier).SetValidator(new SupplierCreatedProductValidator());

            #endregion
        }

    }
}
