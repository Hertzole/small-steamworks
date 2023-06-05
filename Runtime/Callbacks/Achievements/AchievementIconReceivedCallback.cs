#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;

namespace Hertzole.SmallSteamworks
{
	public delegate void AchievementIconReceivedCallback(SteamImage image);

	public readonly struct AchievementIconReceivedResponse : IEquatable<AchievementIconReceivedResponse>
	{
		public SteamImage Image { get; }

		internal AchievementIconReceivedResponse(SteamImage image)
		{
			Image = image;
		}

		public bool Equals(AchievementIconReceivedResponse other)
		{
			return Image.Equals(other.Image);
		}

		public override bool Equals(object obj)
		{
			return obj is AchievementIconReceivedResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			return Image.GetHashCode();
		}

		public static bool operator ==(AchievementIconReceivedResponse left, AchievementIconReceivedResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(AchievementIconReceivedResponse left, AchievementIconReceivedResponse right)
		{
			return !left.Equals(right);
		}
	}
}