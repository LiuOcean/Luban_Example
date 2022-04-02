{{~
    name = x.name
    namespace_with_top_module = x.namespace_with_top_module
    comment = x.comment
    items = x.items
~}}

//using Sirenix.OdinInspector;

namespace {{namespace_with_top_module}}
{
{{~if comment != '' ~}}
    /// <summary>
    /// {{comment | html.escape}}
    /// </summary>
{{~end~}}
    {{~if x.is_flags~}}
    [System.Flags]
    {{~end~}}
    public enum {{name}}
    {
        {{~ for item in items ~}}
{{~if item.comment != '' ~}}
        // 导入 Odin 插件后, 修改 Tpls/common/cs/enum.tpl 即可
        //[LabelText("{{item.escape_comment}}")]
{{~end~}}
        {{item.name}} = {{item.value}},
        
        {{~end~}}
    }
}
