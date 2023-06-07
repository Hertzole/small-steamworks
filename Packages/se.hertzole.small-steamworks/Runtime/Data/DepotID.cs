#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;
using System.Globalization;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

namespace Hertzole.SmallSteamworks
{
	/// <summary>
	///     Represents a Steam Depot ID.
	/// </summary>
	public readonly struct DepotID : IEquatable<DepotID>
	{
		private readonly uint value;

		/// <summary>
		///     Returns an invalid DepotID.
		/// </summary>
		public static DepotID Invalid
		{
			get { return new DepotID(0x0); }
		}

		internal DepotID(uint value)
		{
			this.value = value;
		}

		public override string ToString()
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		public bool Equals(DepotID other)
		{
			return value == other.value;
		}

		public override bool Equals(object obj)
		{
			return obj is DepotID other && Equals(other);
		}

		public override int GetHashCode()
		{
			return (int) value;
		}

		public static bool operator ==(DepotID left, DepotID right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(DepotID left, DepotID right)
		{
			return !left.Equals(right);
		}

#if !DISABLESTEAMWORKS
		public static implicit operator DepotId_t(DepotID depotId)
		{
			return new DepotId_t(depotId.value);
		}

		public static implicit operator DepotID(DepotId_t depotId)
		{
			return new DepotID(depotId.m_DepotId);
		}
#endif
	}
}