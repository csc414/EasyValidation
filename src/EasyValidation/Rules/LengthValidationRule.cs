using System;
using System.Collections.Generic;
using System.Text;

namespace EasyValidation.Rules
{
    public class LengthValidationRule : ValidationRule
    {
        private readonly int _min;

        private readonly int _max;

        public LengthValidationRule(int min, int max)
        {
            if (max != -1 && max < min)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "Max should be larger than min.");
            }

            _min = min;
            _max = max;
        }

        protected override string DefaultErrorMessage => "{PropertyName} 的长度必须在 {MinLength} 到 {MaxLength} 字符，您输入了 {TotalLength} 字符。";

        public override bool IsValid(PropertyValidationContext context)
        {
            if (context.PropertyValue == null)
                return true;

            int length = context.PropertyValue.ToString().Length;

            if (length < _min || (length > _max && _max != -1))
            {
                context.FormattedArguments.Add("MinLength", _min);
                context.FormattedArguments.Add("MaxLength", _max);
                context.FormattedArguments.Add("TotalLength", length);
                return false;
            }

            return true;
        }
    }
}
