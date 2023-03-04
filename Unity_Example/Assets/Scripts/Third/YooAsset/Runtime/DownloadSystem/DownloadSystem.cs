﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace YooAsset
{
	/// <summary>
	/// 1. 保证每一时刻资源文件只存在一个下载器
	/// 2. 保证下载器下载完成后立刻验证并缓存
	/// 3. 保证资源文件不会被重复下载
	/// </summary>
	internal static class DownloadSystem
	{
		private static readonly Dictionary<string, DownloaderBase> _downloaderDic = new Dictionary<string, DownloaderBase>();
		private static readonly List<string> _removeList = new List<string>(100);
		private static readonly Dictionary<string, string> _cachedHashList = new Dictionary<string, string>(1000);
		private static int _breakpointResumeFileSize = int.MaxValue;
		private static EVerifyLevel _verifyLevel = EVerifyLevel.High;


		/// <summary>
		/// 初始化
		/// </summary>
		public static void Initialize(int breakpointResumeFileSize, EVerifyLevel verifyLevel)
		{
			_breakpointResumeFileSize = breakpointResumeFileSize;
			_verifyLevel = verifyLevel;
		}

		/// <summary>
		/// 更新所有下载器
		/// </summary>
		public static void Update()
		{
			// 更新下载器
			_removeList.Clear();
			foreach (var valuePair in _downloaderDic)
			{
				var downloader = valuePair.Value;
				downloader.Update();
				if (downloader.IsDone())
					_removeList.Add(valuePair.Key);
			}

			// 移除下载器
			foreach (var key in _removeList)
			{
				_downloaderDic.Remove(key);
			}
		}

		/// <summary>
		/// 销毁所有下载器
		/// </summary>
		public static void DestroyAll()
		{
			foreach (var valuePair in _downloaderDic)
			{
				var downloader = valuePair.Value;
				downloader.Abort();
			}
			_downloaderDic.Clear();
			_removeList.Clear();
			_cachedHashList.Clear();
			_breakpointResumeFileSize = int.MaxValue;
		}


		/// <summary>
		/// 开始下载资源文件
		/// 注意：只有第一次请求的参数才是有效的
		/// </summary>
		public static DownloaderBase BeginDownload(BundleInfo bundleInfo, int failedTryAgain, int timeout = 60)
		{
			// 查询存在的下载器
			if (_downloaderDic.TryGetValue(bundleInfo.FileHash, out var downloader))
			{
				return downloader;
			}

			// 如果资源已经缓存
			if (ContainsVerifyFile(bundleInfo.FileHash))
			{
				var tempDownloader = new TempDownloader(bundleInfo);
				return tempDownloader;
			}

			// 创建新的下载器	
			{
				YooLogger.Log($"Beginning to download file : {bundleInfo.FileName} URL : {bundleInfo.RemoteMainURL}");
				FileUtility.CreateFileDirectory(bundleInfo.GetCacheLoadPath());
				DownloaderBase newDownloader;
				if (bundleInfo.FileSize >= _breakpointResumeFileSize)
					newDownloader = new HttpDownloader(bundleInfo);
				else
					newDownloader = new FileDownloader(bundleInfo);
				newDownloader.SendRequest(failedTryAgain, timeout);
				_downloaderDic.Add(bundleInfo.FileHash, newDownloader);
				return newDownloader;
			}
		}

		/// <summary>
		/// 获取下载器的总数
		/// </summary>
		public static int GetDownloaderTotalCount()
		{
			return _downloaderDic.Count;
		}

		/// <summary>
		/// 查询是否为验证文件
		/// 注意：被收录的文件完整性是绝对有效的
		/// </summary>
		public static bool ContainsVerifyFile(string fileHash)
		{
			if (_cachedHashList.ContainsKey(fileHash))
			{
				string fileName = _cachedHashList[fileHash];
				string filePath = SandboxHelper.MakeCacheFilePath(fileName);
				if (File.Exists(filePath))
				{
					return true;
				}
				else
				{
					_cachedHashList.Remove(fileHash);
					YooLogger.Error($"Cache file is missing : {fileName}");
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 缓存验证过的文件
		/// </summary>
		public static void CacheVerifyFile(string fileHash, string fileName)
		{
			if (_cachedHashList.ContainsKey(fileHash) == false)
			{
				YooLogger.Log($"Cache verify file : {fileName}");
				_cachedHashList.Add(fileHash, fileName);
			}
		}

		/// <summary>
		/// 验证文件完整性
		/// </summary>
		public static bool CheckContentIntegrity(string filePath, long fileSize, string fileCRC)
		{
			return CheckContentIntegrity(_verifyLevel, filePath, fileSize, fileCRC);
		}

		/// <summary>
		/// 验证文件完整性
		/// </summary>
		public static bool CheckContentIntegrity(EVerifyLevel verifyLevel, string filePath, long fileSize, string fileCRC)
		{
			try
			{
				if(verifyLevel == EVerifyLevel.None)
				{
					return true;
				}
				
				if (File.Exists(filePath) == false)
					return false;

				// 先验证文件大小
				long size = FileUtility.GetFileSize(filePath);
				if (size != fileSize)
					return false;

				// 再验证文件CRC
				if (verifyLevel == EVerifyLevel.High)
				{
					string crc = HashUtility.FileCRC32(filePath);
					return crc == fileCRC;
				}
				else
				{
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}