using System;
using Nireus;
{{ 
    name = x.name
}}
namespace {{x.namespace_with_top_module}}
{
	{{~if x.comment != '' ~}}
	/// <summary>
	/// {{x.escape_comment}}
	/// </summary>
	{{~end~}}
    public partial class {{name}} : ACategory<{{x.value_type}}>
    {
		public static {{name}} Instance { get { return Singleton<{{name}}>.Instance; } }
    }
}
