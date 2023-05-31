namespace Hertzole.SmallSteamworks
{
	public interface ISteamSettings
	{
		/// <summary>
		///     The AppID of the game.
		/// </summary>
		AppID AppID { get; set; }

		/// <summary>
		///     If true, checks if your executable was launched through Steam and relaunches it through Steam if it wasn't.
		/// </summary>
		bool RestartAppIfNecessary { get; set; }

		/// <summary>
		///     If true, the current user's stats will be fetched when the game starts.
		/// </summary>
		bool FetchCurrentStatsOnBoot { get; set; }
	}
}