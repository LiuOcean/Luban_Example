using System.Reflection;
using Newtonsoft.Json;

namespace Example
{
    public class ConfigComponentConfig
    {
        public readonly IAssetLoader loader;

        public ConfigComponentConfig(IAssetLoader loader) { this.loader = loader; }

        public JsonSerializerSettings CreateSetting()
        {
            return new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};
        }
    }
}