#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

namespace Hertzole.SmallSteamworks
{
	public sealed class NoGlobalAchievementStatsException : SteamException
	{
		public NoGlobalAchievementStatsException() : base($"No global achievement stats have been loaded. You must call {nameof(SteamManager)}.{nameof(SteamManager.Achievements)}.{nameof(ISteamAchievements.RequestGlobalAchievementStats)} first.") { }
	}
}