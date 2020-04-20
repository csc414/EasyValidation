using System;

namespace EasyValidation
{
    public class ValidationContext
    {
        public ValidationContext(object instance, Enum group)
        {
            Check.IfNullThrow(instance, "Cannot pass null object to Validate.");

            Group = group;
            Instance = instance;
        }

        public Enum Group { get; }

        public object Instance { get; }

        public Type InstanceType => Instance.GetType();
    }
}