using UnityEngine;

namespace Hertzole.SmallSteamworks.CompleteSample
{
	public class AchievementsSample : BaseSample
	{
		[SerializeField]
		private AchievementBox achievementBoxPrefab = default;
		[SerializeField]
		private RectTransform achievementsContainer = default;
		[SerializeField]
		private GlobalAchievementBox globalAchievementBoxPrefab = default;
		[SerializeField]
		private RectTransform globalAchievementsContainer = default;

		private bool hasLoadedAchievements;

		private AchievementBox[] achievementBoxes;

		public override void ShowSample()
		{
			LoadAchievements();
		}

		private void LoadAchievements()
		{
			if (hasLoadedAchievements)
			{
				return;
			}

			hasLoadedAchievements = true;
			// Get the number of achievements.
			// It's important to call this method as this will prepare Steam to load the achievements!
			uint achievementCount = SteamManager.Achievements.NumberOfAchievements;

			// Create an array to store the achievement boxes.
			achievementBoxes = new AchievementBox[achievementCount];

			for (uint i = 0; i < achievementCount; i++)
			{
				// Get the API name from the Steamworks dashboard.
				string apiName = SteamManager.Achievements.GetAchievementName(i);

				// Create a new achievement box.
				AchievementBox box = Instantiate(achievementBoxPrefab, achievementsContainer);
				box.SetAchievement(apiName);

				// Add it to the array.
				achievementBoxes[i] = box;
			}

			// Get global achievements.
			SteamManager.Achievements.RequestGlobalAchievementStats(result =>
			{
				// Make sure the request was successful.
				if (result != GlobalAchievementStatsResult.Success)
				{
					return;
				}

				// Loop through all the achievements and create a box for each one.
				foreach (SteamGlobalAchievementInfo achievement in SteamManager.Achievements.GetMostAchievedAchievements())
				{
					GlobalAchievementBox box = Instantiate(globalAchievementBoxPrefab, globalAchievementsContainer);
					box.SetAchievement(achievement);
				}
			});
		}
	}
}