using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EasyValidation
{
    public class ModelConfigurationBuilder<T>
    {
        public ModelConfigurationBuilder(ModelConfiguration configuration)
        {
            if (configuration.ModelType != typeof(T))
                throw new ArgumentException("ModelConfiguration -> ModelType does not match the generic type.");

            Configuration = configuration;
        }

        public ModelConfiguration Configuration { get; }

        public PropertyValidatorBuilder<T, TProperty> Property<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var propertyInfo = Helper.GetPropertyInfo(expression);
            Check.IfNullThrow(propertyInfo);

            return new PropertyValidatorBuilder<T, TProperty>(Configuration.GetValidator(propertyInfo));
        }
    }
}
