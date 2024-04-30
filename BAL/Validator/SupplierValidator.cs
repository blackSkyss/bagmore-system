using BAL.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Validator
{
    public class SupplierValidator : AbstractValidator<EditSupplierViewModel>
    {
        public SupplierValidator()
        {
            #region Name
            RuleFor(s => s.Name)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("{PropertyName} is empty!")
               .NotNull().WithMessage("{PropertyName} is null!")
               .Length(5, 90).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters!");
            #endregion

            #region Email
            RuleFor(s => s.Email)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty().WithMessage("{PropertyName} is empty!")
              .NotNull().WithMessage("{PropertyName} is null!")
              .Length(1, 90).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters!")
              .Matches("^[a-zA-Z0-9_.+-]+@gmail\\.com$").WithMessage("{PropertyName} format with example@gmail.com!");
            #endregion

            #region Phone
            RuleFor(s => s.Phone)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty().WithMessage("{PropertyName} is empty!")
              .NotNull().WithMessage("{PropertyName} is null!")
              .Matches("^[0-9]*$").WithMessage("{PropertyName} contain only number!")
              .Length(8, 18).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters!");
            #endregion
        }
    }
}
