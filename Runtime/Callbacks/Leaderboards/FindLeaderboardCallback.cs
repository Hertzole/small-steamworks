#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;

namespace Hertzole.SmallSteamworks
{
	public delegate void FindLeaderboardCallback(bool success, SteamLeaderboard leaderboard);

	public readonly struct FindLeaderboardResponse : IEquatable<FindLeaderboardResponse>
	{
		public readonly bool Success { get; }
		public readonly SteamLeaderboard Leaderboard { get; }

		public FindLeaderboardResponse(bool success, SteamLeaderboard leaderboard)
		{
			Success = success;
			Leaderboard = leaderboard;
		}

		public bool Equals(FindLeaderboardResponse other)
		{
			return Success == other.Success && Leaderboard.Equals(other.Leaderboard);
		}

		public override bool Equals(object obj)
		{
			return obj is FindLeaderboardResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Success.GetHashCode() * 397) ^ Leaderboard.GetHashCode();
			}
		}

		public static bool operator ==(FindLeaderboardResponse left, FindLeaderboardResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(FindLeaderboardResponse left, FindLeaderboardResponse right)
		{
			return !left.Equals(right);
		}
	}
}