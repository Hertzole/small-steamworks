#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;

namespace Hertzole.SmallSteamworks
{
	public readonly struct SteamGlobalAchievementInfo : IEquatable<SteamGlobalAchievementInfo>
	{
		public string ApiName { get; }
		public float Percent { get; }
		public bool IsUnlocked { get; }

		internal SteamGlobalAchievementInfo(string apiName, float percent, bool isUnlocked)
		{
			ApiName = apiName;
			Percent = percent;
			IsUnlocked = isUnlocked;
		}

		public bool Equals(SteamGlobalAchievementInfo other)
		{
			return ApiName == other.ApiName && Percent.Equals(other.Percent) && IsUnlocked == other.IsUnlocked;
		}

		public override bool Equals(object obj)
		{
			return obj is SteamGlobalAchievementInfo other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = ApiName != null ? ApiName.GetHashCode() : 0;
				hashCode = (hashCode * 397) ^ Percent.GetHashCode();
				hashCode = (hashCode * 397) ^ IsUnlocked.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(SteamGlobalAchievementInfo left, SteamGlobalAchievementInfo right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SteamGlobalAchievementInfo left, SteamGlobalAchievementInfo right)
		{
			return !left.Equals(right);
		}
	}
}