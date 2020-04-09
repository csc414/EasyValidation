using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EasyValidation
{
    public class PropertyValidationContext
    {
        private readonly ValidationContext _context;

        private readonly PropertyValidator _propertyValidator;

        private object _value;

        public PropertyValidationContext(ValidationContext context, PropertyValidator propertyValidator)
        {
            _context = context;
            _propertyValidator = propertyValidator;
        }

        public Type InstanceType => _context.InstanceType;

        public PropertyInfo PropertyInfo => _propertyValidator.PropertyInfo;

        public string PropertyName => PropertyInfo.Name;

        public string DisplayName => _propertyValidator.DisplayName;

        public object Instance => _context.Instance;

        public object Value => _value ?? (_value = PropertyInfo.GetValue(_context.Instance));
    }
}
