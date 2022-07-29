using Newtonsoft.Json;

namespace Example
{
    public abstract class ACategory
    {
        public abstract Type      GetConfigType { get; }
        public abstract AConfig   GetOne();
        public abstract AConfig[] GetAll();
        public abstract AConfig   TryGet(int id);

        public abstract void BeginInit(IAssetLoader loader, JsonSerializerSettings settings);

        internal abstract void InternalEndInit();

        public abstract void EndInit();

        public abstract void TranslateText();

        internal abstract void BindRef();
    }

    public abstract class ACategory<T> : ACategory where T : AConfig
    {
        protected Dictionary<int, T> dict;

        public override Type GetConfigType => typeof(T);

        public override AConfig GetOne()
        {
            foreach(var value in dict.Values)
            {
                return value;
            }

            return null;
        }

        public override AConfig[] GetAll() { return dict.Values.ToArray(); }

        public override AConfig TryGet(int id)
        {
            dict.TryGetValue(id, out var config);

            return config;
        }

        public sealed override void BeginInit(IAssetLoader loader, JsonSerializerSettings settings)
        {
            try
            {
                dict = new Dictionary<int, T>();

                var json = loader.Load(GetHashCode(), typeof(T).Name);

                if(string.IsNullOrEmpty(json))
                {
                    return;
                }

                _CustomDeserialize(json, settings);
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                loader.Release(GetHashCode());
            }
        }

        internal override void InternalEndInit()
        {
            foreach(var config in dict.Values)
            {
                config.EndInit();
            }
        }

        public override void EndInit() { }

        public override void TranslateText() { }

        protected virtual void _CustomDeserialize(string json, JsonSerializerSettings settings)
        {
            dict = JsonConvert.DeserializeObject<Dictionary<int, T>>(json, settings);

            foreach(var pair in dict)
            {
                pair.Value.id = pair.Key;
            }

            dict.TrimExcess();
        }

        internal sealed override void BindRef()
        {
            foreach(var v in dict.Values)
            {
                v.BindRef();
            }
        }
    }
}