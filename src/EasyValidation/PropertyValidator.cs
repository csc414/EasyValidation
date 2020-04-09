using EasyValidation.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EasyValidation
{
    public class PropertyValidator
    {
        public PropertyValidator(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        public PropertyInfo PropertyInfo { get; }

        public string DisplayName { get; set; }

        public IList<ValidationRule> Rules { get; } = new List<ValidationRule>();

        public ValidationFailure Validate(ValidationContext context)
        {
            var propertyContext = new PropertyValidationContext(context, this);
            foreach (var rule in Rules)
            {
                var failure = rule.Validate(propertyContext);
                if (failure != null)
                    return failure;
            }
            return null;
        }
    }
}
