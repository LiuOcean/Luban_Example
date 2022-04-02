using System;
using Nireus;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
{{ 
    name = x.name
}}
{{
    func index_func(x)
        result=''
        for index in x.index_list
            result=result + (cs_define_type index.index_field.ctype) + ' ' + (index.index_field.name) + ','
        end
        ret result | string.remove_last ','
    end
}}
{{
    func index_key_v(x)
        result=''
        for index in x.index_list
            result=result + 'v.' + (index.index_field.name) + ','
        end
        ret result | string.remove_last ','
    end
}}
{{
    func index_key(x)
        result=''
        for index in x.index_list
            result=result + (index.index_field.name) + ','
        end
        ret result | string.remove_last ','
    end
}}
namespace {{x.namespace_with_top_module}}
{
	{{~if x.comment != '' ~}}
	/// <summary>
	/// {{x.escape_comment}}
	/// </summary>
	{{~end~}}
    [Config]
    public partial class {{name}} : ACategory<{{x.value_type}}>
    {
        [ClearOnReload]
        public static {{name}} Instance { get; private set; }

        public {{name}}()
        {
            Instance = this;
        }

    {{~if x.is_singleton_table}}
        public static {{x.value_type}} Single { get; private set; }

        public override AConfig GetOne()
        {
            throw new NotImplementedException();
        }

        public override AConfig[] GetAll()
        {
            throw new NotImplementedException();
        }
        
        public override AConfig TryGet(int id)
        {
            throw new NotImplementedException();
        }

        protected override void _CustomDeserialize(string json, JsonSerializerSettings settings)
        {
            Single = JsonHelper.FromJson<{{x.value_type}}>(json, settings);
            Single.EndInit();
        }
        {{~else if x.is_list_table~}}

        private Dictionary<string, {{x.value_type}}> _group = new();

        public override AConfig GetOne()
        {
            foreach(var v in _group.Values)
            {
                return v;
            }
            return null;
        }

        public override AConfig[] GetAll()
        {
            return _group.Values.ToArray();
        }
        
        public override AConfig TryGet(int id)
        {
            throw new NotImplementedException();
        }

        private string _GetKey({{index_func x}})
        {
            return $"{{~for index in x.index_list~}}{ {{index.index_field.name}} }*&^{{~end~}}";
        }

        protected override void _CustomDeserialize(string json, JsonSerializerSettings settings)
        {
            var list = JsonHelper.FromJson<List<{{x.value_type}}>>(json, settings);
            
            foreach(var v in list)
            {
                v.EndInit();
            }

            foreach(var v in list)
            {
                _group.Add(_GetKey({{index_key_v x}}), v);
            }
        }

        public {{x.value_type}} TryGet({{index_func x}})
        {
            _group.TryGetValue(_GetKey({{index_key x}}), out var result);
            return result;
        }

        {{~else~}}
        public override void TranslateText()
        {
            foreach(var v in dict.Values)
            {
                v.TranslateText();
            }
        }
    {{~end~}}

    }
}
