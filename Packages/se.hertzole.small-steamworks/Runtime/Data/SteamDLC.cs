#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;

namespace Hertzole.SmallSteamworks
{
	/// <summary>
	///     Represents a optional Steam DLC.
	/// </summary>
	public readonly struct SteamDLC : IEquatable<SteamDLC>
	{
		/// <summary>
		///     The App ID of the DLC.
		/// </summary>
		public AppID AppID { get; }
		/// <summary>
		///     Returns <c>true</c> if the DLC is currently available on the Steam store; otherwise, <c>false</c>.
		/// </summary>
		public bool IsAvailableOnSteamStore { get; }
		/// <summary>
		///     Returns <c>true</c> if the DLC is currently installed; otherwise, <c>false</c>.
		/// </summary>
		public bool IsInstalled { get; }
		/// <summary>
		///     The name of the DLC.
		/// </summary>
		public string Name { get; }

		internal SteamDLC(AppID appID, bool isAvailableOnSteamStore, bool isInstalled, string name)
		{
			AppID = appID;
			IsAvailableOnSteamStore = isAvailableOnSteamStore;
			IsInstalled = isInstalled;
			Name = name;
		}

		public override bool Equals(object obj)
		{
			return obj is SteamDLC other && Equals(other);
		}

		public bool Equals(SteamDLC other)
		{
			return AppID.Equals(other.AppID) && IsAvailableOnSteamStore == other.IsAvailableOnSteamStore && IsInstalled == other.IsInstalled &&
			       Name == other.Name;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = AppID.GetHashCode();
				hashCode = (hashCode * 397) ^ IsAvailableOnSteamStore.GetHashCode();
				hashCode = (hashCode * 397) ^ IsInstalled.GetHashCode();
				hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(SteamDLC left, SteamDLC right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SteamDLC left, SteamDLC right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return
				$"{nameof(AppID)}: {AppID}, {nameof(IsAvailableOnSteamStore)}: {IsAvailableOnSteamStore}, {nameof(IsInstalled)}: {IsInstalled}, {nameof(Name)}: {Name}";
		}
	}
}