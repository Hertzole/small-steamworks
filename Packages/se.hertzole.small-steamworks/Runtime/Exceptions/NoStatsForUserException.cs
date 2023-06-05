#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

namespace Hertzole.SmallSteamworks
{
	public sealed class NoStatsForUserException : SteamException
	{
		public NoStatsForUserException(SteamID steamID) : base($"There's no user stats for user {steamID}. You must call {nameof(SteamManager)}.{nameof(SteamManager.Stats)}.{nameof(ISteamStats.RequestUserStats)} for this user first.") { }
	}
}