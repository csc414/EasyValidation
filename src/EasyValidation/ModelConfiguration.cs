using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EasyValidation
{
    public class ModelConfiguration
    {
        private Dictionary<PropertyInfo, PropertyValidator> _validators = new Dictionary<PropertyInfo, PropertyValidator>();

        public ModelConfiguration(Type modelType)
        {
            ModelType = modelType;
        }

        public Type ModelType { get; }

        public IEnumerable<PropertyValidator> Validators => _validators.Values;

        public PropertyValidator GetValidator(PropertyInfo propertyInfo)
        {
            if (!_validators.TryGetValue(propertyInfo, out var validator))
                _validators.Add(propertyInfo, validator = new PropertyValidator(propertyInfo));
            return validator;
        }
    }
}
