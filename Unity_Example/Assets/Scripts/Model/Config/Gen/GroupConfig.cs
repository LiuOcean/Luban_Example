
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
    
    public  partial class GroupConfig
    {
        /// <summary>
        /// ID
        /// </summary>
        [MemoryPackOrder(0)]
        public int  id { get; private set; }

        /// <summary>
        /// 客户端使用
        /// </summary>
        [MemoryPackOrder(1)]
        public string  client_name { get; private set; }


        [MemoryPackConstructor]
        public GroupConfig(int id,string client_name) 
        {
        	this.id = id;
        	this.client_name = client_name;
        }
    }
}
