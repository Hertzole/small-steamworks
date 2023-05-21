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

		internal SteamAchievement(string apiName, string displayName, string description, bool isHidden, bool isUnlocked)
		{
			ApiName = apiName;
			DisplayName = displayName;
			Description = description;
			IsHidden = isHidden;
			IsUnlocked = isUnlocked;
		}

		public bool Equals(SteamAchievement other)
		{
			return ApiName == other.ApiName && IsHidden == other.IsHidden && IsUnlocked == other.IsUnlocked;
		}

		public override bool Equals(object obj)
		{
			return obj is SteamAchievement other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = ApiName != null ? ApiName.GetHashCode() : 0;
				hashCode = (hashCode * 397) ^ IsHidden.GetHashCode();
				hashCode = (hashCode * 397) ^ IsUnlocked.GetHashCode();
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