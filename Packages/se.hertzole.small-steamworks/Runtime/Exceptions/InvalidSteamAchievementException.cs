namespace Hertzole.SmallSteamworks
{
	public sealed class InvalidSteamAchievementException : SteamException
	{
		public InvalidSteamAchievementException(string achievementName) : base($"'{achievementName}' is not a valid achievement.") { }
	}
}