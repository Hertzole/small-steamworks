using System;

namespace Hertzole.SmallSteamworks
{
	public readonly struct SteamUser : IEquatable<SteamUser>
	{
		public SteamID SteamID { get; }
		public string Name { get; }
		public bool IsMe { get; }
		public bool IsOnline { get; }
		public int SteamLevel { get; }

		public static SteamUser Invalid { get { return new SteamUser(SteamID.Invalid, string.Empty, false, false, 0); } }

		internal SteamUser(SteamID steamID, string name, bool isMe, bool isOnline, int steamLevel)
		{
			SteamID = steamID;
			Name = name;
			IsMe = isMe;
			IsOnline = isOnline;
			SteamLevel = steamLevel;
		}

		public bool Equals(SteamUser other)
		{
			return SteamID.Equals(other.SteamID) && Name == other.Name && IsMe == other.IsMe && IsOnline == other.IsOnline && SteamLevel == other.SteamLevel;
		}

		public override bool Equals(object obj)
		{
			return obj is SteamUser other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = SteamID.GetHashCode();
				hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ IsMe.GetHashCode();
				hashCode = (hashCode * 397) ^ IsOnline.GetHashCode();
				hashCode = (hashCode * 397) ^ SteamLevel;
				return hashCode;
			}
		}

		public static bool operator ==(SteamUser left, SteamUser right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SteamUser left, SteamUser right)
		{
			return !left.Equals(right);
		}
	}
}