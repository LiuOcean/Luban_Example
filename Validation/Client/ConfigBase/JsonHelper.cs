using Newtonsoft.Json;

namespace Example;

public class JsonHelper
{
    public static string ToJson(object message) { return JsonConvert.SerializeObject(message); }

    public static object FromJson(string json, Type type) { return JsonConvert.DeserializeObject(json, type); }

    public static T FromJson<T>(string json, JsonSerializerSettings settings = null)
    {
        return JsonConvert.DeserializeObject<T>(json, settings);
    }
}