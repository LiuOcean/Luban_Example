using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace Example
{
    public class ViewAssetLoader : IAssetLoader
    {
        public UniTask<T> Load<T>(string path) where T : Object { return YooAssetsEx.LoadAssetAsync<T>(path); }
    }
}