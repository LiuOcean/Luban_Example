using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Example
{
    // internal class LocalizeComponentAwakeSystem : AwakeSystem<LocalizeComponent, LocalizeComponentConfig>
    // {
    // public override void Awake(LocalizeComponent self, LocalizeComponentConfig config) { self.Awake(config); }
    // }

    public class LocalizeComponent // : Entity
    {
        public static LocalizeComponent Instance { get; private set; }


        /// <summary>
        /// 语言发生变化时的回调
        /// </summary>
        public static Action on_language_change;

        /// <summary>
        /// 实际上使用的语言
        /// </summary>
        public SystemLanguage actual_language { get; private set; }

        /// <summary>
        /// 当前组件的配置
        /// </summary>
        private LocalizeComponentConfig _config;

        /// <summary>
        /// 当前语言环境下的所有数据
        /// </summary>
        private ALocalizeCategory _current;

        /// <summary>
        /// 本地化对应的类型
        /// </summary>
        private readonly Dictionary<SystemLanguage, ALocalizeCategory> _localize_type = new();

        public LocalizeComponent(LocalizeComponentConfig config) { Awake(config); }

        internal void Awake(LocalizeComponentConfig config)
        {
            Instance = this;
            _config  = config;

            // var types = EventSystem.Instance.GetTypes(typeof(LocalizeConfigAttribute));

            var types = GetType().Assembly.GetTypes();

            foreach(var type in types)
            {
                var attr = type.GetCustomAttribute<LocalizeConfigAttribute>();

                if(attr is null)
                {
                    continue;
                }

                var handler = Activator.CreateInstance(type) as ALocalizeCategory;

                if(handler is null)
                {
                    continue;
                }

                _localize_type.Add(attr.language, handler);
            }
        }

        /// <summary>
        /// 内部加载并初始化逻辑
        /// </summary>
        public async UniTask Load()
        {
            actual_language = _config.current_language;

            _localize_type.TryGetValue(actual_language, out _current);

            if(_current is null)
            {
                _localize_type.TryGetValue(_config.default_language, out _current);
                actual_language = _config.default_language;
            }

            if(_current is null)
            {
                throw new ArgumentException($"[Localize] language is invalid, current = {_config.current_language}");
            }

            var text_asset = await _Load<TextAsset>(_current.Name());

            _current.Clear();

            // 先对当前语言进行序列化

            _current.Deserialize(text_asset);

            // TODO 增加初始内容的本地化
            // foreach(ALocalizeConfig data in _config.init_datas)
            // {
            //     _localize_datas[data.key] = data;
            //     _default_datas[data.key]  = data;
            // }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async UniTask<T> _Load<T>(string path) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            // Editor 环境下, 且没有在运行
            if(!UnityEditor.EditorApplication.isPlaying)
            {
                return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
            }
#endif
            return await _config.loader.Load<T>(path);
        }

        /// <summary>
        /// 切换系统语言, 提供动态修改
        /// 如果没有必要, 请在 Awake 时赋值
        /// </summary>
        /// <param name="language"></param>
        public async UniTask SwitchLanguage(SystemLanguage language)
        {
            // 实际上使用的语言和配置中的当前语言一致
            // 那么认为本次切换无效
            if(actual_language == _config.current_language)
            {
                return;
            }

            _config.current_language = language;

            await Load();

            on_language_change?.Invoke();
        }

        /// <summary>
        /// 获得某个本地化数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ALocalizeConfig GetData(string key) { return _current.GetData(key); }

        /// <summary>
        /// 获得本地化文本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetText(string key)
        {
            if(_current.TryGetValue(key, out var value))
            {
                return value;
            }

            return key;
        }

        /// <summary>
        /// 针对配表提供的翻译 API
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string GetText(TranslateText text)
        {
            if(_current.TryGetValue(text, out var value))
            {
                return value;
            }

            return text.text;
        }

        /// <summary>
        /// 获得 format 形式的本地化文本
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public string GetTextFormat(string key, params object[] values)
        {
            string format = GetText(key);

            if(string.Equals(format, key))
            {
                return key;
            }

            return string.Format(format, values);
        }
    }
}