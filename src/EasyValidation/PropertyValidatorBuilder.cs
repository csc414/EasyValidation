namespace EasyValidation
{
    public class PropertyValidatorBuilder<T, TProperty>
    {
        public PropertyValidatorBuilder(PropertyValidator validator)
        {
            Validator = validator;
        }

        public PropertyValidator Validator { get; }
    }
}