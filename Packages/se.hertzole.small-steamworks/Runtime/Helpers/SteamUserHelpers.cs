#if !DISABLESTEAMWORKS
using Steamworks;
using UnityEngine;
#endif

namespace Hertzole.SmallSteamworks.Helpers
{
	internal static class SteamUserHelpers
	{
		public static SteamUser GetSteamUser(SteamID steamId)
		{
#if !DISABLESTEAMWORKS
			Debug.Log($"ID: {steamId} Online: {Steamworks.SteamFriends.GetFriendPersonaState(steamId)}");
			
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