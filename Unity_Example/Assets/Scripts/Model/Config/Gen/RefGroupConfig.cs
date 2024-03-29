
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
using MemoryPack;


namespace Example
{
    [MemoryPackable]
    
    public  partial class RefGroupConfig
    {
        /// <summary>
        /// ID
        /// </summary>
        [MemoryPackOrder(0)]
        public int  id { get; private set; }

        /// <summary>
        /// 引用一个 ID
        /// </summary>
        [MemoryPackOrder(1)]
        public int  ref_id { get; private set; }

        /// <summary>
        /// 引用一组 ID
        /// </summary>
        [MemoryPackOrder(2)]
        public IReadOnlyList<int> ref_ids { get; private set;}

        /// <summary>
        /// 引用 4 个 ID，允许为 0
        /// </summary>
        [MemoryPackOrder(3)]
        public IReadOnlyList<int> ref_zero_ids { get; private set;}


        [MemoryPackConstructor]
        public RefGroupConfig(int id,int ref_id,IReadOnlyList<int> ref_ids,IReadOnlyList<int> ref_zero_ids) 
        {
        	this.id = id;
        	this.ref_id = ref_id;
        	this.ref_ids = ref_ids;
        	this.ref_zero_ids = ref_zero_ids;
        }
    }
}
