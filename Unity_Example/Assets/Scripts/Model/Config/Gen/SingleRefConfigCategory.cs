
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
using System.Linq;

namespace Example
{
    [Config]
    public partial class SingleRefConfigCategory : ACategory<SingleRefConfig>
    {
        public static SingleRefConfigCategory Instance { get; private set; }

        public SingleRefConfigCategory()
        {
            Instance = this;
        }


        public static SingleRefConfig Single { get; private set; }

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
            Single = JsonConvert.DeserializeObject<SingleRefConfig>(json, settings);
            Single.EndInit();
        }

        public override void BindRef()
        {
            Single.BindRef();
        }

    }
}

