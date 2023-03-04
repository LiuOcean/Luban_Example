using System.IO;
using UnityEditor;
using UnityEngine.U2D;

namespace YooAsset.Editor
{
    public class AddressFull : IAddressRule
    {
        public string GetAssetAddress(AddressRuleData data) => data.AssetPath;
    }

    public class CollectSpriteAtalas : IFilterRule
    {
        public bool IsCollectAsset(FilterRuleData data) =>
            AssetDatabase.GetMainAssetTypeAtPath(data.AssetPath) == typeof(SpriteAtlas);
    }

    public class CollectJson : IFilterRule
    {
        public bool IsCollectAsset(FilterRuleData data) => Path.GetExtension(data.AssetPath) == ".json";
    }

    public class CollectAsset : IFilterRule
    {
        public bool IsCollectAsset(FilterRuleData data) => Path.GetExtension(data.AssetPath) == ".asset";
    }
}