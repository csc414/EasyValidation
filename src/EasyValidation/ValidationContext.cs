using System;

namespace EasyValidation
{
    public class ValidationContext
    {
        public ValidationContext(object instance)
        {
            Check.IfNullThrow(instance, "Cannot pass null object to Validate.");

            Instance = instance;
        }

        public object Instance { get; }

        public Type InstanceType => Instance.GetType();
    }
}