using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace Example
{
    public class ViewAssetLoader : IAssetLoader
    {
        public UniTask<T> Load<T>(string path) where T : Object { return YooAssetsEx.LoadAssetAsync<T>(path); }

        public async UniTask<string> LoadJson(string path)
        {
            var asset = await YooAssetsEx.LoadAssetAsync<TextAsset>(path);
            return asset.text;
        }

        public async UniTask<byte[]> LoadBytes(string path)
        {
            var asset = await YooAssetsEx.LoadAssetAsync<TextAsset>(path);

            return asset.bytes;
        }
    }
}