#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

namespace Hertzole.SmallSteamworks
{
	public sealed class InvalidSteamLeaderboardException : SteamException
	{
		public InvalidSteamLeaderboardException() : base("The provided leaderboard is invalid.") { }
	}
}