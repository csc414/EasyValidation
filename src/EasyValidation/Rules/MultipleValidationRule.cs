using System;
using System.Collections.Generic;
using System.Text;

namespace EasyValidation.Rules
{
    public class MultipleValidationRule : ValidationRule
    {
        private string _errorMessage;

        public MultipleValidationRule(PropertyValidator validator)
        {
            Validator = validator;
        }

        public PropertyValidator Validator { get; }

        protected override string DefaultErrorMessage => _errorMessage;

        public override bool IsValid(PropertyValidationContext context)
        {
            var failure = Validator.Validate(context.Context);
            if (failure == null)
                return true;

            _errorMessage = failure.ErrorMessage;
            return false;
        }
    }
}
