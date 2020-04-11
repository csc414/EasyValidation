﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EasyValidation.Rules
{
    public class PredicateValidationRule : ValidationRule
    {
        public delegate bool Predicate(object instance, object propertyValue, PropertyValidationContext propertyValidatorContext);

        private readonly Predicate _predicate;

        public PredicateValidationRule(Predicate predicate)
        {
            Check.IfNullThrow(predicate, "A predicate Cannot be null.");

            _predicate = predicate;
        }

        protected override string DefaultErrorMessage => "{PropertyName} 未符合指定的条件。";

        public override bool IsValid(PropertyValidationContext context)
        {
            if (!_predicate(context.Instance, context.PropertyValue, context))
                return false;

            return true;
        }
    }
}
