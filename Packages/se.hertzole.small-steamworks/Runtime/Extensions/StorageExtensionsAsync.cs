using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.SmallSteamworks
{
	public static partial class SteamExtensions
	{
		public static Task<FileWrittenResponse> WriteFileAsync(this ISteamStorage storage,
			string fileName,
			byte[] data,
			CancellationToken cancellationToken = default)
		{
			TaskCompletionSource<FileWrittenResponse> tcs = new TaskCompletionSource<FileWrittenResponse>();

			storage.WriteFile(fileName, data, response =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new FileWrittenResponse(response));
			});

			return tcs.Task;
		}

		public static Task<FileReadResponse> ReadFileAsync(this ISteamStorage storage, string fileName, CancellationToken cancellationToken = default)
		{
			TaskCompletionSource<FileReadResponse> tcs = new TaskCompletionSource<FileReadResponse>();

			storage.ReadFile(fileName, (success, data) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new FileReadResponse(success, data));
			});

			return tcs.Task;
		}

		public static Task<FileSharedResponse> ShareFileAsync(this ISteamStorage storage, string fileName, CancellationToken cancellationToken = default)
		{
			TaskCompletionSource<FileSharedResponse> tcs = new TaskCompletionSource<FileSharedResponse>();

			storage.ShareFile(fileName, (success, ugcHandle, name) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new FileSharedResponse(success, ugcHandle, name));
			});

			return tcs.Task;
		}

		public static Task<UGCDownloadedResponse> DownloadSharedFileAsync(this ISteamStorage storage,
			SteamUGCHandle ugcHandle,
			uint priority = 0,
			CancellationToken cancellationToken = default)
		{
			TaskCompletionSource<UGCDownloadedResponse> tcs = new TaskCompletionSource<UGCDownloadedResponse>();

			storage.DownloadSharedFile(ugcHandle, priority, (success, handle, appId, sizeInBytes, name, ownerId) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new UGCDownloadedResponse(success, handle, appId, sizeInBytes, name, ownerId));
			});

			return tcs.Task;
		}

		public static Task<UGCDownloadedResponse> DownloadSharedFileToLocationAsync(this ISteamStorage storage,
			SteamUGCHandle ugcHandle,
			string location,
			uint priority = 0,
			CancellationToken cancellationToken = default)
		{
			TaskCompletionSource<UGCDownloadedResponse> tcs = new TaskCompletionSource<UGCDownloadedResponse>();

			storage.DownloadSharedFileToLocation(ugcHandle, location, priority, (success, handle, appId, sizeInBytes, name, ownerId) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new UGCDownloadedResponse(success, handle, appId, sizeInBytes, name, ownerId));
			});

			return tcs.Task;
		}
	}
}