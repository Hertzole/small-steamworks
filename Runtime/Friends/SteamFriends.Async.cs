#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.SmallSteamworks
{
	internal partial class SteamFriends
	{
		public Task<UserInformationRetrievedResponse> RequestUserInformationAsync(SteamID id,
			bool requireNameOnly = true,
			CancellationToken cancellationToken = default)
		{
			TaskCompletionSource<UserInformationRetrievedResponse> tcs = new TaskCompletionSource<UserInformationRetrievedResponse>();

			RequestUserInformation(id, requireNameOnly, user =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new UserInformationRetrievedResponse(user));
			});

			return tcs.Task;
		}

		public Task<AvatarRetrievedResponse> GetCurrentUserAvatarAsync(AvatarSize size, CancellationToken cancellationToken = default)
		{
			TaskCompletionSource<AvatarRetrievedResponse> tcs = new TaskCompletionSource<AvatarRetrievedResponse>();

			GetAvatar(Steamworks.SteamUser.GetSteamID(), size, (image, userId, width, height) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new AvatarRetrievedResponse(image, userId, width, height));
			});

			return tcs.Task;
		}

		public Task<AvatarRetrievedResponse> GetAvatarAsync(SteamID id, AvatarSize size, CancellationToken cancellationToken = default)
		{
			TaskCompletionSource<AvatarRetrievedResponse> tcs = new TaskCompletionSource<AvatarRetrievedResponse>();

			GetAvatar(id, size, (image, userId, width, height) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new AvatarRetrievedResponse(image, userId, width, height));
			});

			return tcs.Task;
		}
	}
}
#endif