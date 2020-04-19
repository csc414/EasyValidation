using System;
using System.Collections.Generic;

namespace EasyValidation
{
    public static class ValidationModels
    {
        private static readonly Dictionary<Type, ModelConfiguration> _configurations = new Dictionary<Type, ModelConfiguration>();

        public static IEnumerable<ModelConfiguration> Configurations => _configurations.Values;

        public static ModelConfiguration Configuration<T>()
        {
            return Configuration(typeof(T));
        }

        public static ModelConfiguration Configuration(Type modelType)
        {
            if (!_configurations.TryGetValue(modelType, out var configuration))
                _configurations.Add(modelType, configuration = new ModelConfiguration(modelType));

            return configuration;
        }
    }
}