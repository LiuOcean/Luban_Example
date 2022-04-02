using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Example
{
    public class UsageExample : MonoBehaviour
    {
        private void Awake() { _Init().Forget(); }

        private async UniTask _Init()
        {
            ViewAssetLoader loader = new ViewAssetLoader();

            new ConfigComponent(new ConfigComponentConfig(loader, "Example", typeof(ConfigComponent).Assembly));

            await ConfigComponent.Instance.Load();

            new LocalizeComponent(
                new LocalizeComponentConfig(
                    loader,
                    SystemLanguage.Chinese,
                    SystemLanguage.English,
                    null
                )
            );

            await LocalizeComponent.Instance.Load();

            // 单例表
            Debug.Log($"单例表: {GlobalConfigCategory.Single.default_icon}");

            // 多主键表
            var config = MultipleKeyConfigCategory.Instance.TryGet("First", 100, 200);
            Debug.Log($"多主键表: {config}");

            // ref 绑定
            var ref_config = ConfigComponent.Instance.Get<SingleRefConfig>(1);

            Debug.Log($"ref 绑定: {ref_config.ref_one_ref.id}");

            // 本地化使用
            var localize = LocalizeComponent.Instance.GetText("example/one");

            Debug.Log($"本地化: {localize}");
        }
    }
}