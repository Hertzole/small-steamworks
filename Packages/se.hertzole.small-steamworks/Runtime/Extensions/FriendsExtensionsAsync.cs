#nullable enable
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.SmallSteamworks
{
	public static partial class SteamExtensions
	{
		public static Task<UserInformationRetrievedResponse> RequestUserInformationAsync(this ISteamFriends friends,
			SteamID id,
			bool requireNameOnly = true,
			CancellationToken cancellationToken = default)
		{
			TaskCompletionSource<UserInformationRetrievedResponse> tcs = new TaskCompletionSource<UserInformationRetrievedResponse>();

			friends.RequestUserInformation(id, requireNameOnly, user =>
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

		public static Task<AvatarRetrievedResponse> GetMyAvatarAsync(this ISteamFriends friends, AvatarSize size, CancellationToken cancellationToken = default)
		{
			TaskCompletionSource<AvatarRetrievedResponse> tcs = new TaskCompletionSource<AvatarRetrievedResponse>();

			friends.GetAvatar(Steamworks.SteamUser.GetSteamID(), size, (image, id, width, height) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new AvatarRetrievedResponse(image, id, width, height));
			});

			return tcs.Task;
		}
	}
}