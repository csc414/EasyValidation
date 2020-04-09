using System;
using System.Collections.Generic;
using System.Text;

namespace EasyValidation.Rules
{
    public class NotNullValidationRule : ValidationRule
    {
        protected override string DefaultErrorMessage => "'{PropertyName}' 不能为Null。";

        public override bool IsValid(PropertyValidationContext context)
        {
            if (context.Value == null)
                return false;
            return true;
        }
    }
}
