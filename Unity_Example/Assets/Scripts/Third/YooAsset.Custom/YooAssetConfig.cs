using System.Collections.Generic;
using YooAsset;

namespace YooAsset
{
    public class YooAssetConfig
    {
#if UNITY_EDITOR

        public const string EDITOR_PLAYMODE_KEY = "YooAsset_PlayMode";
#endif

        public readonly string cdn_url;

        public readonly int max_download;
        public readonly int retry;
        public readonly int time_out;


        public readonly YooAssets.EPlayMode play_mode;
        public readonly ILocationServices   location_services;
        public readonly IDecryptionServices decryption_services;
        public readonly bool                clear_cache_when_dirty;
        public readonly EVerifyLevel        verify_level;

        public YooAssetConfig(string              cdn_url,
                              int                 max_download           = 30,
                              int                 time_out               = 30,
                              int                 retry                  = 3,
                              YooAssets.EPlayMode play_mode              = YooAssets.EPlayMode.HostPlayMode,
                              ILocationServices   location_services      = null,
                              IDecryptionServices decryption_services    = null,
                              bool                clear_cache_when_dirty = false,
                              EVerifyLevel        verify_level           = EVerifyLevel.High)
        {
            this.cdn_url      = cdn_url;
            this.max_download = max_download;
            this.time_out     = time_out;
            this.retry        = retry;

#if UNITY_EDITOR
            play_mode = (YooAssets.EPlayMode) UnityEditor.EditorPrefs.GetInt(EDITOR_PLAYMODE_KEY, 0);
#endif
            this.play_mode              =   play_mode;
            this.location_services      =   location_services;
            this.decryption_services    =   decryption_services;
            this.clear_cache_when_dirty =   clear_cache_when_dirty;
            this.location_services      ??= new AddressLocationServices();
            this.verify_level           =   verify_level;
        }
    }
}