#if !DISABLESTEAMWORKS
#nullable enable
using System;
using System.Collections.Generic;
using Hertzole.SmallSteamworks.Helpers;
using Steamworks;

namespace Hertzole.SmallSteamworks
{
	internal sealed class SteamAchievements : ISteamAchievements
	{
		private readonly SteamLogger<SteamAchievements> logger;

		private readonly SteamCallback<UserAchievementStored_t> userAchievementStoredCallback;
		private readonly SteamCallback<UserAchievementIconFetched_t> userAchievementIconFetchedCallback;
		private SteamCallback<GlobalAchievementPercentagesReady_t>? globalAchievementPercentagesReadyCallback;

		/// <inheritdoc />
		public uint NumberOfAchievements
		{
			get { return SteamUserStats.GetNumAchievements(); }
		}

		/// <inheritdoc />
		public bool HasGlobalStats { get; private set; } = false;

		public SteamAchievements()
		{
			logger = new SteamLogger<SteamAchievements>();

			// Have a predicate to make sure the callback only fires for this app.
			userAchievementStoredCallback =
				new SteamCallback<UserAchievementStored_t>(CallbackType.Callback, OnUserAchievementStored, t => t.m_nGameID == SteamUtils.GetAppID().m_AppId);

			userAchievementIconFetchedCallback = new SteamCallback<UserAchievementIconFetched_t>(CallbackType.Callback, OnUserAchievementIconFetched,
				t => t.m_nGameID.m_GameID == SteamUtils.GetAppID().m_AppId);
		}

		private const string KEY_NAME = "name";
		private const string KEY_DESCRIPTION = "desc";
		private const string KEY_HIDDEN = "hidden";

		/// <inheritdoc />
		public event AchievementUnlockCallback? OnAchievementUnlocked;

		/// <inheritdoc />
		public void ResetAchievement(in string achievementName, in bool shouldStore = true)
		{
			ThrowIfCurrentStatsNotAvailable();

			bool result = SteamUserStats.ClearAchievement(achievementName);
			if (result)
			{
				logger.Log($"Achievement '{achievementName}' successfully reset.");
				if (shouldStore)
				{
					logger.Log("Uploading stats to Steam.");
					SteamUserStats.StoreStats();
				}
			}
			else
			{
				logger.LogError($"Failed to reset achievement '{achievementName}'.");
				throw new InvalidSteamAchievementException(achievementName);
			}
		}

		/// <inheritdoc />
		public void ResetAllAchievements(in bool shouldStore = true)
		{
			uint numAchievements = SteamUserStats.GetNumAchievements();
			for (uint i = 0; i < numAchievements; i++)
			{
				string name = SteamUserStats.GetAchievementName(i);
				SteamUserStats.ClearAchievement(name);
			}

			if (shouldStore)
			{
				SteamUserStats.StoreStats();
			}
		}

		/// <inheritdoc />
		public bool IsAchievementUnlocked(in string achievementName, out DateTime unlockTime)
		{
			ThrowIfCurrentStatsNotAvailable();

			bool result = SteamUserStats.GetAchievementAndUnlockTime(achievementName, out bool isUnlocked, out uint unlockTimeSeconds);
			if (result)
			{
				unlockTime = isUnlocked ? DateTime.UnixEpoch.AddSeconds(unlockTimeSeconds) : DateTime.MinValue;
				return isUnlocked;
			}

			logger.LogError($"Failed to get achievement '{achievementName}'.");
			throw new InvalidSteamAchievementException(achievementName);
		}

		/// <inheritdoc />
		public bool IsAchievementUnlockedForUser(in SteamID steamId, in string achievementName, out DateTime unlockTime)
		{
			ThrowIfCurrentStatsNotAvailable();
			((SteamStats) SteamManager.Stats).ThrowIfNoStatsForUser(steamId);

			bool result = SteamUserStats.GetUserAchievementAndUnlockTime(steamId, achievementName, out bool isUnlocked, out uint unlockTimeSeconds);
			if (result)
			{
				unlockTime = isUnlocked ? DateTime.UnixEpoch.AddSeconds(unlockTimeSeconds) : DateTime.MinValue;
				return isUnlocked;
			}

			logger.LogError($"Failed to get achievement '{achievementName}' for user '{steamId}'.");
			throw new InvalidSteamAchievementException(achievementName);
		}

		/// <inheritdoc />
		public void UnlockAchievement(in string achievementName, in bool shouldStore = true)
		{
			ThrowIfCurrentStatsNotAvailable();

			bool result = SteamUserStats.SetAchievement(achievementName);
			if (result)
			{
				logger.Log($"Achievement '{achievementName}' successfully unlocked.");
				if (shouldStore)
				{
					logger.Log("Uploading stats to Steam.");
					SteamUserStats.StoreStats();
				}
			}
			else
			{
				logger.LogError($"Failed to unlock achievement '{achievementName}'.");
				throw new InvalidSteamAchievementException(achievementName);
			}
		}

		/// <inheritdoc />
		public string GetAchievementName(in uint index)
		{
			ThrowIfCurrentStatsNotAvailable();

			return SteamUserStats.GetAchievementName(index);
		}

		/// <inheritdoc />
		public string GetAchievementDisplayName(in string achievementName)
		{
			return GetAchievementDisplayAttribute(achievementName, KEY_NAME);
		}

		/// <inheritdoc />
		public string GetAchievementDescription(in string achievementName)
		{
			return GetAchievementDisplayAttribute(achievementName, KEY_DESCRIPTION);
		}

		/// <inheritdoc />
		public bool IsAchievementHidden(in string achievementName)
		{
			string result = GetAchievementDisplayAttribute(achievementName, KEY_HIDDEN);
			return result == "1";
		}

		/// <inheritdoc />
		public SteamAchievement GetAchievementInfo(in string achievementName)
		{
			ThrowIfCurrentStatsNotAvailable();

			bool success = SteamUserStats.GetAchievementAndUnlockTime(achievementName, out bool isUnlocked, out uint unlockTimeSeconds);
			if (!success)
			{
				throw new InvalidSteamAchievementException(achievementName);
			}

			string? name = SteamUserStats.GetAchievementDisplayAttribute(achievementName, KEY_NAME);
			string? description = SteamUserStats.GetAchievementDisplayAttribute(achievementName, KEY_DESCRIPTION);
			string? hidden = SteamUserStats.GetAchievementDisplayAttribute(achievementName, KEY_HIDDEN);

			return new SteamAchievement(achievementName, name, description, hidden == "1", isUnlocked,
				isUnlocked ? DateTime.UnixEpoch.AddSeconds(unlockTimeSeconds) : DateTime.MinValue);
		}

		/// <inheritdoc />
		public void GetAchievementIcon(string achievementName, AchievementIconReceivedCallback? onIconFetched = null)
		{
			ThrowIfCurrentStatsNotAvailable();

			int handle = SteamUserStats.GetAchievementIcon(achievementName);
			// If the handle is 0, we need to wait for the icon to be loaded.
			// Otherwise, we can just call the callback immediately.
			if (handle == 0)
			{
				if (onIconFetched != null)
				{
					userAchievementIconFetchedCallback.RegisterOnce(t => { onIconFetched?.Invoke(new SteamImage(t.m_nIconHandle)); },
						t => t.m_rgchAchievementName == achievementName);
				}
			}
			else
			{
				onIconFetched?.Invoke(new SteamImage(handle));
			}
		}

		/// <inheritdoc />
		public float GetGlobalAchievementPercent(in string achievementName)
		{
			ThrowIfGlobalStatsNotAvailable();

			bool success = SteamUserStats.GetAchievementAchievedPercent(achievementName, out float achieved);
			if (!success)
			{
				throw new InvalidSteamAchievementException(achievementName);
			}

			return achieved;
		}

		/// <inheritdoc />
		public IEnumerable<SteamGlobalAchievementInfo> GetMostAchievedAchievements()
		{
			logger.Log($"Getting most achieved achievements | Has global stats: {HasGlobalStats}");
			
			ThrowIfGlobalStatsNotAvailable();

			int i = SteamUserStats.GetMostAchievedAchievementInfo(out string name, Constants.k_cchStatNameMax, out float percentage, out bool achieved);
			while (i != -1)
			{
				yield return new SteamGlobalAchievementInfo(name, percentage, achieved);
				i = SteamUserStats.GetNextMostAchievedAchievementInfo(i, out name, Constants.k_cchStatNameMax, out percentage, out achieved);
			}
		}

		/// <inheritdoc />
		public bool IndicateAchievementProgress(in string achievementName, in uint currentProgress, in uint maxProgress)
		{
			ThrowIfCurrentStatsNotAvailable();

#if DEBUG
			bool canBeFound = SteamUserStats.GetAchievement(achievementName, out _);
			if (!canBeFound)
			{
				throw new InvalidSteamAchievementException(achievementName);
			}
#endif

			bool result = SteamUserStats.IndicateAchievementProgress(achievementName, currentProgress, maxProgress);
			return result;
		}

		/// <inheritdoc />
		public void RequestGlobalAchievementStats(GlobalAchievementStatsReceivedCallback? onStatsFetched = null)
		{
			globalAchievementPercentagesReadyCallback ??= new SteamCallback<GlobalAchievementPercentagesReady_t>(CallbackType.CallResult);

			HasGlobalStats = false;
			SteamAPICall_t call = SteamUserStats.RequestGlobalAchievementPercentages();
			globalAchievementPercentagesReadyCallback.RegisterOnce(call, (t, failed) =>
			{
				if (failed)
				{
					logger.LogError("Failed to fetch global achievement stats.");
					onStatsFetched?.Invoke(GlobalAchievementStatsResult.Fail);
				}
				else
				{
					logger.Log($"Successfully fetched global achievement stats with result {t.m_eResult}.");
					switch (t.m_eResult)
					{
						case EResult.k_EResultOK:
							HasGlobalStats = true;
							onStatsFetched?.Invoke(GlobalAchievementStatsResult.Success);
							break;
						case EResult.k_EResultInvalidState:
							onStatsFetched?.Invoke(GlobalAchievementStatsResult.InvalidState);
							break;
						default:
							onStatsFetched?.Invoke(GlobalAchievementStatsResult.Fail);
							break;
					}
				}
			});
		}

		/// <summary>
		///     Called when user achievement stats have been stored.
		/// </summary>
		private void OnUserAchievementStored(UserAchievementStored_t param)
		{
			logger.Log(
				$"{nameof(param.m_nGameID)}: {param.m_nGameID}, {nameof(param.m_rgchAchievementName)}: {param.m_rgchAchievementName}, {nameof(param.m_nCurProgress)}: {param.m_nCurProgress}, {nameof(param.m_nMaxProgress)}: {param.m_nMaxProgress}");

			// If the progress is 0 and the max progress is 0, then the achievement is unlocked.
			if (param.m_nCurProgress == 0 && param.m_nMaxProgress == 0)
			{
				OnAchievementUnlocked?.Invoke(param.m_rgchAchievementName, DateTime.UtcNow);
			}
		}

		private void OnUserAchievementIconFetched(UserAchievementIconFetched_t param)
		{
			logger.Log(
				$"{nameof(param.m_nGameID)}: {param.m_nGameID}, {nameof(param.m_rgchAchievementName)}: {param.m_rgchAchievementName}, {nameof(param.m_bAchieved)}: {param.m_bAchieved}, {nameof(param.m_nIconHandle)}: {param.m_nIconHandle})");
		}

		public void Dispose()
		{
			userAchievementStoredCallback.Dispose();
			userAchievementIconFetchedCallback.Dispose();
			globalAchievementPercentagesReadyCallback?.Dispose();
		}

		private string GetAchievementDisplayAttribute(in string achievementName, in string key)
		{
			ThrowIfCurrentStatsNotAvailable();

			string? result = SteamUserStats.GetAchievementDisplayAttribute(achievementName, key);

			if (string.IsNullOrEmpty(result))
			{
				logger.LogError($"Failed to get achievement display attribute '{key}' for achievement '{achievementName}'.");
				throw new InvalidSteamAchievementException(achievementName);
			}

			return result;
		}

		private static void ThrowIfCurrentStatsNotAvailable()
		{
			if (!SteamManager.Stats.HasCurrentStats)
			{
				throw new NoCurrentStatsException();
			}
		}

		private void ThrowIfGlobalStatsNotAvailable()
		{
			if (!HasGlobalStats)
			{
				throw new NoGlobalAchievementStatsException();
			}
		}
	}
}
#endif