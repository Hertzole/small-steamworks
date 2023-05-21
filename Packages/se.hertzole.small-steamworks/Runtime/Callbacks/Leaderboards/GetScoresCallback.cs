namespace Hertzole.SmallSteamworks
{
	public delegate void GetScoresCallback(bool success, SteamLeaderboard leaderboard, SteamLeaderboardEntry[] entries);
	
	public readonly struct GetScoresResponse
	{
		public bool Success { get; }
		public SteamLeaderboard Leaderboard { get; }
		public SteamLeaderboardEntry[] Entries { get; }
		
		internal GetScoresResponse(bool success, SteamLeaderboard leaderboard, SteamLeaderboardEntry[] entries)
		{
			Success = success;
			Leaderboard = leaderboard;
			Entries = entries;
		}
	}
}