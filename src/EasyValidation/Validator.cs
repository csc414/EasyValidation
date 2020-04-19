using System;
using System.Linq;

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

        public static ValidationResult Validate<T>(T instance) => Validate(new ValidationContext(instance), strategy: null);

        public static ValidationResult Validate<T>(T instance, Action<StrategyBuilder<T>> buildAction)
        {
            var builder = new StrategyBuilder<T>();
            buildAction?.Invoke(builder);
            return Validate(new ValidationContext(instance), builder.Strategy);
        }

        public static ValidationResult Validate<T>(T instance, Strategy strategy) => Validate(new ValidationContext(instance), strategy);

        public static ValidationResult Validate(ValidationContext context, Strategy strategy)
        {
            var result = new ValidationResult();
            var configuration = ValidationModels.Configuration(context.InstanceType);
            var validators = configuration.Validators;
            if (strategy != null)
            {
                if (strategy.IncludeProperties.Any() && strategy.ExcludeProperties.Any())
                    throw new ArgumentException("Include or Exclude, you can only choose one.");

                if (strategy.IncludeProperties.Any())
                    validators = validators.Where(o => strategy.IncludeProperties.Contains(o.Descriptor.PropertyInfo));
                else if (strategy.ExcludeProperties.Any())
                    validators = validators.Where(o => !strategy.ExcludeProperties.Contains(o.Descriptor.PropertyInfo));
            }

            var failures = validators.Select(o => o.Validate(context)).Where(o => o != null);
            return result.AddErrors(failures);
        }
    }
}