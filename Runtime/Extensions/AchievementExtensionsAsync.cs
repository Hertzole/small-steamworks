using System.Threading;
using System.Threading.Tasks;
using Hertzole.SmallSteamworks.Helpers;

namespace Hertzole.SmallSteamworks
{
	public static partial class SteamExtensions
	{
		/// <summary>
		///     <para>Asynchronously gets the icon for the achievement.</para>
		///     <para>
		///         This method automatically calls <see cref="ISteamStats.RequestCurrentStats" /> if needed, so you don't need to
		///         call it.
		///     </para>
		/// </summary>
		/// <param name="achievements"></param>
		/// <param name="achievementName">The 'API Name' of the achievement.</param>
		/// <param name="cancellationToken">Optional cancellation token for cancelling the task.</param>
		/// <returns>
		///     The Steam Image when the icon is downloaded. The icon may be null if something goes wrong when requesting the
		///     icon.
		/// </returns>
		public static async Task<SteamImage?> GetAchievementIconAsync(this ISteamAchievements achievements, string achievementName, CancellationToken cancellationToken = default)
		{
			ThrowHelper.ThrowIfNull(achievements, nameof(achievements));
			
			bool hasCurrentStats = false;
			bool currentStatsSuccess = false;

			SteamManager.Stats.RequestCurrentStats((success, _) =>
			{
				currentStatsSuccess = success;
				hasCurrentStats = true;
			});

			while (!hasCurrentStats)
			{
				cancellationToken.ThrowIfCancellationRequested();
				await Task.Yield();
			}

			if (!currentStatsSuccess)
			{
				return null;
			}

			SteamImage? result = null;

			achievements.GetAchievementIcon(achievementName, image => { result = image; });

			while (result == null)
			{
				cancellationToken.ThrowIfCancellationRequested();
				await Task.Yield();
			}

			return result;
		}

		/// <summary>
		///     <para>
		///         Asynchronously fetch the data for the percentage of players who have received each achievement for the
		///         current game globally.
		///     </para>
		///     <para>
		///         This method automatically calls <see cref="ISteamStats.RequestCurrentStats" /> if needed, so you don't need to
		///         call it.
		///     </para>
		/// </summary>
		/// <param name="achievements"></param>
		/// <param name="cancellationToken">Optional cancellation token for cancelling the task.</param>
		/// <returns>A global achievements stats response with the result whether the call was successful or not.</returns>
		public static async Task<GlobalAchievementStatsReceivedResponse> RequestGlobalAchievementStatsAsync(this ISteamAchievements achievements, CancellationToken cancellationToken = default)
		{
			ThrowHelper.ThrowIfNull(achievements, nameof(achievements));
			
			bool hasCurrentStats = false;
			bool currentStatsSuccess = false;

			SteamManager.Stats.RequestCurrentStats((success, _) =>
			{
				currentStatsSuccess = success;
				hasCurrentStats = true;
			});

			while (!hasCurrentStats)
			{
				cancellationToken.ThrowIfCancellationRequested();
				await Task.Yield();
			}

			if (!currentStatsSuccess)
			{
				return new GlobalAchievementStatsReceivedResponse(GlobalAchievementStatsResult.Fail);
			}

			GlobalAchievementStatsReceivedResponse? response = null;

			achievements.RequestGlobalAchievementStats(result => { response = new GlobalAchievementStatsReceivedResponse(result); });

			while (response == null)
			{
				cancellationToken.ThrowIfCancellationRequested();
				await Task.Yield();
			}

			return response.Value;
		}
	}
}