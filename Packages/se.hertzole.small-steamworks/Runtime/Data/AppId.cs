using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

namespace Hertzole.SmallSteamworks
{
	/// <summary>
	///     Represents a Steam App ID.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct AppId : IEquatable<AppId>, IComparable<AppId>
	{
		[SerializeField]
		internal uint value;

		/// <summary>
		///     Returns an invalid AppId.
		/// </summary>
		public static AppId Invalid
		{
			get { return new AppId(0x0); }
		}

		public AppId(uint value)
		{
			this.value = value;
		}

		public override string ToString()
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		public override bool Equals(object obj)
		{
			return obj is AppId other && Equals(other);
		}

		public override int GetHashCode()
		{
			return (int) value;
		}

		public static bool operator ==(AppId left, AppId right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(AppId left, AppId right)
		{
			return !left.Equals(right);
		}

		public static bool operator >(AppId left, AppId right)
		{
			return left.value > right.value;
		}

		public static bool operator <(AppId left, AppId right)
		{
			return left.value < right.value;
		}

		public int CompareTo(AppId other)
		{
			return value.CompareTo(other.value);
		}

		public bool Equals(AppId other)
		{
			return value == other.value;
		}

#if !DISABLESTEAMWORKS
		public static implicit operator AppId_t(AppId appId)
		{
			return new AppId_t(appId.value);
		}

		public static implicit operator AppId(AppId_t appId)
		{
			return new AppId(appId.m_AppId);
		}
#endif
	}
}