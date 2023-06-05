#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
#endif

namespace Hertzole.SmallSteamworks.Helpers
{
	internal static class SteamUserHelpers
	{
		public static SteamUser GetSteamUser(SteamID steamId)
		{
#if !DISABLESTEAMWORKS
			return new SteamUser(steamId, // ID
				Steamworks.SteamFriends.GetFriendPersonaName(steamId), // Name
				Steamworks.SteamUser.GetSteamID() == steamId, // Is Me
				Steamworks.SteamFriends.GetFriendPersonaState(steamId) == EPersonaState.k_EPersonaStateOnline, // Is Online
				Steamworks.SteamFriends.GetFriendSteamLevel(steamId)); // Steam Level
#else
			return SteamUser.Invalid;
#endif
		}
	}
}