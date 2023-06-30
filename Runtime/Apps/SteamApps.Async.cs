#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif
#nullable enable

#if !DISABLESTEAMWORKS
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.SmallSteamworks
{
	internal partial class SteamApps
	{
		public Task<FileDetailsResponse> GetFileDetailsAsync(in string fileName, CancellationToken cancellationToken = default)
		{
			TaskCompletionSource<FileDetailsResponse> tcs = new TaskCompletionSource<FileDetailsResponse>();

			GetFileDetails(fileName, (exists, size, sha) =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				tcs.SetResult(new FileDetailsResponse(exists, size, sha));
			});

			return tcs.Task;
		}
	}
}
#endif