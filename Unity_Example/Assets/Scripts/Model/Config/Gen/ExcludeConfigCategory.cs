
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
using UnityEngine;
using MemoryPack;

namespace Example
{
    [Config]
    [MemoryPackable]
    public partial class ExcludeConfigCategory : ACategory
    {
        public static ExcludeConfigCategory Instance { get; private set; }
        [MemoryPackConstructor]
        public ExcludeConfigCategory(IReadOnlyDictionary<int, ExcludeConfig> dic)
        {
            Instance = this;
            this.dic = dic;
        }

        [MemoryPackOrder(0)]
        public readonly IReadOnlyDictionary<int, ExcludeConfig> dic;

        public ExcludeConfig Get(int key)
        {
            dic.TryGetValue(key, out var result);
            return result;
        }
        
        public IReadOnlyDictionary<int, ExcludeConfig> GetAll() => dic;

        public override void GenEndInit() 
        {
        }
    }
}
