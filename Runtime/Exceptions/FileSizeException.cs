namespace Hertzole.SmallSteamworks
{
	public sealed class FileSizeException : SteamException
	{
		public FileSizeException(string message) : base(message) { }
	}
}