
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Encrypt;

namespace Example
{
    [Serializable]
    public partial class SingleRefConfig : AConfig
    {
    	/// <summary>
    	/// 单一引用
    	/// </summary>
        [JsonProperty("ref_one")]
        private int _ref_one { get; set; }

        [JsonIgnore]
        public EncryptInt ref_one { get; private set; } = new();

        [JsonIgnore]
        public RefOneConfig ref_one_ref { get; private set; }

        public override void EndInit() 
        {
            ref_one = _ref_one;
            base.EndInit();
        }

        public override void BindRef() 
        {

            ref_one_ref = _GetRef<RefOneConfig>(ref_one);
        }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}

