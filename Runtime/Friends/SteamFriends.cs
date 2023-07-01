#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
#nullable enable
using System;
using System.Collections.Generic;
using Hertzole.SmallSteamworks.Helpers;
using Steamworks;

namespace Hertzole.SmallSteamworks
{
	internal sealed partial class SteamFriends : ISteamFriends
	{
		private readonly SteamLogger<SteamFriends> logger = new SteamLogger<SteamFriends>();

		private readonly SteamCallback<PersonaStateChange_t> onPersonaStateChangedCallback;
		private SteamCallback<AvatarImageLoaded_t>? onAvatarLoadedCallback;

		public SteamUser CurrentUser
		{
			get { return SteamUserHelpers.GetSteamUser(Steamworks.SteamUser.GetSteamID()); }
		}

		public SteamFriends()
		{
			onPersonaStateChangedCallback = new SteamCallback<PersonaStateChange_t>(CallbackType.Callback, OnPersonaStateChanged);
		}

		public string GetCurrentUserDisplayName()
		{
			return Steamworks.SteamFriends.GetPersonaName();
		}

		public string GetUserDisplayName(in SteamID id)
		{
			return Steamworks.SteamFriends.GetFriendPersonaName(id);
		}

		public void RequestUserInformation(in SteamID id, in bool requireNameOnly = true, UserInformationRetrievedCallback? callback = null)
		{
			logger.Log($"Requesting user information for {id} with name only: {requireNameOnly}");

			bool needsToBeFetched = Steamworks.SteamFriends.RequestUserInformation(id, requireNameOnly);
			logger.Log($"{id} needs to be fetched: {needsToBeFetched}");
			if (needsToBeFetched)
			{
				// Create new values here to avoid a closure allocation in the lambda.
				SteamID steamId = id;

				onPersonaStateChangedCallback.RegisterOnce(t =>
				{
					logger.Log($"Persona state changed callback received for user {t.m_ulSteamID} with {t.m_nChangeFlags}.");
					callback?.Invoke(SteamUserHelpers.GetSteamUser(new SteamID(t.m_ulSteamID)));
				}, t => t.m_ulSteamID == steamId);
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

		public void GetCurrentUserAvatar(in AvatarSize size, AvatarRetrievedCallback? callback = null)
		{
			GetAvatar(Steamworks.SteamUser.GetSteamID(), size, callback);
		}

		public void GetAvatar(in SteamID id, in AvatarSize size, AvatarRetrievedCallback? callback = null)
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
				logger.Log($"Avatar is already loaded for {id} with handle {handle}");
				bool gotSize = SteamUtils.GetImageSize(handle, out uint width, out uint height);
				if (gotSize)
				{
					SteamImage img = new SteamImage(handle);
					SteamImageCache.SetImageName(img, $"Avatar {id} ({size})");
					callback?.Invoke(img, id, (int) width, (int) height);
				}
				else
				{
					logger.LogError("Failed to get image size.");
					callback?.Invoke(new SteamImage(0), id, 0, 0);
				}

				return;
			}

			// Create new values here to avoid a closure allocation in the lambda.
			SteamID currentId = id;
			AvatarSize newSize = size;

			// Avatar is not loaded yet. Wait for callback.
			onAvatarLoadedCallback.RegisterOnce(t =>
			{
				logger.Log(
					$"Avatar loaded callback received :: {nameof(t.m_iImage)}: {t.m_iImage}, {nameof(t.m_steamID)}: {t.m_steamID}, {nameof(t.m_iWide)}: {t.m_iWide}, {nameof(t.m_iTall)}: {t.m_iTall}");

				SteamImage img = new SteamImage(t.m_iImage);
				SteamImageCache.SetImageName(img, $"Avatar {t.m_steamID} ({newSize})");
				callback?.Invoke(img, t.m_steamID, t.m_iWide, t.m_iTall);
			}, t => t.m_steamID == currentId);
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