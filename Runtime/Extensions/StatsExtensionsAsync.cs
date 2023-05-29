using System.Threading;
using System.Threading.Tasks;
using Hertzole.SmallSteamworks.Helpers;

namespace Hertzole.SmallSteamworks
{
	public static partial class SteamExtensions
	{
		public static Task<UserStatsReceivedResponse> RequestCurrentStatsAsync(this ISteamStats stats, CancellationToken cancellationToken = default)
		{
			ThrowHelper.ThrowIfNull(stats, nameof(stats));
			
			TaskCompletionSource<UserStatsReceivedResponse> tcs = new TaskCompletionSource<UserStatsReceivedResponse>();

			stats.RequestCurrentStats((success, steamUserStats) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new UserStatsReceivedResponse(success, steamUserStats));
			});

			return tcs.Task;
		}

		public static Task<UserStatsReceivedResponse> RequestUserStatsAsync(this ISteamStats stats,
			in SteamID steamId,
			CancellationToken cancellationToken = default)
		{
			ThrowHelper.ThrowIfNull(stats, nameof(stats));

			TaskCompletionSource<UserStatsReceivedResponse> tcs = new TaskCompletionSource<UserStatsReceivedResponse>();

			stats.RequestUserStats(steamId, (success, steamUserStats) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new UserStatsReceivedResponse(success, steamUserStats));
			});

			return tcs.Task;
		}
	}
}