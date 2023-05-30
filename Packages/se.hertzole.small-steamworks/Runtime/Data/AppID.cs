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
	public struct AppID : IEquatable<AppID>, IComparable<AppID>
	{
		[SerializeField]
		internal uint value;

		/// <summary>
		///     Returns an invalid AppId.
		/// </summary>
		public static AppID Invalid
		{
			get { return new AppID(0x0); }
		}

		public AppID(uint value)
		{
			this.value = value;
		}

		public override string ToString()
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		public override int GetHashCode()
		{
			return (int) value;
		}

		public static bool operator ==(AppID left, AppID right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(AppID left, AppID right)
		{
			return !left.Equals(right);
		}

		public static bool operator >(AppID left, AppID right)
		{
			return left.value > right.value;
		}

		public static bool operator <(AppID left, AppID right)
		{
			return left.value < right.value;
		}
		
		public static bool operator >=(AppID left, AppID right)
		{
			return left.value >= right.value;
		}
		
		public static bool operator <=(AppID left, AppID right)
		{
			return left.value <= right.value;
		}

		public int CompareTo(AppID other)
		{
			return value.CompareTo(other.value);
		}
		
		public override bool Equals(object obj)
		{
			return obj is AppID other && Equals(other);
		}

		public bool Equals(AppID other)
		{
			return value == other.value;
		}

#if !DISABLESTEAMWORKS
		public static implicit operator AppId_t(AppID appId)
		{
			return new AppId_t(appId.value);
		}

		public static implicit operator AppID(AppId_t appId)
		{
			return new AppID(appId.m_AppId);
		}
#endif
	}
}