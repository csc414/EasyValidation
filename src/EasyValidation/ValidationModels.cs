using System;
using System.Collections.Generic;

namespace EasyValidation
{
    public static class ValidationModels
    {
        private static readonly Dictionary<Type, Dictionary<object, ModelConfiguration>> _configurations = new Dictionary<Type, Dictionary<object, ModelConfiguration>>();

        public static ModelConfiguration Configuration<T>()
        {
            return Configuration(typeof(T), null);
        }

        public static ModelConfiguration Configuration<T>(Enum group)
        {
            return Configuration(typeof(T), group);
        }

        public static ModelConfiguration Configuration(Type modelType, Enum group)
        {
            if (!_configurations.TryGetValue(modelType, out var groups))
                _configurations.Add(modelType, groups = new Dictionary<object, ModelConfiguration>());

            object key = modelType;
            if (group != null)
                key = group;
            if (!groups.TryGetValue(key, out var configuration))
                groups.Add(key, configuration = new ModelConfiguration(modelType));

            return configuration;
        }
    }
}