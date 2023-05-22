using System;

namespace Hertzole.SmallSteamworks
{
	public delegate void AvatarRetrievedCallback(SteamImage image, SteamID userId, int width, int height);

	public readonly struct AvatarRetrievedResponse : IEquatable<AvatarRetrievedResponse>
	{
		public SteamImage Image { get; }

		public SteamID UserId { get; }
		public int Width { get; }
		public int Height { get; }

		internal AvatarRetrievedResponse(SteamImage image, SteamID userId, int width, int height)
		{
			Image = image;
			UserId = userId;
			Width = width;
			Height = height;
		}

		public bool Equals(AvatarRetrievedResponse other)
		{
			return Image.Equals(other.Image) && UserId.Equals(other.UserId) && Width == other.Width && Height == other.Height;
		}

		public override bool Equals(object obj)
		{
			return obj is AvatarRetrievedResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Image.GetHashCode();
				hashCode = (hashCode * 397) ^ UserId.GetHashCode();
				hashCode = (hashCode * 397) ^ Width;
				hashCode = (hashCode * 397) ^ Height;
				return hashCode;
			}
		}

		public static bool operator ==(AvatarRetrievedResponse left, AvatarRetrievedResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(AvatarRetrievedResponse left, AvatarRetrievedResponse right)
		{
			return !left.Equals(right);
		}
	}
}