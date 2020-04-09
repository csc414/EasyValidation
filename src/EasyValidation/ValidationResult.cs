using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyValidation
{
    public sealed class ValidationResult
	{
		private readonly List<ValidationFailure> _errors;

		public ValidationResult() : this(Enumerable.Empty<ValidationFailure>()) { }

		public ValidationResult(IEnumerable<ValidationFailure> failures)
		{
			_errors = new List<ValidationFailure>(failures);
		}

		public IEnumerable<ValidationFailure> Errors => _errors;

		public ValidationResult AddError(ValidationFailure failures)
		{
			_errors.Add(failures);
			return this;
		}

		public ValidationResult AddErrors(IEnumerable<ValidationFailure> failures)
		{
			_errors.AddRange(failures);
			return this;
		}

		public bool IsValid => _errors.Count == 0;

		public override string ToString()
		{
			return ToString(Environment.NewLine);
		}

		public string ToString(string separator)
		{
			return string.Join(separator, Errors.Select(failure => failure.ErrorMessage));
		}
	}
}
