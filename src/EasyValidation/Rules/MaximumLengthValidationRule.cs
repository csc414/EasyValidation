namespace EasyValidation.Rules
{
    public class MaximumLengthValidationRule : LengthValidationRule
    {
        public MaximumLengthValidationRule(int max) : base(0, max)
        {
        }

        protected override string DefaultErrorMessage => "{PropertyName} 必须小于或等于{MaxLength}个字符。您输入了{TotalLength}个字符。";
    }
}