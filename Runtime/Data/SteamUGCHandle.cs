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
	///     A handle to a UGC file.
	/// </summary>
	public readonly struct SteamUGCHandle : IEquatable<SteamUGCHandle>
	{
		internal readonly ulong handle;

		public static SteamUGCHandle Invalid { get; } = new SteamUGCHandle(0xffffffffffffffff);

		/// <summary>
		///     Whether the handle is valid. If it's invalid, it can't be used to open a file.
		/// </summary>
		public bool IsValid
		{
			get { return handle != Invalid.handle; }
		}

		internal SteamUGCHandle(ulong handle)
		{
			this.handle = handle;
		}

		public bool Equals(SteamUGCHandle other)
		{
			return handle == other.handle;
		}

		public override bool Equals(object obj)
		{
			return obj is SteamUGCHandle other && Equals(other);
		}

		public override int GetHashCode()
		{
			return handle.GetHashCode();
		}

		public static bool operator ==(SteamUGCHandle left, SteamUGCHandle right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SteamUGCHandle left, SteamUGCHandle right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return handle.ToString(CultureInfo.InvariantCulture);
		}

#if !DISABLESTEAMWORKS
		public static implicit operator SteamUGCHandle(UGCHandle_t handle)
		{
			return new SteamUGCHandle(handle.m_UGCHandle);
		}

		public static implicit operator UGCHandle_t(SteamUGCHandle handle)
		{
			return new UGCHandle_t(handle.handle);
		}
#endif
	}
}