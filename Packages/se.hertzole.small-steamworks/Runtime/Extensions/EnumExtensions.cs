#if !DISABLESTEAMWORKS
using System;
using Steamworks;

namespace Hertzole.SmallSteamworks
{
	internal static class EnumExtensions
	{
		public static ELeaderboardSortMethod ToSteam(this LeaderboardSortMethod sortMethod)
		{
			switch (sortMethod)
			{
				case LeaderboardSortMethod.Ascending:
					return ELeaderboardSortMethod.k_ELeaderboardSortMethodAscending;
				case LeaderboardSortMethod.Descending:
					return ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending;
				default:
					throw new ArgumentOutOfRangeException(nameof(sortMethod), sortMethod, null);
			}
		}

		public static ELeaderboardUploadScoreMethod ToSteam(this LeaderboardUploadScoreMethod uploadScoreMethod)
		{
			switch (uploadScoreMethod)
			{
				case LeaderboardUploadScoreMethod.KeepBest:
					return ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest;
				case LeaderboardUploadScoreMethod.ForceUpdate:
					return ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate;
				default:
					throw new ArgumentOutOfRangeException(nameof(uploadScoreMethod), uploadScoreMethod, null);
			}
		}

		public static ELeaderboardDisplayType ToSteam(this LeaderboardDisplayType displayType)
		{
			switch (displayType)
			{
				case LeaderboardDisplayType.Numeric:
					return ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric;
				case LeaderboardDisplayType.TimeSeconds:
					return ELeaderboardDisplayType.k_ELeaderboardDisplayTypeTimeSeconds;
				case LeaderboardDisplayType.TimeMilliSeconds:
					return ELeaderboardDisplayType.k_ELeaderboardDisplayTypeTimeMilliSeconds;
				default:
					throw new ArgumentOutOfRangeException(nameof(displayType), displayType, null);
			}
		}

		public static LeaderboardSortMethod FromSteam(this ELeaderboardSortMethod sortMethod)
		{
			switch (sortMethod)
			{
				case ELeaderboardSortMethod.k_ELeaderboardSortMethodAscending:
					return LeaderboardSortMethod.Ascending;
				case ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending:
					return LeaderboardSortMethod.Descending;
				default:
					throw new ArgumentOutOfRangeException(nameof(sortMethod), sortMethod, "The sort method is invalid.");
			}
		}

		public static LeaderboardUploadScoreMethod FromSteam(this ELeaderboardUploadScoreMethod uploadScoreMethod)
		{
			switch (uploadScoreMethod)
			{
				case ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest:
					return LeaderboardUploadScoreMethod.KeepBest;
				case ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate:
					return LeaderboardUploadScoreMethod.ForceUpdate;
				default:
					throw new ArgumentOutOfRangeException(nameof(uploadScoreMethod), uploadScoreMethod, "The upload score method is invalid.");
			}
		}

		public static LeaderboardDisplayType FromSteam(this ELeaderboardDisplayType displayType)
		{
			switch (displayType)
			{
				case ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric:
					return LeaderboardDisplayType.Numeric;
				case ELeaderboardDisplayType.k_ELeaderboardDisplayTypeTimeSeconds:
					return LeaderboardDisplayType.TimeSeconds;
				case ELeaderboardDisplayType.k_ELeaderboardDisplayTypeTimeMilliSeconds:
					return LeaderboardDisplayType.TimeMilliSeconds;
				default:
					throw new ArgumentOutOfRangeException(nameof(displayType), displayType, "The display type is invalid.");
			}
		}
	}
}
#endif