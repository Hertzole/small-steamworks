namespace Hertzole.SmallSteamworks
{
	public delegate void AchievementIconReceivedCallback(SteamImage image);

	public readonly struct AchievementIconReceivedResponse
	{
		public SteamImage Image { get; }

		internal AchievementIconReceivedResponse(SteamImage image)
		{
			Image = image;
		}
	}
}