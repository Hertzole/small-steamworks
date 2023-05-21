namespace Hertzole.SmallSteamworks
{
	public sealed class NoGlobalAchievementStatsException : SteamException
	{
		public NoGlobalAchievementStatsException() : base($"No global achievement stats have been loaded. You must call {nameof(SteamManager)}.{nameof(SteamManager.Achievements)}.{nameof(ISteamAchievements.RequestGlobalAchievementStats)} first.") { }
	}
}