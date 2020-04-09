
using EasyValidation.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyValidation
{
    public class PropertyValidatorBuilder
    {
        public PropertyValidatorBuilder(PropertyValidator validator)
        {
            Validator = validator;
        }

        public PropertyValidator Validator { get; }

        public PropertyValidatorBuilder DisplayName(string name)
        {
            Validator.DisplayName = name;
            return this;
        }

        public PropertyValidatorBuilder NotNull(string message = null)
        {
            Validator.Rules.Add(new NotNullValidationRule { ErrorMessage = message });
            return this;
        }

        public PropertyValidatorBuilder NotEmpty(string message = null)
        {
            var defaultValue = GetDefaultValue(Validator.PropertyInfo.PropertyType);
            Validator.Rules.Add(new NotEmptyValidationRule (defaultValue) { ErrorMessage = message });
            return this;
        }

        private object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            return null;
        }
    }
}
