using YooAsset;

namespace Example
{
    public static class YooAssetConfigEx
    {
        public static YooAssetConfig Get()
        {
            return new YooAssetConfig(
                "",
                play_mode: YooAssets.EPlayMode.EditorSimulateMode,
                location_services: new AddressLocationServices()
            );
        }
    }
}