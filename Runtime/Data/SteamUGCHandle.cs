using System;
using System.Globalization;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

namespace Hertzole.SmallSteamworks
{
	public readonly struct SteamUGCHandle : IEquatable<SteamUGCHandle>
	{
		internal readonly ulong handle;

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