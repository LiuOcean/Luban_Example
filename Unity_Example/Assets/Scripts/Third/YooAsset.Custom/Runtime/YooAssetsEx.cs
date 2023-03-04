using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace YooAsset
{
    public static class YooAssetsEx
    {
        private static readonly Dictionary<Object, OperationHandleBase> _OBJ_2_HANDLES = new();

        private static readonly Dictionary<GameObject, Object> _GO_2_OBJ = new();

        private static PatchDownloaderOperation _DOWNLOADER;

        private static YooAssetConfig _CONFIG;

        public static UniTask InitializeAsync(YooAssetConfig config)
        {
            _CONFIG = config;

            YooAssets.InitializeParameters parameters = config.play_mode switch
            {
                YooAssets.EPlayMode.EditorSimulateMode => new YooAssets.EditorSimulateModeParameters(),
                YooAssets.EPlayMode.OfflinePlayMode    => new YooAssets.OfflinePlayModeParameters(),
                YooAssets.EPlayMode.HostPlayMode => new YooAssets.HostPlayModeParameters
                {
                    DecryptionServices  = config.decryption_services,
                    ClearCacheWhenDirty = config.clear_cache_when_dirty,
                    DefaultHostServer   = config.cdn_url,
                    FallbackHostServer  = config.cdn_url,
                    VerifyLevel         = config.verify_level,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(config.play_mode), config.play_mode, null)
            };

            parameters.LocationServices = config.location_services;

            return YooAssets.InitializeAsync(parameters).ToUniTask();
        }

        public static async UniTask<int> UpdateStaticVersion(int time_out = 30)
        {
            var operation = YooAssets.UpdateStaticVersionAsync(time_out);

            await operation.ToUniTask();

            if(operation.Status != EOperationStatus.Succeed)
            {
                return-1;
            }

            return operation.ResourceVersion;
        }

        public static async UniTask<bool> UpdateManifest(int resource_version, int time_out = 30)
        {
            var operation = YooAssets.UpdateManifestAsync(resource_version, time_out);

            await operation.ToUniTask();

            return operation.Status == EOperationStatus.Succeed;
        }

        public static long GetDownloadSize(int downloading_max_num, int retry)
        {
            _DOWNLOADER = YooAssets.CreatePatchDownloader(downloading_max_num, retry);

            return _DOWNLOADER.TotalDownloadCount == 0 ? 0 : _DOWNLOADER.TotalDownloadBytes;
        }

        public static long GetDownloadSize(string[] tags, int downloading_max_num, int retry)
        {
            _DOWNLOADER = YooAssets.CreatePatchDownloader(tags, downloading_max_num, retry);

            return _DOWNLOADER.TotalDownloadCount == 0 ? 0 : _DOWNLOADER.TotalDownloadBytes;
        }

        public static long GetLocationsDownLoadSize(string[] locations, int downloading_max_num, int retry)
        {
            _DOWNLOADER = YooAssets.CreateBundleDownloader(locations, downloading_max_num, retry);

            return _DOWNLOADER.TotalDownloadCount == 0 ? 0 : _DOWNLOADER.TotalDownloadBytes;
        }

        public static async UniTask<bool> Download(IProgress<float> progress = null)
        {
            if(_DOWNLOADER is null)
            {
                return false;
            }

            _DOWNLOADER.BeginDownload();

            await _DOWNLOADER.ToUniTask(progress);

            return _DOWNLOADER.Status == EOperationStatus.Succeed;
        }

        /// <summary>
        /// 获取原生文件
        /// </summary>
        /// <param name="location"></param>
        /// <param name="copy_path"></param>
        /// <param name="progress"></param>
        /// <returns>是否成功, 原生文件存放路径</returns>
        public static async UniTask<(bool, string)> GetRawFile(string           location,
                                                               string           copy_path = null,
                                                               IProgress<float> progress  = null)
        {
            RawFileOperation handle = YooAssets.GetRawFileAsync(location, copy_path);

            await handle.ToUniTask(progress);

            bool success = handle.Status == EOperationStatus.Succeed;

            return(success, success ? handle.GetCachePath() : string.Empty);
        }

        public static async UniTask<GameObject> InstantiateAsync(string           location,
                                                                 Transform        parent_transform = null,
                                                                 bool             stay_world_space = false,
                                                                 IProgress<float> progress         = null)
        {
            var handle = YooAssets.LoadAssetAsync<GameObject>(location);

            await handle.ToUniTask(progress);

            if(!handle.IsValid)
            {
                throw new Exception($"[YooAssetsEx] Failed to load asset: {location}");
            }

            _OBJ_2_HANDLES.TryAdd(handle.AssetObject, handle);

            if(Object.Instantiate(handle.AssetObject, parent_transform, stay_world_space) is not GameObject go)
            {
                Release(handle.AssetObject);
                throw new Exception($"[YooAssetsEx] Failed to instantiate asset: {location}");
            }

            _GO_2_OBJ.Add(go, handle.AssetObject);

            return go;
        }

        public static async UniTask<T> LoadAssetAsync<T>(string location, IProgress<float> progress = null)
            where T : Object
        {
            var handle = YooAssets.LoadAssetAsync<T>(location);

            await handle.ToUniTask(progress);

            if(!handle.IsValid)
            {
                throw new Exception($"[YooAssetsEx] Failed to load asset: {location}");
            }

            _OBJ_2_HANDLES.TryAdd(handle.AssetObject, handle);

            return handle.AssetObject as T;
        }

        public static void ReleaseInstance(GameObject go)
        {
            if(go is null)
            {
                return;
            }

            Object.Destroy(go);

            _GO_2_OBJ.Remove(go, out Object obj);

            Release(obj);
        }

        public static void Release(Object obj)
        {
            if(obj is null)
            {
                return;
            }

            _OBJ_2_HANDLES.Remove(obj, out OperationHandleBase handle);

            handle?.ReleaseInternal();
        }
    }
}