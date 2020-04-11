using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyValidation.Rules
{
    public class NotValidationRule : ValidationRule
    {
        public NotValidationRule(IEnumerable<ValidationRule> rules)
        {
            Rules = rules;
        }

        public NotValidationRule(ValidationRule rule)
        {
            Rules = new[] { rule };
        }

        public IEnumerable<ValidationRule> Rules { get; }

        protected override string DefaultErrorMessage => "{PropertyName} 未符合指定的条件。";

        public override bool IsValid(PropertyValidationContext context)
        {
            foreach (var rule in Rules)
            {
                if (rule.IsValid(context))
                    return false;
            }
            return false;
        }
    }
}
