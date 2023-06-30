#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif
#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.SmallSteamworks
{
	public partial interface ISteamApps
	{
		/// <summary>
		///     Asynchronously retrieves metadata details about a specific file in the depot manifest.
		/// </summary>
		/// <param name="fileName">	The absolute path and name to the file.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping the task.</param>
		/// <returns>A file details response.</returns>
		Task<FileDetailsResponse> GetFileDetailsAsync(in string fileName, CancellationToken cancellationToken = default);
	}
}