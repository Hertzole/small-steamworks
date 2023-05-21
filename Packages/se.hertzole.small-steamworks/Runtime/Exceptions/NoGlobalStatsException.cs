namespace Hertzole.SmallSteamworks
{
	public sealed class NoGlobalStatsException : SteamException
	{
		public NoGlobalStatsException() : base($"Global stats haven't been retrieved yet. You must call {nameof(SteamManager)}.{nameof(SteamManager.Stats)}.{nameof(ISteamStats.RequestGlobalStats)} first.") { }
	}
}