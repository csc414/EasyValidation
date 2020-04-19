namespace EasyValidation.Rules
{
    public class NullValidationRule : ValidationRule
    {
        protected override string DefaultErrorMessage => "{PropertyName} 必须为Null。";

        public override bool IsValid(PropertyValidationContext context)
        {
            return context.PropertyValue == null;
        }
    }
}