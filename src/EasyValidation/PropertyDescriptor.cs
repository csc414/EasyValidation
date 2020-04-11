using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EasyValidation
{
    public sealed class PropertyDescriptor
    {
        public PropertyDescriptor(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        public PropertyInfo PropertyInfo { get; }

        public string DisplayName { get; set; }

        public string ErrorMessage { get; set; }
    }
}
