#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

namespace Hertzole.SmallSteamworks
{
	/// <summary>
	///     The method used to upload a score to a leaderboard.
	/// </summary>
	public enum LeaderboardUploadScoreMethod
	{
		/// <summary>
		///     Leaderboard will only be updated if the new score is better than the existing one.
		/// </summary>
		KeepBest = 0,
		/// <summary>
		///     Leaderboard score will always be replaced with the new score.
		/// </summary>
		ForceUpdate = 1
	}
}