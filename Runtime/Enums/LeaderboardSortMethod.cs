#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

namespace Hertzole.SmallSteamworks
{
	/// <summary>
	///     The sort method used to set whether a higher or lower score is better.
	/// </summary>
	public enum LeaderboardSortMethod
	{
		/// <summary>
		///     The lowest number is displayed first.
		/// </summary>
		Ascending = 0,
		/// <summary>
		///     The highest number is displayed first.
		/// </summary>
		Descending = 1
	}
}