#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;

namespace Hertzole.SmallSteamworks
{
	public delegate void AttachLeaderboardUGCCallback(bool success, SteamLeaderboard leaderboard);

	public readonly struct AttachLeaderboardUGCResponse : IEquatable<AttachLeaderboardUGCResponse>
	{
		public bool Success { get; }
		public SteamLeaderboard Leaderboard { get; }

		internal AttachLeaderboardUGCResponse(bool success, SteamLeaderboard leaderboard)
		{
			Success = success;
			Leaderboard = leaderboard;
		}

		public bool Equals(AttachLeaderboardUGCResponse other)
		{
			return Success == other.Success && Leaderboard.Equals(other.Leaderboard);
		}

		public override bool Equals(object obj)
		{
			return obj is AttachLeaderboardUGCResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Success.GetHashCode() * 397) ^ Leaderboard.GetHashCode();
			}
		}

		public static bool operator ==(AttachLeaderboardUGCResponse left, AttachLeaderboardUGCResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(AttachLeaderboardUGCResponse left, AttachLeaderboardUGCResponse right)
		{
			return !left.Equals(right);
		}
	}
}