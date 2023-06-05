using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hertzole.SmallSteamworks.CompleteSample
{
	public class GlobalAchievementBox : MonoBehaviour
	{
		[SerializeField]
		private RawImage icon = default;
		[SerializeField]
		private TMP_Text nameLabel = default;
		[SerializeField]
		private TMP_Text unlockStatusLabel = default;
		[SerializeField]
		private TMP_Text percentageText = default;
		[SerializeField]
		private Slider progressBar = default;

		public void SetAchievement(SteamGlobalAchievementInfo info)
		{
			nameLabel.text = SteamManager.Achievements.GetAchievementDisplayName(info.ApiName);
			unlockStatusLabel.text = info.IsUnlocked ? "Unlocked" : "Locked";
			percentageText.text = $"{info.Percent.ToString("F0")}%";
			progressBar.minValue = 0;
			progressBar.maxValue = 100;
			progressBar.value = info.Percent;

			SteamManager.Achievements.GetAchievementIcon(info.ApiName, image => { icon.texture = image.Texture; });
		}
	}
}