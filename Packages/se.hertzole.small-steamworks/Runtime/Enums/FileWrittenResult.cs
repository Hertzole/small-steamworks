#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

namespace Hertzole.SmallSteamworks
{
	/// <summary>
	///     Possible results of a file write using the Steam Cloud.
	/// </summary>
	public enum FileWrittenResult
	{
		/// <summary>
		///     The file was written successfully.
		/// </summary>
		Success = 0,
		/// <summary>
		///     The file write failed for some unknown reason.
		/// </summary>
		Failed = 1,
		/// <summary>
		///     The file write failed because the user ran out of storage space in the cloud.
		/// </summary>
		QuotaExceeded = 2
	}
}