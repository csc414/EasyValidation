using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EasyValidation.Rules
{
    public abstract class ValidationRule
    {
        private const string Pattern = @"\{([a-zA-Z0-9]+?)(:([^\{\}]+))?\}";

        private static Regex _regex = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public string ErrorMessage { get; set; }

        protected abstract string DefaultErrorMessage { get; }

        public abstract bool IsValid(PropertyValidationContext context);

        public virtual ValidationFailure Validate(PropertyValidationContext context)
        {
            if (IsValid(context))
                return null;

            var failure = new ValidationFailure(context.PropertyInfo.Name);
            failure.FormattedArguments.Add("PropertyName", context.DisplayName ?? context.PropertyInfo.Name);
            BuildFormattedArguments(failure.FormattedArguments);
            failure.ErrorMessage = FormatedErrorMessage(failure.FormattedArguments);
            return failure;
        }

        protected virtual void BuildFormattedArguments(IDictionary<string, object> arguments) { }

        private string FormatedErrorMessage(IDictionary<string, object> arguments)
        {
            return _regex.Replace(ErrorMessage ?? DefaultErrorMessage, match =>
            {
                var name = match.Result("$1");
                if (arguments.TryGetValue(name, out object value))
                    return value?.ToString();
                return match.Value;
            });
        }
    }
}
