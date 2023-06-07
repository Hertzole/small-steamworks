#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif
#nullable enable

#if !DISABLESTEAMWORKS
using System;
using System.Buffers;
using System.Collections.Generic;
using Hertzole.SmallSteamworks.Helpers;
using Steamworks;

namespace Hertzole.SmallSteamworks
{
	internal sealed partial class SteamApps : ISteamApps
	{
		private string[]? availableGameLanguages;

		private SteamCallback<FileDetailsResult_t>? fileDetailsCallResult;

		private readonly Callback<DlcInstalled_t> dlcInstalled;
		private readonly Callback<TimedTrialStatus_t> timedTrialStatus;

		private readonly Dictionary<AppID, string> appInstallDirectoryCache = new Dictionary<AppID, string>();
		private readonly Dictionary<AppID, DepotID[]> appDepotsCache = new Dictionary<AppID, DepotID[]>();

		private static readonly SteamLogger<SteamApps> logger = new SteamLogger<SteamApps>();

		public bool IsCurrentAppInCybercafe
		{
			get { return Steamworks.SteamApps.BIsCybercafe(); }
		}
		public bool IsCurrentAppInstalled
		{
			get { return Steamworks.SteamApps.BIsAppInstalled(SteamManager.Settings.AppID); }
		}
		public bool IsLowViolenceEnabled
		{
			get { return Steamworks.SteamApps.BIsLowViolence(); }
		}
		public bool IsSubscribedToCurrentApp
		{
			get { return Steamworks.SteamApps.BIsSubscribedApp(SteamManager.Settings.AppID); }
		}
		public bool IsSubscribedFromFamilySharing
		{
			get { return Steamworks.SteamApps.BIsSubscribedFromFamilySharing(); }
		}
		public bool IsSubscribedFromFreeWeekend
		{
			get { return Steamworks.SteamApps.BIsSubscribedFromFreeWeekend(); }
		}
		public bool IsVACBanned
		{
			get { return Steamworks.SteamApps.BIsVACBanned(); }
		}
		public int AppBuildId
		{
			get { return Steamworks.SteamApps.GetAppBuildId(); }
		}
		public SteamID AppOwner
		{
			get { return Steamworks.SteamApps.GetAppOwner(); }
		}
		public DateTime PurchaseTime
		{
			get { return DateTime.UnixEpoch.AddSeconds(Steamworks.SteamApps.GetEarliestPurchaseUnixTime(SteamManager.Settings.AppID)); }
		}
		public string? CurrentBetaName
		{
			get { return Steamworks.SteamApps.GetCurrentBetaName(out string? newName, 256) ? newName : null; }
		}
		public string InstallDirectory
		{
			get { return GetAppInstallDirectoryInternal(SteamManager.Settings.AppID); }
		}
		public IReadOnlyList<string> AvailableGameLanguages
		{
			get
			{
				if (availableGameLanguages != null)
				{
					return availableGameLanguages;
				}

				string? languages = Steamworks.SteamApps.GetAvailableGameLanguages();
				logger.Log(languages);

				if (string.IsNullOrEmpty(languages))
				{
					availableGameLanguages = Array.Empty<string>();
					return availableGameLanguages;
				}

				availableGameLanguages = languages.Split(',');
				return availableGameLanguages;
			}
		}

		public string GameLanguage
		{
			get { return Steamworks.SteamApps.GetCurrentGameLanguage(); }
		}
		public IReadOnlyList<DepotID> InstalledDepots
		{
			get { return GetInstalledDepotsInternal(SteamManager.Settings.AppID); }
		}
		public string? LaunchCommandLine
		{
			get
			{
				Steamworks.SteamApps.GetLaunchCommandLine(out string? line, 1024 * 32);
				return line;
			}
		}

		public SteamApps()
		{
			dlcInstalled = Callback<DlcInstalled_t>.Create(OnDLCInstalledInternal);
			timedTrialStatus = Callback<TimedTrialStatus_t>.Create(OnTimeTrialChangedInternal);
		}

		public event Action<AppID>? OnInstalledDLC;
		public event TimeTrialStatusCallback? OnTimeTrialChanged;

		private const uint INSTALL_DIRECTORY_START_BUFFER_SIZE = 64;
		private const int INSTALLED_DEPOTS_BUFFER_SIZE = 128;

		public bool IsSubscribedToApp(AppID appId)
		{
			return Steamworks.SteamApps.BIsSubscribedApp(appId);
		}

		public DateTime GetAppPurchaseTime(AppID appid)
		{
			return DateTime.UnixEpoch.AddSeconds(Steamworks.SteamApps.GetEarliestPurchaseUnixTime(appid));
		}

		public string GetAppInstallDirectory(AppID appId)
		{
			return GetAppInstallDirectoryInternal(appId);
		}

		public void MarkContentCorrupt(bool missingFilesOnly)
		{
			Steamworks.SteamApps.MarkContentCorrupt(missingFilesOnly);
		}

		public IEnumerable<SteamDLC> GetAllDLC()
		{
			int count = Steamworks.SteamApps.GetDLCCount();
			for (int i = 0; i < count; i++)
			{
				bool success = Steamworks.SteamApps.BGetDLCDataByIndex(i, out AppId_t appId, out bool available, out string? name, 1024);
				if (!success)
				{
					continue;
				}

				yield return new SteamDLC(appId, available, Steamworks.SteamApps.BIsDlcInstalled(appId), name);
			}
		}

		public bool IsDLCInstalled(AppID dlcId)
		{
			return Steamworks.SteamApps.BIsDlcInstalled(dlcId);
		}

		public void InstallDLC(AppID dlcId)
		{
			Steamworks.SteamApps.InstallDLC(dlcId);
		}

		public void UninstallDLC(AppID dlcId)
		{
			Steamworks.SteamApps.UninstallDLC(dlcId);
		}

		public bool TryGetDLCDownloadProgress(AppID dlcId, out ulong bytesDownloaded, out ulong bytesTotal)
		{
			return Steamworks.SteamApps.GetDlcDownloadProgress(dlcId, out bytesDownloaded, out bytesTotal);
		}

		public IReadOnlyList<DepotID> GetInstalledDepots(AppID appId)
		{
			return GetInstalledDepotsInternal(appId);
		}

		public string? GetLaunchQueryParam(string key)
		{
			string? result = Steamworks.SteamApps.GetLaunchQueryParam(key);
			return string.IsNullOrWhiteSpace(result) ? null : result;
		}

		public void GetFileDetails(string fileName, FileDetailsCallback? callback = null)
		{
			fileDetailsCallResult ??= new SteamCallback<FileDetailsResult_t>(CallbackType.CallResult);

			SteamAPICall_t call = Steamworks.SteamApps.GetFileDetails(fileName);
			fileDetailsCallResult.RegisterOnce(call, (t, failure) =>
			{
				if (failure)
				{
					logger.LogError("Failed to get file details.");
					callback?.Invoke(false, 0, Array.Empty<byte>());
					return;
				}

				callback?.Invoke(t.m_eResult == EResult.k_EResultOK, t.m_ulFileSize, t.m_FileSHA);
			});
		}

		public void Dispose()
		{
			appInstallDirectoryCache.Clear();
			appDepotsCache.Clear();

			fileDetailsCallResult?.Dispose();

			dlcInstalled.Dispose();
			timedTrialStatus.Dispose();
		}

		private void OnDLCInstalledInternal(DlcInstalled_t param)
		{
			OnInstalledDLC?.Invoke(param.m_nAppID);
		}

		private void OnTimeTrialChangedInternal(TimedTrialStatus_t param)
		{
			OnTimeTrialChanged?.Invoke(param.m_unAppID, param.m_bIsOffline, param.m_unSecondsAllowed, param.m_unSecondsPlayed,
				param.m_unSecondsAllowed - param.m_unSecondsPlayed);
		}

		private string GetAppInstallDirectoryInternal(AppID id)
		{
			logger.Log(id.ToString());

			// If we already have the directory cached, just return it.
			if (appInstallDirectoryCache.TryGetValue(id, out string? directory))
			{
				logger.Log($"{id} :: Found in cache: {directory}");
				return directory;
			}

			uint count = INSTALL_DIRECTORY_START_BUFFER_SIZE;
			directory = null;

			// Keep getting the directory until it's not null or empty. This is because the count is sometimes bigger than the buffer.
			while (string.IsNullOrEmpty(directory))
			{
				// If the count is bigger than the previous count, it means the directory is bigger than the buffer.
				uint previousCount = count;
				count = Steamworks.SteamApps.GetAppInstallDir(id, out directory, count);
				// Reset the directory because it will be set to something weird if the count is bigger than the buffer.
				if (count > previousCount)
				{
					logger.Log($"{id} :: Count ({count}) is bigger than previous count ({previousCount}).");
					directory = null;
				}
			}

			logger.Log($"{id} :: Found directory: {directory}");

			appInstallDirectoryCache.Add(id, directory);
			return directory;
		}

		private IReadOnlyList<DepotID> GetInstalledDepotsInternal(AppID appId)
		{
			if (appDepotsCache.TryGetValue(appId, out DepotID[]? depots))
			{
				return depots;
			}

			DepotId_t[]? buffer = ArrayPool<DepotId_t>.Shared.Rent(INSTALLED_DEPOTS_BUFFER_SIZE);
			uint count = Steamworks.SteamApps.GetInstalledDepots(appId, buffer, (uint) buffer.Length);
			if (count == 0)
			{
				ArrayPool<DepotId_t>.Shared.Return(buffer);
				appDepotsCache.Add(appId, Array.Empty<DepotID>());
				return Array.Empty<DepotID>();
			}

			depots = new DepotID[count];
			for (int i = 0; i < count; i++)
			{
				depots[i] = buffer[i];
			}

			ArrayPool<DepotId_t>.Shared.Return(buffer);
			appDepotsCache.Add(appId, depots);
			return depots;
		}
	}
}
#endif