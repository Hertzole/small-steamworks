namespace Hertzole.SmallSteamworks
{
	public delegate void AttachLeaderboardUGCCallback(bool success, SteamLeaderboard leaderboard);

	public readonly struct AttachLeaderboardUGCResponse
	{
		public bool Success { get; }
		public SteamLeaderboard Leaderboard { get; }
		
		internal AttachLeaderboardUGCResponse(bool success, SteamLeaderboard leaderboard)
		{
			Success = success;
			Leaderboard = leaderboard;
		}
	}
}