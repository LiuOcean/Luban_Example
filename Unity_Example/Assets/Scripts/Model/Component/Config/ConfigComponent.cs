using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Example
{
    // internal class ConfigComponentAwakeSystem : AwakeSystem<ConfigComponent, ConfigComponentConfig>
    // {
    // public override void Awake(ConfigComponent self, ConfigComponentConfig config) { self.Awake(config); }
    // }

    public class ConfigComponent //: Entity
    {
        public static ConfigComponent Instance { get; private set; }

        private IAssetLoader _loader;

        internal JsonSerializerSettings settings { get; private set; }

        private readonly Dictionary<Type, ACategory> _all_configs = new();


        public ConfigComponent(ConfigComponentConfig config) { Awake(config); }

        internal void Awake(ConfigComponentConfig config)
        {
            _loader  = config.loader;
            settings = config.CreateSetting();

            Instance = this;
        }

        /// <summary>
        /// 异步加载所有用 Config 标记的配置表
        /// </summary>
        public async UniTask Load()
        {
            // using var tasks = ListComponent<UniTask>.Create();
            var tasks = new List<UniTask>();
            try
            {
                _all_configs.Clear();

                // var types = EventSystem.Instance.GetTypes(typeof(ConfigAttribute));

                var types = GetType().Assembly.GetTypes();

                List<ACategory> for_load = new List<ACategory>();
                foreach(Type type in types)
                {
                    object[] attrs = type.GetCustomAttributes(typeof(ConfigAttribute), true);

                    if(attrs.Length == 0)
                    {
                        continue;
                    }

                    object obj = Activator.CreateInstance(type);

                    if(!(obj is ACategory icategory))
                    {
                        // Log.Error($"[ConfigComponent] {type.Name} not inherit form ACategory");
                        Debug.LogError($"[ConfigComponent] {type.Name} not inherit form ACategory");
                        continue;
                    }

                    for_load.Add(icategory);
                }

                foreach(ACategory category in for_load)
                {
                    tasks.Add(category.BeginInit(_loader, settings));
                }

                // await UniTask.WhenAll(tasks.List);
                await UniTask.WhenAll(tasks);

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
            catch(Exception e)
            {
                // Log.Error(e);
                Debug.LogError(e);
            }
        }

        public AConfig GetOne(Type type)
        {
            _all_configs.TryGetValue(type, out var category);

            if(category is null)
            {
                // Log.Error($"[ConfigComponent] not found key: {type.FullName}");
                Debug.LogError($"[ConfigComponent] not found key: {type.FullName}");
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
                // Log.Error($"[ConfigComponent] not found key: {type.FullName}");
                Debug.LogError($"[ConfigComponent] not found key: {type.FullName}");
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
                // Log.Error($"[ConfigComponent] not found key: {type.FullName}");
                Debug.LogError($"[ConfigComponent] not found key: {type.FullName}");
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
}