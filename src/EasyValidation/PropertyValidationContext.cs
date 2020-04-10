using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace EasyValidation
{
    public class PropertyValidationContext
    {
        private const string Pattern = @"\{([a-zA-Z0-9]+?)(:([^\{\}]+))?\}";

        private static Regex _regex = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private readonly PropertyValidator _propertyValidator;

        private object _value;

        public PropertyValidationContext(ValidationContext context, PropertyValidator propertyValidator)
        {
            Context = context;
            _propertyValidator = propertyValidator;
            FormattedArguments = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase) { { nameof(PropertyName), propertyValidator.Descriptor.DisplayName ?? propertyValidator.Descriptor.PropertyInfo.Name } };
        }

        public ValidationContext Context { get; }

        public object Instance => Context.Instance;

        public Type InstanceType => Context.InstanceType;

        public PropertyInfo PropertyInfo => _propertyValidator.Descriptor.PropertyInfo;

        public string PropertyName => PropertyInfo.Name;

        public object PropertyValue => _value ?? (_value = PropertyInfo.GetValue(Context.Instance));

        public string DisplayName => _propertyValidator.Descriptor.DisplayName;

        public IDictionary<string, object> FormattedArguments { get; }

        public string BuildErrorMessage(string template)
        {
            return _regex.Replace(template, match =>
            {
                var name = match.Result("$1");
                if (FormattedArguments.TryGetValue(name, out object value))
                    return value?.ToString();
                return match.Value;
            });
        }
    }
}
