using System;

namespace Hertzole.SmallSteamworks
{
	public delegate void UserInformationRetrievedCallback(SteamUser user);

	public readonly struct UserInformationRetrievedResponse : IEquatable<UserInformationRetrievedResponse>
	{
		public SteamUser User { get; }

		internal UserInformationRetrievedResponse(SteamUser user)
		{
			User = user;
		}

		public bool Equals(UserInformationRetrievedResponse other)
		{
			return User.Equals(other.User);
		}

		public override bool Equals(object obj)
		{
			return obj is UserInformationRetrievedResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			return User.GetHashCode();
		}

		public static bool operator ==(UserInformationRetrievedResponse left, UserInformationRetrievedResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(UserInformationRetrievedResponse left, UserInformationRetrievedResponse right)
		{
			return !left.Equals(right);
		}
	}
}