using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine = System.Numerics;
{{
    name = x.name
    parent_def_type = x.parent_def_type
    parent = x.parent
    export_fields = x.export_fields
    hierarchy_export_fields = x.hierarchy_export_fields
}}
namespace {{x.namespace_with_top_module}}
{
	{{~if x.comment != '' ~}}
	/// <summary>
	/// {{x.escape_comment}}
	/// </summary>
	{{~end~}}
    [Serializable]
    public partial class {{name}} : AConfig
    {
    {{~ for field in export_fields ~}}
		{{~if field.name != "id"~}}
		{{~if field.comment != '' ~}}
    	/// <summary>
    	/// {{field.escape_comment}}
    	/// </summary>
		{{~end~}}
		[JsonProperty]
        public {{cs_define_type field.ctype}} {{field.name}} { get; private set; }
		{{~end~}}

        {{~if field.gen_text_key~}}
        public {{cs_define_text_key_field field}} { get; }
        {{~end~}}
    {{~end~}}

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
