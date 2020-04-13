using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyValidation.Rules
{
    public class DataAnnotationRule : ValidationRule
    {
        private readonly IEnumerable<ValidationAttribute> _attributes;

        private string _errorMessage;

        public DataAnnotationRule(ValidationAttribute attribute) : this(new[] { attribute }) { }

        public DataAnnotationRule(IEnumerable<ValidationAttribute> attributes)
        {
            _attributes = attributes;
        }

        protected override string DefaultErrorMessage => _errorMessage;

        public override bool IsValid(PropertyValidationContext context)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(context.Instance);
            validationContext.MemberName = context.PropertyName;
            validationContext.DisplayName = context.DisplayName;
            foreach (var attribute in _attributes)
            {
                var result = attribute.GetValidationResult(context.PropertyValue, validationContext);
                if(result != null)
                {
                    _errorMessage = result.ErrorMessage;
                    return false;
                }
            }
            return true;
        }
    }
}
