using System;
#if !DISABLESTEAMWORKS
using Hertzole.SmallSteamworks.Helpers;
using Steamworks;
#endif

namespace Hertzole.SmallSteamworks
{
	public readonly struct SteamLeaderboardEntry : IEquatable<SteamLeaderboardEntry>
	{
		public SteamUser UserID { get; }
		public int Score { get; }
		public int GlobalRank { get; }
		public SteamUGCHandle? UGC { get; }

		public bool IsValid { get; }

#if !DISABLESTEAMWORKS
		internal SteamLeaderboardEntry(SteamID userID, int score, int globalRank, UGCHandle_t ugcHandle)
		{
			UserID = SteamUserHelpers.GetSteamUser(userID);
			Score = score;
			GlobalRank = globalRank;
			if (ugcHandle != UGCHandle_t.Invalid)
			{
				UGC = new SteamUGCHandle(ugcHandle.m_UGCHandle);
			}
			else
			{
				UGC = null;
			}

			IsValid = true;
		}
#endif
		public bool Equals(SteamLeaderboardEntry other)
		{
			return UserID.Equals(other.UserID) && Score == other.Score && GlobalRank == other.GlobalRank && Nullable.Equals(UGC, other.UGC) && IsValid == other.IsValid;
		}

		public override bool Equals(object obj)
		{
			return obj is SteamLeaderboardEntry other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = UserID.GetHashCode();
				hashCode = (hashCode * 397) ^ Score;
				hashCode = (hashCode * 397) ^ GlobalRank;
				hashCode = (hashCode * 397) ^ UGC.GetHashCode();
				hashCode = (hashCode * 397) ^ IsValid.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(SteamLeaderboardEntry left, SteamLeaderboardEntry right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SteamLeaderboardEntry left, SteamLeaderboardEntry right)
		{
			return !left.Equals(right);
		}
	}
}