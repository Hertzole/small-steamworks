#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;

namespace Hertzole.SmallSteamworks
{
	public delegate void UserStatsReceivedCallback(bool success, SteamID userID);

	public readonly struct UserStatsReceivedResponse : IEquatable<UserStatsReceivedResponse>
	{
		public bool Success { get; }
		public SteamID UserID { get; }

		internal UserStatsReceivedResponse(bool success, SteamID userID)
		{
			Success = success;
			UserID = userID;
		}

		public bool Equals(UserStatsReceivedResponse other)
		{
			return Success == other.Success && UserID.Equals(other.UserID);
		}

		public override bool Equals(object obj)
		{
			return obj is UserStatsReceivedResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Success.GetHashCode() * 397) ^ UserID.GetHashCode();
			}
		}

		public static bool operator ==(UserStatsReceivedResponse left, UserStatsReceivedResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(UserStatsReceivedResponse left, UserStatsReceivedResponse right)
		{
			return !left.Equals(right);
		}
	}
}