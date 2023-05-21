using System;

namespace Hertzole.SmallSteamworks
{
	public readonly struct SteamAchievement : IEquatable<SteamAchievement>
	{
		public string ApiName { get; }
		public string DisplayName { get; }
		public string Description { get; }
		public bool IsHidden { get; }
		public bool IsUnlocked { get; }
		public DateTime UnlockTime { get; }

		internal SteamAchievement(string apiName, string displayName, string description, bool isHidden, bool isUnlocked, DateTime unlockTime)
		{
			ApiName = apiName;
			DisplayName = displayName;
			Description = description;
			IsHidden = isHidden;
			IsUnlocked = isUnlocked;
			UnlockTime = unlockTime;
		}

		public bool Equals(SteamAchievement other)
		{
			return ApiName == other.ApiName && DisplayName == other.DisplayName && Description == other.Description && IsHidden == other.IsHidden && IsUnlocked == other.IsUnlocked && UnlockTime.Equals(other.UnlockTime);
		}

		public override bool Equals(object obj)
		{
			return obj is SteamAchievement other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (ApiName != null ? ApiName.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (DisplayName != null ? DisplayName.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ IsHidden.GetHashCode();
				hashCode = (hashCode * 397) ^ IsUnlocked.GetHashCode();
				hashCode = (hashCode * 397) ^ UnlockTime.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(SteamAchievement left, SteamAchievement right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SteamAchievement left, SteamAchievement right)
		{
			return !left.Equals(right);
		}
	}
}