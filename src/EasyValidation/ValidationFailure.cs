using System;
using System.Collections.Generic;
using System.Text;

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

		public IDictionary<string, object> FormattedArguments { get; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

		public override string ToString()
		{
			return ErrorMessage;
		}
	}
}
