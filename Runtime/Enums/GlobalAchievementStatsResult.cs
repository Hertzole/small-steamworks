#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

namespace Hertzole.SmallSteamworks
{
	/// <summary>
	///     Results for calling <see cref="ISteamAchievements.RequestGlobalAchievementStats" />.
	/// </summary>
	public enum GlobalAchievementStatsResult
	{
		/// <summary>
		///     The call was successful.
		/// </summary>
		Success = 0,
		/// <summary>
		///     Stats haven't been loaded yet. Call <see cref="ISteamStats.RequestCurrentStats" />.
		/// </summary>
		InvalidState = 1,
		/// <summary>
		///     The remote failed.
		/// </summary>
		Fail = 2
	}
}