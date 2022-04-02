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
                    SystemLanguage.ChineseSimplified,
                    SystemLanguage.English,
                    null
                )
            );

            await LocalizeComponent.Instance.Load();

            // 单例表
            Debug.Log(GlobalConfigCategory.Single.default_icon);

            // 多主键表
            var config = MultipleKeyConfigCategory.Instance.TryGet("First", 100, 200);
            Debug.Log(config.ToString());

            // ref 绑定
            var ref_config = ConfigComponent.Instance.Get<SingleRefConfig>(1);

            Debug.Log(ref_config.ref_one_ref.id);
        }
    }
}