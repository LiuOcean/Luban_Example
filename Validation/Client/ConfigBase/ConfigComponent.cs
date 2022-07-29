using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace Example;

public class ConfigComponentConfig
{
    public readonly IAssetLoader loader;
    public readonly Assembly     assembly;

    public ConfigComponentConfig(Assembly assembly, IAssetLoader loader)
    {
        this.assembly = assembly;
        this.loader   = loader;
    }

    public JsonSerializerSettings CreateSetting()
    {
        return new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto, SerializationBinder = new CustomBinder(assembly, "Example")
        };
    }
}

public class ConfigComponent
{
    public static ConfigComponent Instance { get; private set; } = new();

    private IAssetLoader          _loader;
    private ConfigComponentConfig _config;

    internal JsonSerializerSettings settings { get; private set; }

    private readonly Dictionary<Type, ACategory> _all_configs = new();

    public void Awake(ConfigComponentConfig config)
    {
        _config  = config;
        _loader  = config.loader;
        settings = config.CreateSetting();

        Instance = this;
    }

    public void Load()
    {
        _all_configs.Clear();

        HashSet<Type> types = new();

        foreach(var type in _config.assembly.GetTypes())
        {
            types.Add(type);
        }

        List<ACategory> for_load = new List<ACategory>();

        foreach(Type type in types)
        {
            object[] attrs = type.GetCustomAttributes(typeof(ConfigAttribute), true);

            if(attrs.Length == 0)
            {
                continue;
            }

            if(type.IsAbstract || type.IsGenericType)
            {
                continue;
            }

            object obj = Activator.CreateInstance(type);

            if(obj is not ACategory icategory)
            {
                continue;
            }

            for_load.Add(icategory);
        }

        foreach(ACategory category in for_load)
        {
            category.BeginInit(_loader, settings);
        }

        foreach(ACategory category in for_load)
        {
            category.InternalEndInit();
            category.EndInit();
            _all_configs[category.GetConfigType] = category;
        }

        foreach(ACategory category in for_load)
        {
            category.BindRef();
        }
    }

    public AConfig GetOne(Type type)
    {
        _all_configs.TryGetValue(type, out var category);

        if(category is null)
        {
            return null;
        }

        return category.GetOne();
    }

    public T GetOne<T>() where T : AConfig { return(T) GetOne(typeof(T)); }

    public AConfig Get(Type type, int id)
    {
        _all_configs.TryGetValue(type, out var category);

        if(category is null)
        {
            return null;
        }

        return category.TryGet(id);
    }

    public T Get<T>(int id) where T : AConfig { return(T) Get(typeof(T), id); }

    public AConfig[] GetAll(Type type)
    {
        _all_configs.TryGetValue(type, out var category);

        if(category is null)
        {
            return null;
        }

        return category.GetAll();
    }

    public T[] GetAll<T>() where T : AConfig { return GetAll(typeof(T)) as T[]; }

    public ACategory GetCategory(Type type)
    {
        _all_configs.TryGetValue(type, out var category);
        return category;
    }

    public ACategory GetCategory<T>() where T : AConfig { return GetCategory(typeof(T)); }
}