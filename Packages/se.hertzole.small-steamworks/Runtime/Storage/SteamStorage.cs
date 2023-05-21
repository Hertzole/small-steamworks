#if !DISABLESTEAMWORKS
#nullable enable
using System;
using Hertzole.SmallSteamworks.Helpers;
using Steamworks;

namespace Hertzole.SmallSteamworks
{
	internal sealed class SteamStorage : ISteamStorage
	{
		private readonly SteamLogger<SteamStorage> logger = new SteamLogger<SteamStorage>();

		private SteamCallback<RemoteStorageFileWriteAsyncComplete_t>? onFileWriteAsyncCompleteCallResult;
		private SteamCallback<RemoteStorageFileReadAsyncComplete_t>? onFileReadAsyncCompleteCallResult;
		private SteamCallback<RemoteStorageFileShareResult_t>? onFileShareResultCallResult;
		private SteamCallback<RemoteStorageDownloadUGCResult_t>? onDownloadUGCResultCallResult;

		public bool WriteFileSynchronous(string fileName, byte[] data)
		{
			logger.Log($"Writing file synchronously: {fileName} with size: {data.Length}");
			bool success = SteamRemoteStorage.FileWrite(fileName, data, data.Length);
			return success;
		}

		public void WriteFile(string fileName, byte[] data, FileWrittenCallback? callback = null)
		{
			onFileWriteAsyncCompleteCallResult ??= new SteamCallback<RemoteStorageFileWriteAsyncComplete_t>(CallbackType.CallResult);

			logger.Log($"Writing file: {fileName} with size: {data.Length}");

			SteamAPICall_t call = SteamRemoteStorage.FileWriteAsync(fileName, data, (uint) data.Length);
			onFileWriteAsyncCompleteCallResult.RegisterOnce(call, (t, failure) =>
			{
				if (failure)
				{
					logger.LogError("Call result failed when writing file.");
					callback?.Invoke(false);
					return;
				}

				logger.Log($"File written: {t.m_eResult}");
				callback?.Invoke(t.m_eResult == EResult.k_EResultOK);
			});
		}

		public FileReadResponse ReadFileSynchronous(string fileName)
		{
			logger.Log($"Reading file synchronously: {fileName}");

			int fileSize = SteamRemoteStorage.GetFileSize(fileName);
			if (fileSize == -1)
			{
				logger.LogError($"File not found: {fileName}");
				return new FileReadResponse(false, Array.Empty<byte>());
			}

			byte[] data = new byte[fileSize];
			int read = SteamRemoteStorage.FileRead(fileName, data, fileSize);
			if (read == 0)
			{
				logger.LogError($"Failed to read file: {fileName}");
				return new FileReadResponse(false, Array.Empty<byte>());
			}

			return new FileReadResponse(true, data);
		}

		public void ReadFile(string fileName, FileReadCallback? callback = null)
		{
			logger.Log($"Reading file: {fileName}");

			int fileSize = SteamRemoteStorage.GetFileSize(fileName);
			if (fileSize == -1)
			{
				logger.LogError($"File not found: {fileName}");
				callback?.Invoke(false, Array.Empty<byte>());
				return;
			}

			onFileReadAsyncCompleteCallResult ??= new SteamCallback<RemoteStorageFileReadAsyncComplete_t>(CallbackType.CallResult);

			SteamAPICall_t call = SteamRemoteStorage.FileReadAsync(fileName, 0, (uint) fileSize);
			onFileReadAsyncCompleteCallResult.RegisterOnce(call, (t, failure) =>
			{
				if (failure)
				{
					logger.LogError("Call result failed when reading file.");
					callback?.Invoke(false, Array.Empty<byte>());
					return;
				}

				logger.Log($"File read: {t.m_eResult}");
				byte[] data = new byte[t.m_cubRead];
				bool readSuccess = t.m_eResult == EResult.k_EResultOK && SteamRemoteStorage.FileReadAsyncComplete(t.m_hFileReadAsync, data, t.m_cubRead);
				callback?.Invoke(readSuccess, data);
			});
		}

		public bool FileExists(string fileName)
		{
			return SteamRemoteStorage.FileExists(fileName);
		}

		public bool FilePersisted(string fileName)
		{
			return SteamRemoteStorage.FilePersisted(fileName);
		}

		public bool ForgetFile(string fileName)
		{
			return SteamRemoteStorage.FileForget(fileName);
		}

		public bool DeleteFile(string fileName)
		{
			return SteamRemoteStorage.FileDelete(fileName);
		}

		public int GetFileSize(string fileName)
		{
			return SteamRemoteStorage.GetFileSize(fileName);
		}

		public QuotaResponse GetQuota()
		{
			SteamRemoteStorage.GetQuota(out ulong total, out ulong available);
			return new QuotaResponse(total, available);
		}

		public SteamWriteBatch WriteBatch()
		{
			return new SteamWriteBatch();
		}

		public SteamWriteStream WriteStream(string fileName)
		{
			UGCFileWriteStreamHandle_t handle = SteamRemoteStorage.FileWriteStreamOpen(fileName);
			return new SteamWriteStream(handle);
		}

		public void ShareFile(string fileName, FileSharedCallback? callback = null)
		{
			logger.Log($"Sharing file: {fileName}");

			onFileShareResultCallResult ??= new SteamCallback<RemoteStorageFileShareResult_t>(CallbackType.CallResult);

			SteamAPICall_t call = SteamRemoteStorage.FileShare(fileName);
			onFileShareResultCallResult.RegisterOnce(call, (t, failure) =>
			{
				if (failure)
				{
					logger.LogError("Call result failed when sharing file.");
					callback?.Invoke(false, t.m_hFile, string.Empty);

					return;
				}

				logger.Log($"File shared: {t.m_eResult}");
				callback?.Invoke(t.m_eResult == EResult.k_EResultOK, t.m_hFile, t.m_rgchFilename);
			});
		}

		public void DownloadSharedFile(SteamUGCHandle ugcHandle, uint priority = 0, UGCDownloadedCallback? callback = null)
		{
			logger.Log($"Downloading UGC: {ugcHandle}");

			onDownloadUGCResultCallResult ??= new SteamCallback<RemoteStorageDownloadUGCResult_t>(CallbackType.CallResult);

			SteamAPICall_t call = SteamRemoteStorage.UGCDownload(ugcHandle, priority);
			onDownloadUGCResultCallResult.RegisterOnce(call, (t, failure) =>
			{
				if (failure)
				{
					logger.LogError("Call result failed when downloading UGC.");
					callback?.Invoke(false, t.m_hFile, t.m_nAppID, 0, string.Empty, SteamID.Invalid);

					return;
				}

				logger.Log($"UGC downloaded: {t.m_eResult}");
				callback?.Invoke(t.m_eResult == EResult.k_EResultOK, ugcHandle, t.m_nAppID, t.m_nSizeInBytes, t.m_pchFileName, new SteamID(t.m_ulSteamIDOwner));
			});
		}

		public void DownloadSharedFileToLocation(SteamUGCHandle ugcHandle, string location, uint priority = 0, UGCDownloadedCallback? callback = null)
		{
			logger.Log($"Downloading UGC to location: {ugcHandle}, {location}");

			onDownloadUGCResultCallResult ??= new SteamCallback<RemoteStorageDownloadUGCResult_t>(CallbackType.CallResult);

			SteamAPICall_t call = SteamRemoteStorage.UGCDownloadToLocation(ugcHandle, location, priority);
			onDownloadUGCResultCallResult.RegisterOnce(call, (t, failure) =>
			{
				if (failure)
				{
					logger.LogError("Call result failed when downloading UGC to location.");
					callback?.Invoke(false, t.m_hFile, t.m_nAppID, 0, string.Empty, SteamID.Invalid);

					return;
				}

				logger.Log($"UGC downloaded to location: {t.m_eResult}");
				callback?.Invoke(t.m_eResult == EResult.k_EResultOK, ugcHandle, t.m_nAppID, t.m_nSizeInBytes, t.m_pchFileName, new SteamID(t.m_ulSteamIDOwner));
			});
		}

		public byte[] ReadUGCFile(SteamUGCHandle ugcHandle, out int fileSize)
		{
			logger.Log($"Reading UGC: {ugcHandle}");

			bool gotDetails = SteamRemoteStorage.GetUGCDetails(ugcHandle, out AppId_t appId, out string? name, out int sizeInBytes, out CSteamID owner);
			if (!gotDetails)
			{
				logger.LogError("Failed to get UGC details.");
				fileSize = 0;
				return Array.Empty<byte>();
			}

			byte[] data = new byte[sizeInBytes];
			int readData = SteamRemoteStorage.UGCRead(ugcHandle, data, sizeInBytes, 0, EUGCReadAction.k_EUGCRead_ContinueReadingUntilFinished);
			if (readData != sizeInBytes)
			{
				logger.LogError("Failed to read UGC data.");
				fileSize = 0;
				return Array.Empty<byte>();
			}

			fileSize = sizeInBytes;
			return data;
		}

		public void Dispose()
		{
			onFileWriteAsyncCompleteCallResult?.Dispose();
			onFileReadAsyncCompleteCallResult?.Dispose();
			onFileShareResultCallResult?.Dispose();
			onDownloadUGCResultCallResult?.Dispose();
		}
	}
}
#endif