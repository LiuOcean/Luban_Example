using System.Collections.Generic;
using Newtonsoft.Json;

namespace Example
{
    public class AConfig
    {
        [JsonProperty]
        public int id { get; internal set; }

        public virtual void TranslateText() { }

        public virtual void EndInit() { }

        public virtual void BindRef() { }

        protected T _GetRef<T>(int id) where T : AConfig
        {
            var result = ConfigComponent.Instance.Get<T>(id);
            result?.BindRef();
            return result;
        }

        protected List<T> _GetRefList<T>(IReadOnlyList<int> ids) where T : AConfig
        {
            var result = new List<T>();

            foreach(var id in ids)
            {
                var item = ConfigComponent.Instance.Get<T>(id);

                item?.BindRef();

                result.Add(item);
            }

            return result;
        }
    }
}