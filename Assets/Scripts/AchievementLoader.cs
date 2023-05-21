using Hertzole.SmallSteamworks;
using UnityEngine;

public class AchievementLoader : MonoBehaviour
{
	[SerializeField]
	private AchievementBox box = default;

	private bool hasLoaded;

	private void Awake()
	{
		box.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
	
		if (!hasLoaded && SteamManager.Stats.HasCurrentStats)
		{
			hasLoaded = true;

			uint count = SteamManager.Achievements.NumberOfAchievements;
			for (uint i = 0; i < count; i++)
			{
				AchievementBox newBox = Instantiate(box, box.transform.parent);
				newBox.gameObject.SetActive(true);
				newBox.SetAchievement(SteamManager.Achievements.GetAchievementName(i));
			}
		}
	}
}