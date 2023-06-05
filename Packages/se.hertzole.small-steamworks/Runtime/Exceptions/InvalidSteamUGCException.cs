#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

namespace Hertzole.SmallSteamworks
{
	public sealed class InvalidSteamUGCException : SteamException
	{
		public InvalidSteamUGCException() : base("The provided UGC handle is invalid.") { }
	}
}