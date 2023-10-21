
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MemoryPack;
using UnityEngine;

namespace Example
{
    public abstract class ACategory
    {
        public abstract void GenEndInit();

        public virtual void CustomEndInit() { }
    }

    public static class ConfigLoader
    {
        public static async UniTask Load(IAssetLoader loader)
        {
            try
            {
                // var       types     = Game.EventSystem.GetTypes(typeof(ConfigAttribute));
                // using var for_load  = ListComponent<ACategory>.Create();
                // using var tasks     = ListComponent<UniTask>.Create();

                var types    = typeof(ConfigLoader).Assembly.GetTypes();
                var for_load = new List<ACategory>();
                var tasks    = new List<UniTask>();
                
                foreach(Type type in types)
                {
                    var attrs = type.GetCustomAttributes(typeof(ConfigAttribute), true);

                    if(attrs.Length == 0)
                    {
                        continue;
                    }

                    tasks.Add(_Init(loader, type, for_load));

                    if(tasks.Count < 50)
                    {
                        continue;
                    }

                    await UniTask.WhenAll(tasks);
                    tasks.Clear();
                }

                if(tasks.Count > 0)
                {
                    await UniTask.WhenAll(tasks);
                    tasks.Clear();
                }

                foreach(ACategory category in for_load)
                {
                    category.GenEndInit();
                }

                foreach(var category in for_load)
                {
                    category.CustomEndInit();
                }
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
        }

        private static async UniTask<ACategory> _Init(IAssetLoader loader, Type type, List<ACategory> for_load)
        {
            var bytes = await loader.LoadBytes(type.Name);
            
            var category = MemoryPackSerializer.Deserialize(type, bytes) as ACategory;

            for_load.Add(category);

            return category;
        }
    }
}


