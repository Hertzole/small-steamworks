namespace Hertzole.SmallSteamworks
{
	public delegate void GlobalStatsReceivedCallback(bool success);

	public readonly struct GlobalStatsReceivedResponse
	{
		public bool Success { get; }

		internal GlobalStatsReceivedResponse(bool success)
		{
			Success = success;
		}
	}
}