using System;
using System.Collections.Generic;
using System.Text;

namespace EasyValidation.Rules
{
    public class MinimumLengthValidationRule : LengthValidationRule
    {
        public MinimumLengthValidationRule(int min) : base(min, -1)
        {
        }

        protected override string DefaultErrorMessage => "{PropertyName} 必须大于或等于{MinLength}个字符。您输入了{TotalLength}个字符。";
    }
}
