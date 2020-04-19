namespace EasyValidation.Rules
{
    public class MultipleValidationRule : ValidationRule
    {
        private readonly PropertyValidator _validator;

        private string _errorMessage;

        public MultipleValidationRule(PropertyValidator validator)
        {
            _validator = validator;
        }

        protected override string DefaultErrorMessage => _errorMessage;

        public override bool IsValid(PropertyValidationContext context)
        {
            var failure = _validator.Validate(context.Context);
            if (failure == null)
                return true;

            _errorMessage = failure.ErrorMessage;
            return false;
        }
    }
}