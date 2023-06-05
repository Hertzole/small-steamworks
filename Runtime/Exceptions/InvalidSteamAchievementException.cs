#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

namespace Hertzole.SmallSteamworks
{
	public sealed class InvalidSteamAchievementException : SteamException
	{
		public InvalidSteamAchievementException(string achievementName) : base($"'{achievementName}' is not a valid achievement.") { }
	}
}