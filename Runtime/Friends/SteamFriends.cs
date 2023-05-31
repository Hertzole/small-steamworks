#if !DISABLESTEAMWORKS
#nullable enable
using System;
using System.Collections.Generic;
using Hertzole.SmallSteamworks.Helpers;
using Steamworks;

namespace Hertzole.SmallSteamworks
{
	internal sealed class SteamFriends : ISteamFriends
	{
		private readonly SteamLogger<SteamFriends> logger = new SteamLogger<SteamFriends>();

		private readonly SteamCallback<PersonaStateChange_t> onPersonaStateChangedCallback;
		private SteamCallback<AvatarImageLoaded_t>? onAvatarLoadedCallback;

		public SteamFriends()
		{
			onPersonaStateChangedCallback = new SteamCallback<PersonaStateChange_t>(CallbackType.Callback, OnPersonaStateChanged);
		}

		public SteamUser CurrentUser { get { return SteamUserHelpers.GetSteamUser(Steamworks.SteamUser.GetSteamID()); } }

		public string GetCurrentUserDisplayName()
		{
			return Steamworks.SteamFriends.GetPersonaName();
		}

		public string GetUserDisplayName(SteamID id)
		{
			return Steamworks.SteamFriends.GetFriendPersonaName(id);
		}

		public void RequestUserInformation(SteamID id, bool requireNameOnly = true, UserInformationRetrievedCallback? callback = null)
		{
			logger.Log($"Requesting user information for {id} with name only: {requireNameOnly}");

			bool needsToBeFetched = Steamworks.SteamFriends.RequestUserInformation(id, requireNameOnly);
			logger.Log($"{id} needs to be fetched: {needsToBeFetched}");
			if (needsToBeFetched)
			{
				onPersonaStateChangedCallback.RegisterOnce(t =>
				{
					logger.Log($"Persona state changed callback received for user {t.m_ulSteamID} with {t.m_nChangeFlags}.");
					callback?.Invoke(SteamUserHelpers.GetSteamUser(new SteamID(t.m_ulSteamID)));
				}, t => t.m_ulSteamID == id);
			}
			else
			{
				callback?.Invoke(SteamUserHelpers.GetSteamUser(id));
			}
		}

		private void OnPersonaStateChanged(PersonaStateChange_t obj)
		{
			logger.Log($"{obj.m_ulSteamID} has changed state with {obj.m_nChangeFlags}.");
		}

		public void GetAvatar(SteamID id, AvatarSize size, AvatarRetrievedCallback? callback = null)
		{
			logger.Log($"Getting avatar for {id} with size {size}");

			onAvatarLoadedCallback ??= new SteamCallback<AvatarImageLoaded_t>(CallbackType.Callback);

			int handle;
			switch (size)
			{
				case AvatarSize.Small:
					handle = Steamworks.SteamFriends.GetSmallFriendAvatar(id);
					break;
				case AvatarSize.Medium:
					handle = Steamworks.SteamFriends.GetMediumFriendAvatar(id);
					break;
				case AvatarSize.Large:
					handle = Steamworks.SteamFriends.GetLargeFriendAvatar(id);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(size), size, null);
			}

			// No avatar set.
			if (handle == 0)
			{
				logger.Log($"Avatar handle is 0. User {id} does not have avatar.");
				callback?.Invoke(new SteamImage(0), id, 0, 0);
				return;
			}

			// Avatar is already loaded if it isn't -1.
			if (handle != -1)
			{
				logger.Log($"Avatar is already loaded for {id}");
				bool gotSize = SteamUtils.GetImageSize(handle, out uint width, out uint height);
				if (gotSize)
				{
					callback?.Invoke(new SteamImage(handle), id, (int) width, (int) height);
				}
				else
				{
					logger.LogError("Failed to get image size.");
					callback?.Invoke(new SteamImage(0), id, 0, 0);
				}

				return;
			}

			// Avatar is not loaded yet. Wait for callback.
			onAvatarLoadedCallback.RegisterOnce(t =>
			{
				logger.Log($"Avatar loaded callback received :: {nameof(t.m_iImage)}: {t.m_iImage}, {nameof(t.m_steamID)}: {t.m_steamID}, {nameof(t.m_iWide)}: {t.m_iWide}, {nameof(t.m_iTall)}: {t.m_iTall}");
				callback?.Invoke(new SteamImage(t.m_iImage), id, t.m_iWide, t.m_iTall);
			}, t => t.m_steamID == id);
		}

		public IEnumerable<SteamUser> GetFriends()
		{
			int count = Steamworks.SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
			for (int i = 0; i < count; i++)
			{
				SteamID id = Steamworks.SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
				yield return SteamUserHelpers.GetSteamUser(id);
			}
		}

		public void Dispose()
		{
			onPersonaStateChangedCallback?.Dispose();
			onAvatarLoadedCallback?.Dispose();
		}
	}
}
#endif