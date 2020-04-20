using System;
using System.Linq;
using System.Linq.Expressions;

namespace EasyValidation
{
    public static class Validator
    {
        public static ModelConfigurationBuilder<T> Model<T>()
        {
            var configuration = ValidationModels.GetOrCreateConfiguration<T>();
            return new ModelConfigurationBuilder<T>(configuration);
        }

        public static ModelConfigurationBuilder<T> Model<T>(Enum group)
        {
            var configuration = ValidationModels.GetOrCreateConfiguration<T>(group);
            return new ModelConfigurationBuilder<T>(configuration);
        }

        public static void Model<T>(Action<ModelConfigurationBuilder<T>> buildAction)
        {
            var configuration = ValidationModels.GetOrCreateConfiguration<T>();
            buildAction?.Invoke(new ModelConfigurationBuilder<T>(configuration));
        }

        public static void Model<T>(Enum group, Action<ModelConfigurationBuilder<T>> buildAction)
        {
            var configuration = ValidationModels.GetOrCreateConfiguration<T>(group);
            buildAction?.Invoke(new ModelConfigurationBuilder<T>(configuration));
        }

        public static ValidationResult Validate<T>(T instance) => Validate(instance, group: null);

        public static ValidationResult Validate<T>(T instance, Enum group)
        {
            return Validate(instance, group, selector: null);
        }

        public static ValidationResult Validate<T>(T instance, Action<PropertySelector<T>> selectorAction) => Validate(instance, null, selectorAction);

        public static ValidationResult Validate<T>(T instance, Enum group, Action<PropertySelector<T>> selectorAction)
        {
            PropertySelector<T> selector = null;
            if(selectorAction != null)
            {
                selector = new PropertySelector<T>();
                selectorAction.Invoke(selector);
            }
            
            return Validate(instance, group, selector);
        }

        public static ValidationResult Validate<T>(T instance, Enum group, IPropertySelect selector)
        {
            var context = new ValidationContext(instance, group);
            var result = new ValidationResult();
            var configuration = ValidationModels.GetConfiguration(context.InstanceType, context.Group);
            if (configuration == null)
                return result;

            var validators = configuration.Validators;
            if (selector != null)
                validators = selector.Select(validators);

            var failures = validators.Select(o => o.Validate(context)).Where(o => o != null);
            return result.AddErrors(failures);
        }
    }
}