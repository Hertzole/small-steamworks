#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif
#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.SmallSteamworks
{
	public partial interface ISteamFriends
	{
		/// <summary>
		///     Asynchronously requests information about the specified user.
		/// </summary>
		/// <param name="id">The ID of the user to get the info from.</param>
		/// <param name="requireNameOnly">
		///     If true, it will only request the name; otherwise it will request the avatar too, which
		///     is slower.
		/// </param>
		/// <param name="cancellationToken">Optional cancellation token to cancel the task.</param>
		Task<UserInformationRetrievedResponse> RequestUserInformationAsync(SteamID id,
			bool requireNameOnly = true,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Asynchronously gets the avatar of the current user.
		/// </summary>
		/// <param name="size">The size of the avatar.</param>
		/// <param name="cancellationToken">Optional cancellation token to cancel the task.</param>
		Task<AvatarRetrievedResponse> GetCurrentUserAvatarAsync(AvatarSize size, CancellationToken cancellationToken = default);

		/// <summary>
		///     Asynchronously gets the avatar of the specified user.
		/// </summary>
		/// <param name="id">The ID of the user to get the avatar from.</param>
		/// <param name="size">The size of the avatar.</param>
		/// <param name="cancellationToken">Optional cancellation token to cancel the task.</param>
		Task<AvatarRetrievedResponse> GetAvatarAsync(SteamID id, AvatarSize size, CancellationToken cancellationToken = default);
	}
}