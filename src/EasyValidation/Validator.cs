using System;
using System.Linq;
using System.Linq.Expressions;

namespace EasyValidation
{
    public static class Validator
    {
        public static ValidationResult Validate(object instance) => Validate(new ValidationContext(instance));

        public static ValidationResult Validate(ValidationContext context)
        {
            var result = new ValidationResult();
            var validators = ValidationObjects.GetValidators(context.InstanceType);
            var failures = validators.Select(o => o.Validate(context)).Where(o => o != null);
            return result.AddErrors(failures);
        }
    }
}
