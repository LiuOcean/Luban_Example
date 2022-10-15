using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Encrypt;

{{
    func encrypt_type_name(x)
        type = x.ctype.type_name
        case type
            when 'int'
                ret 'EncryptInt'
            when 'float'
                ret 'EncryptFloat'
            when 'double'
                ret 'EncryptDouble'
            when 'long'
                ret 'EncryptLong'
            else 
                ret '==='
        end
    end
}}
{{
    func is_encrypt_type(x)
        type = x.ctype.type_name
        case type
            when 'bean'
                ret false
            when 'list','array','set'
                case x.ctype.element_type
                    when 'int', 'float', 'double', 'long'
                        ret true
                    else
                        ret false
                end
            when 'int', 'float', 'double', 'long'
                ret true
            else
                ret false
        end
    end
}}
{{
    func is_encrypt_base_type(x)
        type = x.ctype.type_name
        case type
            when 'int', 'float', 'double', 'long'
                ret true
            else 
                ret false
        end
    end
}}
{{
    func is_encrypt_list_type(x)
        type = x.ctype.type_name
        case type 
            when 'list','array','set'
                case x.ctype.element_type
                    when 'int', 'float', 'double', 'long'
                        ret true
                    else
                        ret false
                end
            else
                ret false
        end
    end
}}
{{
    func is_list_bean(x)
        type = x.ctype.type_name
        case type
            when 'list','array','set'
                case x.ctype.element_type.type_name
                    when 'bean'
                        ret true
                    else
                        ret false
                end
            else
                ret false
        end
    end
}}
{{
    func property_name(x)
        ret '[JsonProperty("' + (x.name) + '")]'
    end
}}
{{
    name = x.name
    parent_def_type = x.parent_def_type
    parent = x.parent
    export_fields = x.export_fields
    hierarchy_export_fields = x.hierarchy_export_fields
}}
namespace Example
{
	{{~if x.comment != '' ~}}
	/// <summary>
	/// {{x.escape_comment}}
	/// </summary>
	{{~end~}}
    [Serializable]
    public partial class {{name}} : {{if parent_def_type}} {{x.parent}} {{else}} AConfig {{end}}
    {
    {{~ for field in export_fields ~}}
		{{~if field.name != "id"~}}
		    {{~if field.comment != '' ~}}
    	/// <summary>
    	/// {{field.escape_comment}}
    	/// </summary>
		    {{~end~}}
            {{~if field.gen_text_key~}}
        [JsonProperty("{{field.name}}")]
        private TranslateText _{{field.name}} { get; set; }
        [JsonIgnore]
        public string {{field.name}} => _{{field.name}}.text;
            {{~else if field.ctype.type_name == "bean"~}}
        [JsonProperty]
        public {{cs_define_type field.ctype}}  {{field.name}} { get; private set; }
            {{~else if is_encrypt_base_type field~}}
        {{property_name field}}
        private {{cs_define_type field.ctype}} _{{field.name}} { get; set; }

        [JsonIgnore]
        public {{encrypt_type_name field}} {{field.name}} { get; private set; } = new();
            {{~else if is_encrypt_list_type field~}}

        public {{cs_define_type field.ctype}}  {{field.name}} { get; private set; }

            {{~else if field.ctype.type_name == "list"~}}
        {{property_name field}}
        private {{cs_define_type field.ctype}} _{{field.name}} { get; set; }
      
        [JsonIgnore]
        private IReadOnlyList<{{cs_define_type field.ctype.element_type}}> __{{field.name}};

        [JsonIgnore]
        public IReadOnlyList<{{cs_define_type field.ctype.element_type}}> {{field.name}} => __{{field.name}} ??= _{{field.name}}.AsReadOnly();
            {{~else~}}
        [JsonProperty]
        public {{cs_define_type field.ctype}}  {{field.name}} { get; private set; }
            {{~end~}}
        {{~ if field.gen_ref ~}}
            {{~ if field.ref ~}}
        [JsonIgnore]
        public {{cs_define_type field.ref_type}} {{field.name}}_ref { get; private set; }
            {{~else}}
        [JsonIgnore]
        private List<{{cs_define_type field.element_ref_type.element_type}}> _{{field.name}}_ref { get; set; }
        [JsonIgnore]
        private IReadOnlyList<{{cs_define_type field.element_ref_type.element_type}}> __{{field.name}}_ref;
        [JsonIgnore]
        public IReadOnlyList<{{cs_define_type field.element_ref_type.element_type}}> {{field.name}}_ref => __{{field.name}}_ref ??= _{{field.name}}_ref.AsReadOnly();
            {{~end~}}
        {{~end~}}
		{{~end~}}
    {{~end~}}

        public override void EndInit() 
        {
        {{~ for field in export_fields ~}}
        {{~if field.name != "id"~}}
            {{~if is_encrypt_type field~}}
            {{field.name}} = _{{field.name}};
            {{~else if field.ctype.type_name == "bean"~}}
            {{field.name}}?.EndInit();
            {{~else if is_list_bean field~}}
            foreach(var _e in {{field.name}}) { _e?.EndInit(); }
            {{~end~}}
        {{~end~}}
        {{~end~}}
            base.EndInit();
        }

        public override void TranslateText()
        {
        {{~ for field in export_fields ~}}
        {{~if field.gen_text_key~}}
            _{{field.name}}?.Translate();
        {{~else if field.has_recursive_text~}}
            {{~if field.ctype.type_name == "bean"~}}
            {{field.name}}?.TranslateText();
            {{~else if is_list_bean field~}}
            foreach(var _e in {{field.name}}) { _e?.TranslateText(); }
            {{~end~}}
        {{~end~}}
        {{~end~}}
            base.TranslateText();
        }

        public override void BindRef() 
        {
            {{~ for field in export_fields ~}}
            {{~ if field.gen_ref ~}}
                {{~ if field.ref ~}}
            {{field.name}}_ref = _GetRef<{{cs_define_type field.ref_type}}>({{field.name}});
                {{~else}}
            _{{field.name}}_ref = _GetRefList<{{cs_define_type field.element_ref_type.element_type}}>({{field.name}});
                {{~end~}}
            {{~end~}}
            {{~end~}}
        }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
