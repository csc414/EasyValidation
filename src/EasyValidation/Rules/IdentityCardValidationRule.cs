using System.Text.RegularExpressions;

namespace EasyValidation.Rules
{
    public class IdentityCardValidationRule : RegularExpressionValidationRule
    {
        private const string Pattern = @"^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})([0-9]|X)$";

        protected override string DefaultErrorMessage => "{PropertyName} 不是有效的电子邮件地址。";

        public IdentityCardValidationRule() : base(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline)
        {
        }
    }
}