using System.Collections.Generic;

namespace EasyValidation.Rules
{
    public class NotValidationRule : ValidationRule
    {
        private readonly IEnumerable<ValidationRule> _rules;

        public NotValidationRule(ValidationRule rule) : this(new[] { rule })
        {
        }

        public NotValidationRule(IEnumerable<ValidationRule> rules)
        {
            _rules = rules;
        }

        protected override string DefaultErrorMessage => "{PropertyName} 未符合指定的条件。";

        public override bool IsValid(PropertyValidationContext context)
        {
            foreach (var rule in _rules)
            {
                if (rule.IsValid(context))
                    return false;
            }
            return true;
        }
    }
}