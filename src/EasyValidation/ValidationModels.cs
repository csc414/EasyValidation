using System;
using System.Collections.Generic;

namespace EasyValidation
{
    public static class ValidationModels
    {
        private static readonly Dictionary<Type, Dictionary<object, ModelConfiguration>> _configurations = new Dictionary<Type, Dictionary<object, ModelConfiguration>>();

        public static ModelConfiguration GetOrCreateConfiguration<T>()
        {
            return GetOrCreateConfiguration<T>(null);
        }

        public static ModelConfiguration GetOrCreateConfiguration<T>(Enum group)
        {
            return GetOrCreateConfiguration(typeof(T), group);
        }

        public static ModelConfiguration GetOrCreateConfiguration(Type modelType, Enum group)
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

        public static ModelConfiguration GetConfiguration(Type modelType, Enum group)
        {
            if (!_configurations.TryGetValue(modelType, out var groups))
                return null;

            object key = modelType;
            if (group != null)
                key = group;

            if (!groups.TryGetValue(key, out var configuration))
                return null;

            return configuration;
        }
    }
}