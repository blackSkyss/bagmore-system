using BAL.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Validator
{
    public class DeliveryMethodValidator : AbstractValidator<EditDeliveryMethodViewModel>
    {
        public DeliveryMethodValidator()
        {
            #region Name
            RuleFor(d => d.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty!")
                .NotNull().WithMessage("{PropertyName} is null!")
                .Length(5, 45).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters!");
            #endregion

            #region Price
            RuleFor(d => d.Price)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty!")
                .NotNull().WithMessage("{PropertyName} is null"!)
                .Must(p => p >= 1 && p <= 100000000).WithMessage("{PropertyName} from 1 to 100000000");

            #endregion

            #region Description
            RuleFor(d => d.Description)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty!")
                .NotNull().WithMessage("{PropertyName} is null"!)
                .Length(5, 190).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters!");
            #endregion

        }
    }
}
