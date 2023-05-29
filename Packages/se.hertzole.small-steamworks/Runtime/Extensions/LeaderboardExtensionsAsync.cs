#nullable enable
using System.Threading;
using System.Threading.Tasks;
using Hertzole.SmallSteamworks.Helpers;

namespace Hertzole.SmallSteamworks
{
	public static partial class SteamExtensions
	{
		public static Task<FindLeaderboardResponse> FindLeaderboardAsync(this ISteamLeaderboards leaderboards, in string leaderboardName, CancellationToken cancellationToken = default)
		{
			ThrowHelper.ThrowIfNull(leaderboards, nameof(leaderboards));

			TaskCompletionSource<FindLeaderboardResponse> tcs = new TaskCompletionSource<FindLeaderboardResponse>();

			leaderboards.FindLeaderboard(leaderboardName, (success, steamLeaderboard) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new FindLeaderboardResponse(success, steamLeaderboard));
			});

			return tcs.Task;
		}
		
		public static Task<FindLeaderboardResponse> FindOrCreateLeaderboardAsync(this ISteamLeaderboards leaderboards, in string leaderboardName, in LeaderboardSortMethod sortMethod, in LeaderboardDisplayType displayType, CancellationToken cancellationToken = default)
		{
			ThrowHelper.ThrowIfNull(leaderboards, nameof(leaderboards));
			
			TaskCompletionSource<FindLeaderboardResponse> tcs = new TaskCompletionSource<FindLeaderboardResponse>();

			leaderboards.FindOrCreateLeaderboard(leaderboardName, sortMethod, displayType, (success, steamLeaderboard) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new FindLeaderboardResponse(success, steamLeaderboard));
			});

			return tcs.Task;
		}

		public static Task<UploadScoreResponse> UploadScoreAsync(this ISteamLeaderboards leaderboards, in SteamLeaderboard leaderboard, in LeaderboardUploadScoreMethod uploadScoreMethod, in int score, in int[]? scoreDetails = null, CancellationToken cancellationToken = default)
		{
			ThrowHelper.ThrowIfNull(leaderboards, nameof(leaderboards));
			
			TaskCompletionSource<UploadScoreResponse> tcs = new TaskCompletionSource<UploadScoreResponse>();

			leaderboards.UploadScore(leaderboard, uploadScoreMethod, score, scoreDetails, (success, steamLeaderboard, score, scoreChanged, newRank, previousRank) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new UploadScoreResponse(success, steamLeaderboard, score, scoreChanged, newRank, previousRank));
			});

			return tcs.Task;
		}

		public static Task<UploadScoreResponse> UploadScoreAsync(this in SteamLeaderboard leaderboard, in LeaderboardUploadScoreMethod uploadScoreMethod, in int score, in int[]? scoreDetails = null, CancellationToken cancellationToken = default)
		{
			return SteamManager.Leaderboards.UploadScoreAsync(leaderboard, uploadScoreMethod, score, scoreDetails, cancellationToken);
		}

		public static Task<AttachLeaderboardUGCResponse> AttachLeaderboardUGCAsync(this ISteamLeaderboards leaderboards, in SteamLeaderboard leaderboard, in SteamUGCHandle ugcHandle, CancellationToken cancellationToken = default)
		{
			ThrowHelper.ThrowIfNull(leaderboards, nameof(leaderboards));
			
			TaskCompletionSource<AttachLeaderboardUGCResponse> tcs = new TaskCompletionSource<AttachLeaderboardUGCResponse>();

			leaderboards.AttachLeaderboardUGC(leaderboard, ugcHandle, (success, steamLeaderboard) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new AttachLeaderboardUGCResponse(success, steamLeaderboard));
			});

			return tcs.Task;
		}

		public static Task<AttachLeaderboardUGCResponse> AttachLeaderboardUGCAsync(this in SteamLeaderboard leaderboard, in SteamUGCHandle ugcHandle, CancellationToken cancellationToken = default)
		{
			return SteamManager.Leaderboards.AttachLeaderboardUGCAsync(leaderboard, ugcHandle, cancellationToken);
		}

		public static Task<GetScoresResponse> GetScoresAsync(this ISteamLeaderboards leaderboards, in SteamLeaderboard leaderboard, in int count, in int offset = 0, CancellationToken cancellationToken = default)
		{
			ThrowHelper.ThrowIfNull(leaderboards, nameof(leaderboards));
			
			TaskCompletionSource<GetScoresResponse> tcs = new TaskCompletionSource<GetScoresResponse>();

			leaderboards.GetScores(leaderboard, count, offset, (success, steamLeaderboard, entries) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new GetScoresResponse(success, steamLeaderboard, entries));
			});

			return tcs.Task;
		}

		public static Task<GetScoresResponse> GetScoresAsync(this in SteamLeaderboard leaderboard, in int count, in int offset = 0, CancellationToken cancellationToken = default)
		{
			return SteamManager.Leaderboards.GetScoresAsync(leaderboard, count, offset, cancellationToken);
		}

		public static Task<GetScoresResponse> GetScoresFromFriendsAsync(this ISteamLeaderboards leaderboards, in SteamLeaderboard leaderboard, CancellationToken cancellationToken = default)
		{
			ThrowHelper.ThrowIfNull(leaderboards, nameof(leaderboards));
			
			TaskCompletionSource<GetScoresResponse> tcs = new TaskCompletionSource<GetScoresResponse>();

			leaderboards.GetScoresFromFriends(leaderboard, (success, steamLeaderboard, entries) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new GetScoresResponse(success, steamLeaderboard, entries));
			});

			return tcs.Task;
		}

		public static Task<GetScoresResponse> GetScoresFromFriendsAsync(this in SteamLeaderboard leaderboard, CancellationToken cancellationToken = default)
		{
			return SteamManager.Leaderboards.GetScoresFromFriendsAsync(leaderboard, cancellationToken);
		}

		public static Task<GetScoresResponse> GetScoresAroundUserAsync(this ISteamLeaderboards leaderboards, in SteamLeaderboard leaderboard, in int rangeStart = 10, in int rangeEnd = 10, CancellationToken cancellationToken = default)
		{
			ThrowHelper.ThrowIfNull(leaderboards, nameof(leaderboards));
			
			TaskCompletionSource<GetScoresResponse> tcs = new TaskCompletionSource<GetScoresResponse>();

			leaderboards.GetScoresAroundUser(leaderboard, rangeStart, rangeEnd, (success, steamLeaderboard, entries) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new GetScoresResponse(success, steamLeaderboard, entries));
			});

			return tcs.Task;
		}

		public static Task<GetScoresResponse> GetScoresAroundUserAsync(this in SteamLeaderboard leaderboard, in int rangeStart = 10, in int rangeEnd = 10, CancellationToken cancellationToken = default)
		{
			return SteamManager.Leaderboards.GetScoresAroundUserAsync(leaderboard, rangeStart, rangeEnd, cancellationToken);
		}
	}
}