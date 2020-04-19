using System.Collections.Generic;

namespace EasyValidation
{
    public sealed class ValidationFailure
    {
        public ValidationFailure(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }

        public string ErrorMessage { get; set; }

        public IDictionary<string, object> FormattedArguments { get; set; }

        public override string ToString()
        {
            return ErrorMessage;
        }
    }
}