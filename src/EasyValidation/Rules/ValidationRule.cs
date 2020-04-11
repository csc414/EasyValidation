using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EasyValidation.Rules
{
    public abstract class ValidationRule
    {
        public string ErrorMessage { get; set; }

        protected abstract string DefaultErrorMessage { get; }

        public abstract bool IsValid(PropertyValidationContext context);

        public virtual ValidationFailure Validate(PropertyValidationContext context)
        {
            if (IsValid(context))
                return null;

            var failure = new ValidationFailure(context.PropertyInfo.Name);
            failure.FormattedArguments = context.FormattedArguments;
            failure.ErrorMessage = context.BuildErrorMessage(ErrorMessage ?? context.Descriptor.ErrorMessage ?? DefaultErrorMessage);
            return failure;
        }
    }
}
