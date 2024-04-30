using BAL.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Validator
{
    public class BrandValidator : AbstractValidator<BrandCreatedViewModel>
    {
        public BrandValidator()
        {
            #region Name
            //Name
            RuleFor(b => b.Name)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("Please enter {PropertyName}")
               .Length(2, 20).WithMessage("{PropertyName} must be between {MinLength}..{MaxLength} characters")
               .NotNull().WithMessage("Please enter {PropertyName}");
            #endregion

            #region Logo
            //Logo
            /*RuleFor(b => b.Logo)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("Please enter {PropertyName}")
               .NotNull().WithMessage("Please enter {PropertyName}");*/
            #endregion 
        }
    }
}
