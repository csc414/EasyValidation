using System;
using System.Linq;
using System.Linq.Expressions;

namespace EasyValidation
{
    public static class Validator
    {
        public static ModelConfigurationBuilder<T> Model<T>()
        {
            var configuration = ValidationModels.Configuration<T>();
            return new ModelConfigurationBuilder<T>(configuration);
        }

        public static void Model<T>(Action<ModelConfigurationBuilder<T>> buildAction)
        {
            var configuration = ValidationModels.Configuration<T>();
            buildAction?.Invoke(new ModelConfigurationBuilder<T>(configuration));
        }

        public static ValidationResult Validate(object instance) => Validate(new ValidationContext(instance));

        public static ValidationResult Validate(ValidationContext context)
        {
            var result = new ValidationResult();
            var configuration = ValidationModels.Configuration(context.InstanceType);
            var failures = configuration.Validators.Select(o => o.Validate(context)).Where(o => o != null);
            return result.AddErrors(failures);
        }
    }
}
