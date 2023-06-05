#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

namespace Hertzole.SmallSteamworks
{
	public enum AvatarSize
	{
		/// <summary>
		///     32x32 avatar size.
		/// </summary>
		Small = 0,
		/// <summary>
		///     64x64 avatar size.
		/// </summary>
		Medium = 1,
		/// <summary>
		///     128x128 avatar size.
		/// </summary>
		Large = 2
	}
}