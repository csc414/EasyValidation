using System;

namespace EasyValidation.Rules
{
    public class RangeValidationRule : ValidationRule
    {
        private IComparable _from;

        private IComparable _to;

        public RangeValidationRule(IComparable from, IComparable to)
        {
            if (to.CompareTo(from) == -1)
            {
                throw new ArgumentOutOfRangeException(nameof(to), "To should be larger than from.");
            }

            _from = from;
            _to = to;
        }

        protected override string DefaultErrorMessage => "{PropertyName} 必须在 {From} 到 {To}之间， 您输入了 {Value}。";

        public override bool IsValid(PropertyValidationContext context)
        {
            var propertyValue = (IComparable)context.PropertyValue;
            if (propertyValue == null)
                return true;

            if (propertyValue.CompareTo(_from) < 0 || propertyValue.CompareTo(_to) > 0)
            {
                context.FormattedArguments.Add("From", _from);
                context.FormattedArguments.Add("To", _to);
                context.FormattedArguments.Add("Value", context.PropertyValue);
                return false;
            }
            return true;
        }
    }
}