#nullable enable
using System;
using System.Collections.Generic;

namespace Hertzole.SmallSteamworks
{
	public interface ISteamFriends : IDisposable
	{
		SteamUser Me { get; }

		string GetMyPersonaName();

		string GetUserPersonaName(SteamID id);

		void RequestUserInformation(SteamID id, bool requireNameOnly = true, UserInformationRetrievedCallback? callback = null);

		void GetAvatar(SteamID id, AvatarSize size, AvatarRetrievedCallback? callback = null);

		IEnumerable<SteamUser> GetFriends();
	}
}