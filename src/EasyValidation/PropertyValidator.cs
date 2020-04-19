using EasyValidation.Rules;
using System.Collections.Generic;
using System.Reflection;

namespace EasyValidation
{
    public class PropertyValidator
    {
        public PropertyValidator(PropertyInfo propertyInfo)
        {
            Descriptor = new PropertyDescriptor(propertyInfo);
        }

        public PropertyValidator(PropertyDescriptor descriptor)
        {
            Descriptor = descriptor;
        }

        public PropertyDescriptor Descriptor { get; }

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