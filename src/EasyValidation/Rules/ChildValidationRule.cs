using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EasyValidation.Rules
{
    public class ChildValidationRule : ValidationRule
    {
        private readonly Enum _group;

        private string _errorMessage;

        public ChildValidationRule(Enum group)
        {
            _group = group;
        }

        protected override string DefaultErrorMessage => _errorMessage;

        public override bool IsValid(PropertyValidationContext context)
        {
            if (context.PropertyValue == null)
                return true;

            var group = _group ?? context.ParentContext.Group;
            ModelConfiguration configuration = null;
            if(context.PropertyValue is IEnumerable enumerable)
            {
                configuration = ValidationModels.GetConfiguration(context.PropertyInfo.PropertyType.GenericTypeArguments[0], group);
                if (configuration == null)
                    return true;

                foreach (object instance in enumerable)
                {
                    if (!IsValid(configuration, instance, group))
                        return false;
                }
                return true;
            }

            configuration = ValidationModels.GetConfiguration(context.PropertyInfo.PropertyType, group);
            if (configuration == null)
                return true;

            return IsValid(configuration, context.PropertyValue, group);
        }

        private bool IsValid(ModelConfiguration configuration, object instance, Enum group)
        {
            var childContext = new ValidationContext(instance, group);
            foreach (var validator in configuration.Validators)
            {
                var failure = validator.Validate(childContext);
                if (failure != null)
                {
                    _errorMessage = failure.ErrorMessage;
                    return false;
                }
            }
            return true;
        }
    }
}
