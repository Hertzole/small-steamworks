using System;
using System.Globalization;
using UnityEngine;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

namespace Hertzole.SmallSteamworks
{
	[Serializable]
	public struct SteamID : IEquatable<SteamID>, IComparable<SteamID>, IEquatable<ulong>, IComparable<ulong>
	{
		[SerializeField]
		private ulong value;

		public static SteamID Invalid { get; } = new SteamID(0);

		public SteamID(ulong value)
		{
			this.value = value;
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}

		public static bool operator ==(SteamID left, SteamID right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SteamID left, SteamID right)
		{
			return !left.Equals(right);
		}

		public static bool operator ==(ulong left, SteamID right)
		{
			return right.Equals(left);
		}

		public static bool operator !=(ulong left, SteamID right)
		{
			return !right.Equals(left);
		}

		public static bool operator ==(SteamID left, ulong right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SteamID left, ulong right)
		{
			return !left.Equals(right);
		}

		public static bool operator >(SteamID left, SteamID right)
		{
			return left.CompareTo(right) > 0;
		}

		public static bool operator <(SteamID left, SteamID right)
		{
			return left.CompareTo(right) < 0;
		}
		
		public static bool operator >=(SteamID left, SteamID right)
		{
			return left.CompareTo(right) >= 0;
		}
		
		public static bool operator <=(SteamID left, SteamID right)
		{
			return left.CompareTo(right) <= 0;
		}

		public override string ToString()
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		public int CompareTo(SteamID other)
		{
			return value.CompareTo(other.value);
		}

		public int CompareTo(ulong other)
		{
			return value.CompareTo(other);
		}
		
		public override bool Equals(object obj)
		{
			return obj is SteamID other && Equals(other);
		}

		public bool Equals(SteamID other)
		{
			return value == other.value;
		}

		public bool Equals(ulong other)
		{
			return value == other;
		}

#if !DISABLESTEAMWORKS
		public static implicit operator CSteamID(SteamID steamID)
		{
			return new CSteamID(steamID.value);
		}

		public static implicit operator SteamID(CSteamID steamID)
		{
			return new SteamID(steamID.m_SteamID);
		}

		public static bool operator ==(SteamID left, CSteamID right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SteamID left, CSteamID right)
		{
			return !left.Equals(right);
		}

		public static bool operator ==(CSteamID left, SteamID right)
		{
			return right.Equals(left);
		}

		public static bool operator !=(CSteamID left, SteamID right)
		{
			return !right.Equals(left);
		}
#endif
	}
}