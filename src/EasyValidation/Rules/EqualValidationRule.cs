using System;
using System.Collections;

namespace EasyValidation.Rules
{
    public class EqualValidationRule : ValidationRule
    {
        private readonly Delegate _delegate;

        private readonly IEqualityComparer _comparer;

        private readonly object _comparisonValue;

        public EqualValidationRule(object comparisonValue, IEqualityComparer comparer = null)
        {
            _comparisonValue = comparisonValue;
            _comparer = comparer;
        }

        public EqualValidationRule(Delegate @delegate, IEqualityComparer comparer = null)
        {
            _delegate = @delegate;
            _comparer = comparer;
        }

        protected override string DefaultErrorMessage => "{PropertyName} 不能和 {ComparisonValue} 相等。";

        public override bool IsValid(PropertyValidationContext context)
        {
            var value = GetComparisonValue(context);
            if (!Compare(context.PropertyValue, value))
            {
                context.FormattedArguments.Add("ComparisonValue", value);
                return false;
            }
            return true;
        }

        private bool Compare(object propertyValue, object value)
        {
            if (_comparer != null)
                return _comparer.Equals(value, propertyValue);

            return Equals(value, propertyValue);
        }

        private object GetComparisonValue(PropertyValidationContext context)
        {
            if (_delegate != null)
                return _delegate.DynamicInvoke(context.Instance);

            return _comparisonValue;
        }
    }
}