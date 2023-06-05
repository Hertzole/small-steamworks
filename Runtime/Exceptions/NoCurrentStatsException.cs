#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

namespace Hertzole.SmallSteamworks
{
	public sealed class NoCurrentStatsException : SteamException
	{
		public NoCurrentStatsException() : base($"Current stats have not been retrieved yet. You must call {nameof(SteamManager)}.{nameof(SteamManager.Stats)}.{nameof(ISteamStats.RequestCurrentStats)} first.") { }
	}
}