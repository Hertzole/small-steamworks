#nullable enable

namespace Hertzole.SmallSteamworks
{
	public static partial class SteamExtensions
	{
		public static void UploadScore(this in SteamLeaderboard leaderboard, in LeaderboardUploadScoreMethod uploadScoreMethod, in int score, in int[]? scoreDetails = null, UploadScoreCallback? callback = null)
		{
			SteamManager.Leaderboards.UploadScore(leaderboard, uploadScoreMethod, score, scoreDetails, callback);
		}

		public static void AttachUGC(this in SteamLeaderboard leaderboard, in SteamUGCHandle ugcHandle, AttachLeaderboardUGCCallback? callback = null)
		{
			SteamManager.Leaderboards.AttachLeaderboardUGC(leaderboard, ugcHandle, callback);
		}

		public static void GetScores(this in SteamLeaderboard leaderboard, in int count, in int offset = 0, GetScoresCallback? callback = null)
		{
			SteamManager.Leaderboards.GetScores(leaderboard, count, offset, callback);
		}

		public static void GetScoresFromFriends(this in SteamLeaderboard leaderboard, GetScoresCallback? callback = null)
		{
			SteamManager.Leaderboards.GetScoresFromFriends(leaderboard, callback);
		}

		public static void GetScoresAroundUser(this in SteamLeaderboard leaderboard, in int rangeStart = 10, in int rangeEnd = 10, GetScoresCallback? callback = null)
		{
			SteamManager.Leaderboards.GetScoresAroundUser(leaderboard, rangeStart, rangeEnd, callback);
		}
	}
}