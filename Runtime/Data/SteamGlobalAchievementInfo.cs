using System;

namespace Hertzole.SmallSteamworks
{
	public readonly struct SteamGlobalAchievementInfo : IEquatable<SteamGlobalAchievementInfo>
	{
		public string Name { get; }
		public float Percent { get; }
		public bool IsUnlocked { get; }

		internal SteamGlobalAchievementInfo(string name, float percent, bool isUnlocked)
		{
			Name = name;
			Percent = percent;
			IsUnlocked = isUnlocked;
		}

		public bool Equals(SteamGlobalAchievementInfo other)
		{
			return Name == other.Name && Percent.Equals(other.Percent) && IsUnlocked == other.IsUnlocked;
		}

		public override bool Equals(object obj)
		{
			return obj is SteamGlobalAchievementInfo other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Name != null ? Name.GetHashCode() : 0;
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