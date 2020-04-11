using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyValidation.Rules
{
    public class EmptyValidationRule : ValidationRule
    {
        private readonly object _defaultValue;

        public EmptyValidationRule(object defaultValue)
        {
            _defaultValue = defaultValue;
        }

        protected override string DefaultErrorMessage => "{PropertyName} 必须为空。";

        public override bool IsValid(PropertyValidationContext context)
        {
            switch (context.PropertyValue)
            {
                case null:
                case string s when string.IsNullOrWhiteSpace(s):
                case ICollection c when c.Count == 0:
                case Array a when a.Length == 0:
                case IEnumerable e when !e.Cast<object>().Any():
                    return true;
            }
            return Equals(context.PropertyValue, _defaultValue);
        }
    }
}
