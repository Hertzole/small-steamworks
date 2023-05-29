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