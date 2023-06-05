#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

namespace Hertzole.SmallSteamworks
{
	/// <summary>
	///     The display type used by the Steam Community web site to know how to format the leaderboard scores when displayed.
	/// </summary>
	public enum LeaderboardDisplayType
	{
		/// <summary>
		///     The score is just a simple numerical value.
		/// </summary>
		Numeric = 0,
		/// <summary>
		///     The score represents a time in seconds.
		/// </summary>
		TimeSeconds = 1,
		/// <summary>
		///     The score represents a time in milliseconds.
		/// </summary>
		TimeMilliSeconds = 2
	}
}