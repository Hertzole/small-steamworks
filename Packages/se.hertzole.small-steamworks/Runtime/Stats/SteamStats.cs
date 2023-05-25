#if !DISABLESTEAMWORKS
#nullable enable
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using Hertzole.SmallSteamworks.Helpers;
using Steamworks;

namespace Hertzole.SmallSteamworks
{
	internal sealed class SteamStats : ISteamStats
	{
		private bool hasGlobalStats = false;

		private readonly SteamLogger<SteamStats> logger;

		private readonly SteamCallback<UserStatsReceived_t> userStatsReceivedCallback;
		private readonly SteamCallback<UserStatsReceived_t> userStatsReceivedCallResult;
		private readonly SteamCallback<GlobalStatsReceived_t> globalStatsReceivedCallResult;
		private readonly SteamCallback<UserStatsUnloaded_t> userStatsUnloadedCallback;

		private readonly HashSet<CSteamID> cachedUserStats = new HashSet<CSteamID>();

		public bool HasCurrentStats { get; private set; }
		public bool IsLoadingCurrentStats { get; private set; }

		public SteamStats(ISteamSettings settings)
		{
			logger = new SteamLogger<SteamStats>();

			userStatsReceivedCallback = new SteamCallback<UserStatsReceived_t>(CallbackType.Callback);
			userStatsReceivedCallResult = new SteamCallback<UserStatsReceived_t>(CallbackType.CallResult);
			globalStatsReceivedCallResult = new SteamCallback<GlobalStatsReceived_t>(CallbackType.CallResult);
			userStatsUnloadedCallback = new SteamCallback<UserStatsUnloaded_t>(CallbackType.Callback, OnUserStatsUnloaded);

			if (settings.FetchCurrentStatsOnBoot)
			{
				RequestCurrentStats();
			}
		}

		public void RequestCurrentStats(UserStatsReceivedCallback? callback = null)
		{
			if (IsLoadingCurrentStats)
			{
				throw new InvalidOperationException($"Already loading current stats. Check {nameof(IsLoadingCurrentStats)} first before calling {nameof(RequestCurrentStats)}.");
			}

			IsLoadingCurrentStats = true;
			HasCurrentStats = false;

			bool success = SteamUserStats.RequestCurrentStats();
			if (!success)
			{
				logger.LogError("Failed to request current stats!");
				IsLoadingCurrentStats = false;
				return;
			}

			userStatsReceivedCallback.RegisterOnce(t =>
			{
				if (t.m_eResult != EResult.k_EResultOK)
				{
					logger.LogError($"Failed to request current stats! Result: {t.m_eResult}");
					IsLoadingCurrentStats = false;
					return;
				}

				logger.Log("Successfully requested current stats!");
				HasCurrentStats = true;
				IsLoadingCurrentStats = false;
				callback?.Invoke(t.m_eResult == EResult.k_EResultOK, t.m_steamIDUser);
			}, t => t.m_steamIDUser == Steamworks.SteamUser.GetSteamID());
		}

		public void RequestUserStats(in SteamID steamID, UserStatsReceivedCallback? callback = null)
		{
			SteamAPICall_t call = SteamUserStats.RequestUserStats(steamID);
			userStatsReceivedCallResult.RegisterOnce(call, (t, failure) =>
			{
				if (failure)
				{
					logger.LogError("Call result failed :: Failed to request user stats!");
					callback?.Invoke(false, SteamID.Invalid);
					return;
				}

				if (t.m_eResult != EResult.k_EResultOK)
				{
					logger.LogError($"Call result failed :: Failed to request user stats! Result: {t.m_eResult}");
				}
				else
				{
					cachedUserStats.Add(t.m_steamIDUser);
				}

				callback?.Invoke(t.m_eResult == EResult.k_EResultOK, t.m_steamIDUser);
			});
		}

		public void RequestGlobalStats(in int historyDays, GlobalStatsReceivedCallback? callback = null)
		{
			hasGlobalStats = false;
			ThrowIfCurrentStatsNotFetched();

			if (historyDays < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(historyDays), "History days cannot be less than 0.");
			}

			if (historyDays > 60)
			{
				throw new ArgumentOutOfRangeException(nameof(historyDays), "History days cannot be more than 60.");
			}

			SteamAPICall_t call = SteamUserStats.RequestGlobalStats(historyDays);
			globalStatsReceivedCallResult.RegisterOnce(call, (t, failure) =>
			{
				if (failure)
				{
					logger.LogError("Call result failed :: Failed to request global stats!");
					callback?.Invoke(false);
					return;
				}

				if (t.m_eResult != EResult.k_EResultOK)
				{
					logger.LogError($"Call result failed :: Failed to request global stats! Result: {t.m_eResult}");
				}
				else
				{
					hasGlobalStats = true;
				}

				callback?.Invoke(t.m_eResult == EResult.k_EResultOK);
			});
		}

		public bool StoreStats()
		{
			ThrowIfCurrentStatsNotFetched();

			return SteamUserStats.StoreStats();
		}

		public bool ResetAllStats(in bool achievementsToo = false)
		{
			ThrowIfCurrentStatsNotFetched();

			bool result = SteamUserStats.ResetAllStats(achievementsToo);

			if (result)
			{
				logger.Log("Successfully reset all stats!");
				RequestCurrentStats();
			}
			else
			{
				logger.LogError("Failed to reset all stats!");
			}

			return result;
		}

		public void SetStatInt(in string statName, in int value, in bool shouldStore = true)
		{
			ThrowIfCurrentStatsNotFetched();
			ThrowIfStatNameIsInvalid(statName);

			bool success = SteamUserStats.SetStat(statName, value);
			if (!success)
			{
				logger.LogError($"Failed to set stat {statName} to {value}!");
				return;
			}

			logger.Log($"Successfully set stat {statName} to {value}!");

			if (shouldStore)
			{
				SteamUserStats.StoreStats();
			}
		}

		public void SetStatFloat(in string statName, in float value, in bool shouldStore = true)
		{
			ThrowIfCurrentStatsNotFetched();
			ThrowIfStatNameIsInvalid(statName);

			bool success = SteamUserStats.SetStat(statName, value);
			if (!success)
			{
				logger.LogError($"Failed to set stat {statName} to {value}!");
				return;
			}

			logger.Log($"Successfully set stat {statName} to {value}!");

			if (shouldStore)
			{
				SteamUserStats.StoreStats();
			}
		}

		public int GetStatInt(in string statName)
		{
			ThrowIfCurrentStatsNotFetched();
			ThrowIfStatNameIsInvalid(statName);

			bool success = SteamUserStats.GetStat(statName, out int value);
			if (!success)
			{
				logger.LogError($"Failed to get stat {statName}!");
				return 0;
			}

			logger.Log($"Successfully got stat {statName}!");
			return value;
		}

		public float GetStatFloat(in string statName)
		{
			ThrowIfCurrentStatsNotFetched();
			ThrowIfStatNameIsInvalid(statName);

			bool success = SteamUserStats.GetStat(statName, out float value);
			if (!success)
			{
				logger.LogError($"Failed to get stat {statName}!");
				return 0;
			}

			logger.Log($"Successfully got stat {statName}!");
			return value;
		}

		public int GetUserStatInt(in SteamID steamID, in string statName)
		{
			ThrowIfNoStatsForUser(steamID);
			ThrowIfStatNameIsInvalid(statName);

			bool success = SteamUserStats.GetUserStat(steamID, statName, out int value);
			if (!success)
			{
				logger.LogError($"Failed to get stat {statName} for user {steamID}!");
				return 0;
			}

			logger.Log($"Successfully got stat {statName} for user {steamID}!");
			return value;
		}

		public float GetUserStatFloat(in SteamID steamID, in string statName)
		{
			ThrowIfNoStatsForUser(steamID);
			ThrowIfStatNameIsInvalid(statName);

			bool success = SteamUserStats.GetUserStat(steamID, statName, out float value);
			if (!success)
			{
				logger.LogError($"Failed to get stat {statName} for user {steamID}!");
				return 0;
			}

			logger.Log($"Successfully got stat {statName} for user {steamID}!");
			return value;
		}

		public long GetGlobalStatInt(in string statName)
		{
			ThrowIfNoGlobalStats();
			ThrowIfStatNameIsInvalid(statName);

			bool success = SteamUserStats.GetGlobalStat(statName, out long value);
			if (!success)
			{
				logger.LogError($"Failed to get global stat {statName}!");
				return 0;
			}

			logger.Log($"Successfully got global stat {statName}!");
			return value;
		}

		public double GetGlobalStatFloat(in string statName)
		{
			ThrowIfNoGlobalStats();
			ThrowIfStatNameIsInvalid(statName);

			bool success = SteamUserStats.GetGlobalStat(statName, out double value);
			if (!success)
			{
				logger.LogError($"Failed to get global stat {statName}!");
				return 0;
			}

			logger.Log($"Successfully got global stat {statName}!");
			return value;
		}

		public long[] GetGlobalStatHistoryInt(in string statName, in int maxDays = 60)
		{
			ThrowIfNoGlobalStats();
			ThrowIfStatNameIsInvalid(statName);

			long[]? buffer = ArrayPool<long>.Shared.Rent(maxDays);

			int count = SteamUserStats.GetGlobalStatHistory(statName, buffer, (uint) buffer.Length);
			if (count == 0)
			{
				ArrayPool<long>.Shared.Return(buffer);
				logger.LogError($"Failed to get global stat history for {statName}!");
				return Array.Empty<long>();
			}
			
			long[] result = new long[count];
			Array.Copy(buffer, result, count);
			ArrayPool<long>.Shared.Return(buffer);

			logger.Log($"Successfully got global stat history for {statName}!");
			return result;
		}

		public double[] GetGlobalStatHistoryFloat(in string statName, in int maxDays = 60)
		{
			ThrowIfNoGlobalStats();
			ThrowIfStatNameIsInvalid(statName);
			
			double[]? buffer = ArrayPool<double>.Shared.Rent(maxDays);

			int count = SteamUserStats.GetGlobalStatHistory(statName, buffer, (uint) buffer.Length);
			if (count == 0)
			{
				ArrayPool<double>.Shared.Return(buffer);
				logger.LogError($"Failed to get global stat history for {statName}!");
				return  Array.Empty<double>();
			}

			double[] result = new double[count];
			Array.Copy(buffer, result, count);
			ArrayPool<double>.Shared.Return(buffer);
			
			logger.Log($"Successfully got global stat history for {statName}!");
			return result;
		}

		public bool UpdateAvgRateStat(in string statName, in float countThisSession, in double sessionLength, bool shouldStore = true)
		{
			bool result = SteamUserStats.UpdateAvgRateStat(statName, countThisSession, sessionLength);
			if (!result)
			{
				logger.LogError($"Failed to update average rate stat {statName}!");
				return false;
			}
			
			if (shouldStore)
			{
				SteamUserStats.StoreStats();
			}
			
			logger.Log($"Successfully updated average rate stat {statName}!");
			return true;
		}

		public void Dispose()
		{
			userStatsReceivedCallback.Dispose();
			userStatsReceivedCallResult.Dispose();
			globalStatsReceivedCallResult.Dispose();
			userStatsUnloadedCallback.Dispose();
		}

		/// <summary>
		///     Called when Steam's internal cache of stats for the a specific user has been unloaded.
		/// </summary>
		private void OnUserStatsUnloaded(UserStatsUnloaded_t param)
		{
			cachedUserStats.Remove(param.m_steamIDUser);
		}

		private void ThrowIfCurrentStatsNotFetched()
		{
			if (!HasCurrentStats)
			{
				throw new NoCurrentStatsException();
			}
		}

		internal void ThrowIfNoStatsForUser(in CSteamID steamID)
		{
			if (!cachedUserStats.Contains(steamID))
			{
				throw new NoStatsForUserException(steamID);
			}
		}

		private static void ThrowIfStatNameIsInvalid(in string statName)
		{
			if (string.IsNullOrWhiteSpace(statName))
			{
				throw new ArgumentException("Stat name cannot be null or whitespace!", nameof(statName));
			}

			int byteCount = Encoding.UTF8.GetByteCount(statName);
			if (byteCount > Constants.k_cchStatNameMax)
			{
				throw new ArgumentException($"Stat name cannot be longer than {Constants.k_cchStatNameMax} bytes!", nameof(statName));
			}
		}

		private void ThrowIfNoGlobalStats()
		{
			if (!hasGlobalStats)
			{
				throw new NoGlobalStatsException();
			}
		}
	}
}
#endif