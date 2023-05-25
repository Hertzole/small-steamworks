namespace Hertzole.SmallSteamworks
{
	public interface ISteamSettings
	{
		AppId AppId { get; set; }
		
		bool RestartAppIfNecessary { get; set; }
		
		bool FetchCurrentStatsOnBoot { get; set; }
	}
}