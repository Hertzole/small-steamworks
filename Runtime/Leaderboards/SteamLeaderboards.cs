#if !DISABLESTEAMWORKS
#nullable enable
using System;
using System.Globalization;
using System.Text;
using Hertzole.SmallSteamworks.Helpers;
using Steamworks;

namespace Hertzole.SmallSteamworks
{
	internal sealed class SteamLeaderboards : ISteamLeaderboards
	{
		private readonly SteamLogger<SteamLeaderboards> logger;

		private SteamCallback<LeaderboardFindResult_t>? leaderboardFindResultCallResult;
		private SteamCallback<LeaderboardScoreUploaded_t>? leaderboardScoreUploadedCallResult;
		private SteamCallback<LeaderboardScoresDownloaded_t>? leaderboardScoresDownloadedCallResult;
		private SteamCallback<LeaderboardUGCSet_t>? leaderboardUgcSetCallResult;

		public SteamLeaderboards()
		{
			logger = new SteamLogger<SteamLeaderboards>();
		}

		public void FindLeaderboard(in string leaderboardName, FindLeaderboardCallback? callback = null)
		{
			logger.Log($"{nameof(leaderboardName)}: {leaderboardName}, name byte count: {Encoding.UTF8.GetByteCount(leaderboardName).ToString(CultureInfo.InvariantCulture)}");

			ThrowIfLeaderboardNameIsInvalid(leaderboardName);

			leaderboardFindResultCallResult ??= new SteamCallback<LeaderboardFindResult_t>(CallbackType.CallResult);

			SteamAPICall_t call = SteamUserStats.FindLeaderboard(leaderboardName);
			leaderboardFindResultCallResult.RegisterOnce(call, (t, failure) =>
			{
				if (failure)
				{
					logger.LogError("Call result failed :: Failed to find leaderboard.");
					callback?.Invoke(false, SteamLeaderboard.Invalid);
					return;
				}

				logger.Log($"Leaderboard callback :: Found Leaderboard: {t.m_bLeaderboardFound == 1}, Leaderboard: {t.m_hSteamLeaderboard.m_SteamLeaderboard}");

				SteamLeaderboard leaderboard = new SteamLeaderboard(t.m_hSteamLeaderboard.m_SteamLeaderboard);

				callback?.Invoke(t.m_bLeaderboardFound == 1, leaderboard);
			});
		}

		public void FindOrCreateLeaderboard(in string leaderboardName, in LeaderboardSortMethod sortMethod, in LeaderboardDisplayType displayType, FindLeaderboardCallback? callback = null)
		{
			logger.Log($"{nameof(leaderboardName)}: {leaderboardName}, name byte count: {Encoding.UTF8.GetByteCount(leaderboardName).ToString(CultureInfo.InvariantCulture)}, {nameof(sortMethod)}: {sortMethod}, {nameof(displayType)}: {displayType}");

			ThrowIfLeaderboardNameIsInvalid(leaderboardName);

			leaderboardFindResultCallResult ??= new SteamCallback<LeaderboardFindResult_t>(CallbackType.CallResult);

			SteamAPICall_t call = SteamUserStats.FindOrCreateLeaderboard(leaderboardName, sortMethod.ToSteam(), displayType.ToSteam());
			leaderboardFindResultCallResult.RegisterOnce(call, (t, failure) =>
			{
				if (failure)
				{
					logger.LogError("Call result failed :: Failed to find or create leaderboard.");
					callback?.Invoke(false, SteamLeaderboard.Invalid);
					return;
				}

				logger.Log($"Leaderboard callback :: Found or created Leaderboard: {t.m_bLeaderboardFound == 1}, Leaderboard: {t.m_hSteamLeaderboard.m_SteamLeaderboard}");

				SteamLeaderboard leaderboard = new SteamLeaderboard(t.m_hSteamLeaderboard.m_SteamLeaderboard);

				callback?.Invoke(t.m_bLeaderboardFound == 1, leaderboard);
			});
		}

		public void SubmitScore(in SteamLeaderboard leaderboard, in LeaderboardUploadScoreMethod uploadScoreMethod, in int score, in int[]? scoreDetails = null, UploadScoreCallback? callback = null)
		{
			logger.Log($"{nameof(leaderboard)}: {leaderboard}, {nameof(uploadScoreMethod)}: {uploadScoreMethod}, {nameof(score)}: {score}, {nameof(scoreDetails)}: {scoreDetails?.Length ?? 0}");

			ThrowIfLeaderboardIsInvalid(leaderboard);

			if (scoreDetails != null && scoreDetails.Length > Constants.k_cLeaderboardDetailsMax)
			{
				throw new ArgumentException($"Score details can't be longer than {Constants.k_cLeaderboardDetailsMax} elements.", nameof(scoreDetails));
			}

			leaderboardScoreUploadedCallResult ??= new SteamCallback<LeaderboardScoreUploaded_t>(CallbackType.CallResult);

			SteamAPICall_t call = SteamUserStats.UploadLeaderboardScore(leaderboard, uploadScoreMethod.ToSteam(), score, scoreDetails, scoreDetails?.Length ?? 0);
			leaderboardScoreUploadedCallResult.RegisterOnce(call, (t, failure) =>
			{
				if (failure)
				{
					logger.LogError("Call result failed :: Failed to upload score.");
					callback?.Invoke(false, SteamLeaderboard.Invalid, 0, false, 0, 0);
					return;
				}

				logger.Log($"Leaderboard callback :: {nameof(t.m_bSuccess)}: {t.m_bSuccess}, {nameof(t.m_nScore)}: {t.m_nScore}, {nameof(t.m_bScoreChanged)}: {t.m_nScore}, {nameof(t.m_nGlobalRankNew)}: {t.m_nGlobalRankNew}, {nameof(t.m_nGlobalRankPrevious)}: {t.m_nGlobalRankPrevious}");

				callback?.Invoke(t.m_bSuccess == 1, new SteamLeaderboard(t.m_hSteamLeaderboard.m_SteamLeaderboard), t.m_nScore, t.m_bScoreChanged == 1, t.m_nGlobalRankNew, t.m_nGlobalRankPrevious);
			});
		}

		public void AttachLeaderboardUGC(in SteamLeaderboard leaderboard, in SteamUGCHandle ugcHandle, AttachLeaderboardUGCCallback? callback = null)
		{
			ThrowIfLeaderboardIsInvalid(leaderboard);
			ThrowIfUGCHandleIsInvalid(ugcHandle);

			leaderboardUgcSetCallResult ??= new SteamCallback<LeaderboardUGCSet_t>(CallbackType.CallResult);

			SteamAPICall_t call = SteamUserStats.AttachLeaderboardUGC(leaderboard, ugcHandle);
			leaderboardUgcSetCallResult.RegisterOnce(call, (t, failure) =>
			{
				if (failure)
				{
					logger.LogError("Call result failed :: Failed to attach leaderboard UGC.");
					callback?.Invoke(false, SteamLeaderboard.Invalid);
					return;
				}

				logger.Log($"Leaderboard callback :: {nameof(t.m_eResult)}: {t.m_eResult}, {nameof(t.m_hSteamLeaderboard)}: {t.m_hSteamLeaderboard.m_SteamLeaderboard}");
				callback?.Invoke(t.m_eResult == EResult.k_EResultOK, t.m_hSteamLeaderboard);
			});
		}

		public void GetScores(in SteamLeaderboard leaderboard, in int count, in int offset = 0, GetScoresCallback? callback = null)
		{
			ThrowIfLeaderboardIsInvalid(leaderboard);

			GetScoresInternal(leaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, offset + 1, (offset + 1) + count - 1, callback);
		}

		public void GetScoresFromFriends(in SteamLeaderboard leaderboard, GetScoresCallback? callback = null)
		{
			ThrowIfLeaderboardIsInvalid(leaderboard);

			GetScoresInternal(leaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends, 0, 0, callback);
		}

		public void GetScoresAroundUser(in SteamLeaderboard leaderboard, in int rangeStart, in int rangeEnd, GetScoresCallback? callback = null)
		{
			ThrowIfLeaderboardIsInvalid(leaderboard);

			GetScoresInternal(leaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, rangeStart, rangeEnd, callback);
		}

		private void GetScoresInternal(in SteamLeaderboard leaderboard, in ELeaderboardDataRequest dataRequest, in int rangeStart, in int rangeEnd, GetScoresCallback? callback = null)
		{
			logger.Log($"{nameof(leaderboard)}: {leaderboard}, {nameof(dataRequest)}: {dataRequest}, {nameof(rangeStart)}: {rangeStart}, {nameof(rangeEnd)}: {rangeEnd}");

			leaderboardScoresDownloadedCallResult ??= new SteamCallback<LeaderboardScoresDownloaded_t>(CallbackType.CallResult);

			SteamAPICall_t call = SteamUserStats.DownloadLeaderboardEntries(leaderboard, dataRequest, rangeStart, rangeEnd);
			leaderboardScoresDownloadedCallResult.RegisterOnce(call, (t, failure) =>
			{
				if (failure)
				{
					logger.LogError("Call result failed :: Failed to download leaderboard entries.");
					callback?.Invoke(false, SteamLeaderboard.Invalid, Array.Empty<SteamLeaderboardEntry>());
					return;
				}

				int count = t.m_cEntryCount;

				// If there are no tries, return a cached empty array to avoid allocations.
				if (count == 0)
				{
					callback?.Invoke(true, t.m_hSteamLeaderboard, Array.Empty<SteamLeaderboardEntry>());
					return;
				}

				SteamLeaderboardEntry[] entries = new SteamLeaderboardEntry[count];

				for (int i = 0; i < count; i++)
				{
					bool success = SteamUserStats.GetDownloadedLeaderboardEntry(t.m_hSteamLeaderboardEntries, i, out LeaderboardEntry_t entry, null, 0);
					if (success)
					{
						entries[i] = new SteamLeaderboardEntry(entry.m_steamIDUser, entry.m_nScore, entry.m_nGlobalRank, entry.m_hUGC);
					}
				}

				callback?.Invoke(true, t.m_hSteamLeaderboard, entries);
			});
		}

		public void Dispose()
		{
			leaderboardFindResultCallResult?.Dispose();
			leaderboardScoreUploadedCallResult?.Dispose();
			leaderboardScoresDownloadedCallResult?.Dispose();
			leaderboardUgcSetCallResult?.Dispose();
		}

		private static void ThrowIfLeaderboardNameIsInvalid(in string leaderboardName)
		{
			if (string.IsNullOrWhiteSpace(leaderboardName))
			{
				throw new ArgumentException("Leaderboard name can't be null or empty.", nameof(leaderboardName));
			}

			int length = Encoding.UTF8.GetByteCount(leaderboardName);
			if (length > Constants.k_cchLeaderboardNameMax)
			{
				throw new ArgumentException("Leaderboard name can't be longer than 128 bytes.", nameof(leaderboardName));
			}
		}

		private static void ThrowIfLeaderboardIsInvalid(in SteamLeaderboard leaderboard)
		{
			if (!leaderboard.IsValid)
			{
				throw new InvalidSteamLeaderboardException();
			}
		}

		private static void ThrowIfUGCHandleIsInvalid(in SteamUGCHandle ugcHandle) { }
	}
}
#endif