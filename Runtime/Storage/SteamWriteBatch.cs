#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

namespace Hertzole.SmallSteamworks
{
	public readonly struct SteamWriteBatch : IDisposable, IEquatable<SteamWriteBatch>
	{
		public bool Equals(SteamWriteBatch other)
		{
			return true;
		}

		public override bool Equals(object obj)
		{
			return obj is SteamWriteBatch other && Equals(other);
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public static bool operator ==(SteamWriteBatch left, SteamWriteBatch right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SteamWriteBatch left, SteamWriteBatch right)
		{
			return !left.Equals(right);
		}

		public void Dispose()
		{
#if !DISABLESTEAMWORKS
			SteamRemoteStorage.EndFileWriteBatch();
#endif
		}
	}
}