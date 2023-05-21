namespace Hertzole.SmallSteamworks
{
	public sealed class NoStatsForUserException : SteamException
	{
		public NoStatsForUserException(SteamID steamID) : base($"There's no user stats for user {steamID}. You must call {nameof(SteamManager)}.{nameof(SteamManager.Stats)}.{nameof(ISteamStats.RequestUserStats)} for this user first.") { }
	}
}