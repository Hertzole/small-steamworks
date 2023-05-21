namespace Hertzole.SmallSteamworks
{
	public sealed class InvalidSteamLeaderboardException : SteamException
	{
		public InvalidSteamLeaderboardException() : base("The provided leaderboard is invalid.") { }
	}
}