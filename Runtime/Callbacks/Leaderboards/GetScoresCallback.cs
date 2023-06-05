#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;

namespace Hertzole.SmallSteamworks
{
	public delegate void GetScoresCallback(bool success, SteamLeaderboard leaderboard, SteamLeaderboardEntry[] entries);

	public readonly struct GetScoresResponse : IEquatable<GetScoresResponse>
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

		public bool Equals(GetScoresResponse other)
		{
			return Success == other.Success && Leaderboard.Equals(other.Leaderboard) && Equals(Entries, other.Entries);
		}

		public override bool Equals(object obj)
		{
			return obj is GetScoresResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Success.GetHashCode();
				hashCode = (hashCode * 397) ^ Leaderboard.GetHashCode();
				hashCode = (hashCode * 397) ^ (Entries != null ? Entries.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(GetScoresResponse left, GetScoresResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(GetScoresResponse left, GetScoresResponse right)
		{
			return !left.Equals(right);
		}
	}
}