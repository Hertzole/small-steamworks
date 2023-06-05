#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;

namespace Hertzole.SmallSteamworks
{
	public delegate void GlobalAchievementStatsReceivedCallback(GlobalAchievementStatsResult result);

	public readonly struct GlobalAchievementStatsReceivedResponse : IEquatable<GlobalAchievementStatsReceivedResponse>
	{
		public GlobalAchievementStatsResult Result { get; }

		internal GlobalAchievementStatsReceivedResponse(GlobalAchievementStatsResult result)
		{
			Result = result;
		}

		public bool Equals(GlobalAchievementStatsReceivedResponse other)
		{
			return Result == other.Result;
		}

		public override bool Equals(object obj)
		{
			return obj is GlobalAchievementStatsReceivedResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			return (int) Result;
		}

		public static bool operator ==(GlobalAchievementStatsReceivedResponse left, GlobalAchievementStatsReceivedResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(GlobalAchievementStatsReceivedResponse left, GlobalAchievementStatsReceivedResponse right)
		{
			return !left.Equals(right);
		}
	}
}