#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

namespace Hertzole.SmallSteamworks
{
	public sealed class NoGlobalStatsException : SteamException
	{
		public NoGlobalStatsException() : base($"Global stats haven't been retrieved yet. You must call {nameof(SteamManager)}.{nameof(SteamManager.Stats)}.{nameof(ISteamStats.RequestGlobalStats)} first.") { }
	}
}