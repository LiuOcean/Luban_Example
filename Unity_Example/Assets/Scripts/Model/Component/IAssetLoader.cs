using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Example
{
    public interface IAssetLoader
    {
        UniTask<T> Load<T>(string path) where T : Object;

        UniTask<string> LoadJson(string path);

        UniTask<byte[]> LoadBytes(string path);
    }
}