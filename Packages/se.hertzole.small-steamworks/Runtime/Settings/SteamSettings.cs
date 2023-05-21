namespace Hertzole.SmallSteamworks
{
	public sealed class SteamSettings : ISteamSettings
	{
		public AppId AppId { get; set; } = new AppId(480);
		public bool RestartAppIfNecessary { get; set; } = false;
	}
}