using BAL.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Validator
{
    public class CategoryValidator : AbstractValidator<EditCategoryViewModel>
    {
        public CategoryValidator()
        {

            #region Name
            RuleFor(c => c.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty!")
                .NotNull().WithMessage("{PropertyName} is null!")
                .Length(5, 45).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters!")
                .Matches("^[a-zA-Z ]+$").WithMessage("{PropertyName} contain only character!");
            #endregion

        }
    }
}
