using System;
using System.Collections.Generic;
using System.Reflection;

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

        public bool Inherited { get; set; }

        public Enum InheritedGroup { get; set; }

        public IEnumerable<PropertyValidator> Validators
        {
            get
            {
                foreach (var item in _validators.Values)
                    yield return item;

                if(Inherited)
                {
                    var baseType = ModelType.BaseType;
                    while (baseType != typeof(object))
                    {
                        var configuration = ValidationModels.GetConfiguration(baseType, InheritedGroup);
                        if(configuration != null)
                        {
                            foreach (var item in configuration.Validators)
                                yield return item;
                        }
                        baseType = baseType.BaseType;
                    }
                }
            }
        }

        public PropertyValidator GetValidator(PropertyInfo propertyInfo)
        {
            if (propertyInfo.DeclaringType != ModelType)
                throw new ArgumentException($"{propertyInfo.Name} belong to {propertyInfo.DeclaringType.Name}, not {ModelType.Name}", nameof(propertyInfo));

            if (!_validators.TryGetValue(propertyInfo, out var validator))
                _validators.Add(propertyInfo, validator = new PropertyValidator(propertyInfo));
            return validator;
        }
    }
}