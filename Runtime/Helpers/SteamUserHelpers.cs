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