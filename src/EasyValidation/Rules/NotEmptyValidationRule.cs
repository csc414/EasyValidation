using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyValidation.Rules
{
    public class NotEmptyValidationRule : ValidationRule
    {
        private readonly object _defaultValue;

        public NotEmptyValidationRule(object defaultValue)
        {
            _defaultValue = defaultValue;
        }

        protected override string DefaultErrorMessage => "'{PropertyName}' 不能为空。";

        public override bool IsValid(PropertyValidationContext context)
        {
            switch (context.Value)
            {
                case null:
                case string s when string.IsNullOrWhiteSpace(s):
                case ICollection c when c.Count == 0:
                case Array a when a.Length == 0:
                case IEnumerable e when !e.Cast<object>().Any():
                    return false;
            }
            return !Equals(context.Value, _defaultValue);
        }
    }
}
