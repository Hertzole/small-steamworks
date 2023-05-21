namespace Hertzole.SmallSteamworks
{
	public sealed class NoCurrentStatsException : SteamException
	{
		public NoCurrentStatsException() : base($"Current stats have not been retrieved yet. You must call {nameof(SteamManager)}.{nameof(SteamManager.Stats)}.{nameof(ISteamStats.RequestCurrentStats)} first.") { }
	}
}