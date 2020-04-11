using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EasyValidation.Rules
{
    public class RegularExpressionValidationRule : ValidationRule
    {
        private Func<object, Regex> _func;

        public RegularExpressionValidationRule(string pattern, RegexOptions options) : this(o => pattern, options) { }

        public RegularExpressionValidationRule(Func<object, string> func, RegexOptions options)
        {
            _func = o => CreateRegex(func(o), options);
        }

        protected override string DefaultErrorMessage => "{PropertyName} 的格式不正确。";

        public override bool IsValid(PropertyValidationContext context)
        {
            if (context.PropertyValue == null)
                return true;

            var regex = _func(context.Instance);
            return regex.IsMatch((string) context.PropertyValue);
        }

        private static Regex CreateRegex(string expression, RegexOptions options = RegexOptions.None)
        {
            return new Regex(expression, options, TimeSpan.FromSeconds(1.5));
        }
    }
}
