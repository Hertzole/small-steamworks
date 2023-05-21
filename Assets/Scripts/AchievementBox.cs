using TMPro;
using Hertzole.SmallSteamworks;
using UnityEngine;
using UnityEngine.UI;

public class AchievementBox : MonoBehaviour
{
	[SerializeField]
	private RawImage icon = default;
	[SerializeField]
	private TMP_Text nameLabel = default;
	[SerializeField]
	private TMP_Text percentLabel = default;

	public void SetAchievement(string achievementName)
	{
		nameLabel.text = SteamManager.Achievements.GetAchievementDisplayName(achievementName);

		SteamManager.Achievements.GetAchievementIcon(achievementName, image => { icon.texture = image.Texture; });
	}
}