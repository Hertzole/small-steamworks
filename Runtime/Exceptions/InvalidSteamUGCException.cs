namespace Hertzole.SmallSteamworks
{
	public sealed class InvalidSteamUGCException : SteamException
	{
		public InvalidSteamUGCException() : base("The provided UGC handle is invalid.") { }
	}
}