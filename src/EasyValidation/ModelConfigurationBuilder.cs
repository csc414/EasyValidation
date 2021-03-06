﻿using System;
using System.Linq.Expressions;

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

        public void Inherited(Enum group = null)
        {
            Configuration.Inherited = true;
            Configuration.InheritedGroup = group;
        }
    }
}