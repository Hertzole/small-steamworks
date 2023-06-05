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
	public readonly struct SteamLeaderboard : IEquatable<SteamLeaderboard>
	{
		internal readonly ulong handle;

		public bool IsValid { get { return handle != 0; } }

		public string Name { get; }
		public int EntryCount { get; }
		public LeaderboardDisplayType DisplayType { get; }
		public LeaderboardSortMethod SortMethod { get; }

		public static SteamLeaderboard Invalid { get { return new SteamLeaderboard(0); } }

		internal SteamLeaderboard(ulong handle)
		{
			this.handle = handle;

#if !DISABLESTEAMWORKS
			SteamLeaderboard_t leaderboard = new SteamLeaderboard_t(handle);

			// If the handle is 0 (invalid), just default the values.
			Name = handle == 0 ? string.Empty : SteamUserStats.GetLeaderboardName(leaderboard);
			EntryCount = handle == 0 ? 0 : SteamUserStats.GetLeaderboardEntryCount(leaderboard);
			DisplayType = handle == 0 ? LeaderboardDisplayType.Numeric : SteamUserStats.GetLeaderboardDisplayType(leaderboard).FromSteam();
			SortMethod = handle == 0 ? LeaderboardSortMethod.Ascending : SteamUserStats.GetLeaderboardSortMethod(leaderboard).FromSteam();
#else
			Name = string.Empty;
			EntryCount = 0;
			DisplayType = LeaderboardDisplayType.Numeric;
			SortMethod = LeaderboardSortMethod.Ascending;
#endif
		}

		public bool Equals(SteamLeaderboard other)
		{
			return handle == other.handle;
		}

		public override bool Equals(object obj)
		{
			return obj is SteamLeaderboard other && Equals(other);
		}

		public override int GetHashCode()
		{
			return handle.GetHashCode();
		}

		public static bool operator ==(SteamLeaderboard left, SteamLeaderboard right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SteamLeaderboard left, SteamLeaderboard right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return handle.ToString(CultureInfo.InvariantCulture);
		}

#if !DISABLESTEAMWORKS
		public static implicit operator SteamLeaderboard_t(SteamLeaderboard leaderboard)
		{
			return new SteamLeaderboard_t(leaderboard.handle);
		}
		
		public static implicit operator SteamLeaderboard(SteamLeaderboard_t leaderboard)
		{
			return new SteamLeaderboard(leaderboard.m_SteamLeaderboard);
		}
#endif
	}
}