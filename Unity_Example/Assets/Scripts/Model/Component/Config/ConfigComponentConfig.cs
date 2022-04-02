using System.Reflection;
using Newtonsoft.Json;

namespace Example
{
    public class ConfigComponentConfig
    {
        public readonly IAssetLoader loader;
        public readonly string       config_namespace;
        public readonly Assembly     config_assembly;

        public ConfigComponentConfig(IAssetLoader loader, string config_namespace, Assembly config_assembly)
        {
            this.loader           = loader;
            this.config_namespace = config_namespace;
            this.config_assembly  = config_assembly;
        }

        public JsonSerializerSettings CreateSetting()
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling    = TypeNameHandling.Auto,
                SerializationBinder = new CustomBinder(config_assembly, config_namespace)
            };
        }
    }
}