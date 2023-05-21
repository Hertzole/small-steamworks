namespace Hertzole.SmallSteamworks
{
	public delegate void GlobalAchievementStatsReceivedCallback(GlobalAchievementStatsResult result);

	public readonly struct GlobalAchievementStatsReceivedResponse
	{
		public GlobalAchievementStatsResult Result { get; }

		internal GlobalAchievementStatsReceivedResponse(GlobalAchievementStatsResult result)
		{
			Result = result;
		}
	}
}