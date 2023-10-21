using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace Example
{
    public class UsageExample : MonoBehaviour
    {
        private void Awake()
        {
            UniTaskScheduler.UnobservedTaskException += Debug.LogError;

            _Init().Forget();
        }

        private async UniTask _Init()
        {
            await YooAssetsEx.InitializeAsync(YooAssetConfigEx.Get());

            ViewAssetLoader loader = new ViewAssetLoader();

            await ConfigLoader.Load(loader);

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
            var config = MultipleKeyConfigCategory.Instance.Get("First", 100, 200);
            Debug.Log($"多主键表: {config}");

            // 本地化使用
            var localize = LocalizeComponent.Instance.GetText("example/one");

            Debug.Log($"本地化: {localize}");
        }
    }
}