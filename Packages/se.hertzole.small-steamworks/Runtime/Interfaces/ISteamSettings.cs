namespace Hertzole.SmallSteamworks
{
	public interface ISteamSettings
	{
		AppID AppID { get; set; }
		
		bool RestartAppIfNecessary { get; set; }
		
		bool FetchCurrentStatsOnBoot { get; set; }
	}
}