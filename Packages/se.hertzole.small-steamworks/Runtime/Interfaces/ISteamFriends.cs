﻿#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

#nullable enable
using System;
using System.Collections.Generic;

namespace Hertzole.SmallSteamworks
{
	public partial interface ISteamFriends : IDisposable
	{
		/// <summary>
		///     The current user.
		/// </summary>
		SteamUser CurrentUser { get; }

		/// <summary>
		///     Returns the current user's display name.
		/// </summary>
		/// <returns></returns>
		string GetCurrentUserDisplayName();

		/// <summary>
		///     <para>Returns the display name of the specified user.</para>
		///     <para>You must have requested this user's information first by calling <see cref="RequestUserInformation" />.</para>
		/// </summary>
		/// <param name="id">The ID of the user to get the name from.</param>
		/// <returns>The name of the specified user.</returns>
		string GetUserDisplayName(in SteamID id);

		/// <summary>
		///     Asynchronously requests information about the specified user.
		/// </summary>
		/// <param name="id">The ID of the user to get the info from.</param>
		/// <param name="requireNameOnly">
		///     If true, it will only request the name; otherwise it will request the avatar too, which
		///     is slower.
		/// </param>
		/// <param name="callback">The callback when the information has been retrieved.</param>
		void RequestUserInformation(in SteamID id, in bool requireNameOnly = true, UserInformationRetrievedCallback? callback = null);

		/// <summary>
		///     Asynchronously gets the avatar of the current user.
		/// </summary>
		/// <param name="size">The size of the avatar.</param>
		/// <param name="callback">The callback when the avatar has been retrieved.</param>
		void GetCurrentUserAvatar(in AvatarSize size, AvatarRetrievedCallback? callback = null);
		
		/// <summary>
		///     Asynchronously gets the avatar of the specified user.
		/// </summary>
		/// <param name="id">The ID of the user to get the avatar from.</param>
		/// <param name="size">The size of the avatar.</param>
		/// <param name="callback">The callback when the avatar has been retrieved.</param>
		void GetAvatar(in SteamID id, in AvatarSize size, AvatarRetrievedCallback? callback = null);

		/// <summary>
		///     Gets all the friends of the current user.
		/// </summary>
		/// <returns>An enumerable that you can enumerate over.</returns>
		IEnumerable<SteamUser> GetFriends();
	}
}