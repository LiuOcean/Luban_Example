using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Example
{
    public class ViewAssetLoader : IAssetLoader
    {
        public UniTask<T> Load<T>(string path) where T : Object
        {
            return Addressables.LoadAssetAsync<T>(path).ToUniTask();
        }
    }
}