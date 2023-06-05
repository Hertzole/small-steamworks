#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

#nullable enable
using Hertzole.SmallSteamworks.Helpers;

namespace Hertzole.SmallSteamworks
{
	public static partial class SteamExtensions
	{
		public static void GetMyAvatar(this ISteamFriends friends, AvatarSize size, AvatarRetrievedCallback? callback = null)
		{
			ThrowHelper.ThrowIfNull(friends, nameof(friends));
			
#if !DISABLESTEAMWORKS
			friends.GetAvatar(Steamworks.SteamUser.GetSteamID(), size, callback);
#endif
		}
	}
}