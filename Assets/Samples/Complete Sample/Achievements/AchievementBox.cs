using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Hertzole.SmallSteamworks.CompleteSample
{
	public class AchievementBox : MonoBehaviour
	{
		[SerializeField] 
		private RawImage icon = default;
		[SerializeField] 
		private TMP_Text displayNameLabel = default;
		[SerializeField] 
		private TMP_Text descriptionLabel = default;
		[SerializeField] 
		private TMP_Text unlockTimeLabel = default;
		[SerializeField]
		private TMP_Text unlockLabel = default;
		[SerializeField] 
		private Button toggleLockButton = default;
		[SerializeField] 
		private TMP_Text buttonLabel = default;

		private string achievementApiName;
		private string displayName;
		private string description;
		
		private bool isUnlocked;
		private bool isHidden;
		private DateTime unlockTime;

		private void Awake()
		{
			toggleLockButton.onClick.AddListener(() =>
			{
				if (isUnlocked)
				{
					SteamManager.Achievements.ResetAchievement(achievementApiName);
					isUnlocked = false;
					UpdateVisuals();
				}
				else
				{
					SteamManager.Achievements.UnlockAchievement(achievementApiName);
				}
			});
		}

		private void OnEnable()
		{
			if (!string.IsNullOrEmpty(achievementApiName))
			{
				isUnlocked = SteamManager.Achievements.IsAchievementUnlocked(achievementApiName, out unlockTime);
				UpdateVisuals();
			}
			
			SteamManager.Achievements.OnAchievementUnlocked += OnAchievementUnlocked;
		}
		
		private void OnDisable()
		{
			if (SteamManager.IsInitialized)
			{
				SteamManager.Achievements.OnAchievementUnlocked -= OnAchievementUnlocked;
			}
		}

		private void OnAchievementUnlocked(string achievementName, DateTime newUnlockTime)
		{
			if (!achievementApiName.Equals(achievementName, StringComparison.Ordinal))
			{
				return;
			}

			isUnlocked = true;
			unlockTime = newUnlockTime;
			UpdateVisuals();
		}

		public void SetAchievement(string apiName)
		{
			achievementApiName = apiName;
			SteamAchievement achievementInfo = SteamManager.Achievements.GetAchievementInfo(achievementApiName);
			isUnlocked = achievementInfo.IsUnlocked;
			isHidden = achievementInfo.IsHidden;
			unlockTime = achievementInfo.UnlockTime;
			displayName = achievementInfo.DisplayName;
			description = achievementInfo.Description;

			UpdateVisuals();
		}

		private void UpdateVisuals()
		{
			if (isHidden && !isUnlocked)
			{
				displayNameLabel.text = "???";
				descriptionLabel.text = "???";
			}
			else
			{
				displayNameLabel.text = displayName;
				descriptionLabel.text = description;
			}
			
			unlockTimeLabel.text = isUnlocked ? $"{unlockTime:yyyy-MM-dd HH:mm:ss}" : "Not unlocked yet.";
			unlockLabel.text = isUnlocked ? "Unlocked" : "Locked";
			buttonLabel.text = isUnlocked ? "Lock" : "Unlock";
			
			SteamManager.Achievements.GetAchievementIcon(achievementApiName, (SteamImage image) =>
			{
				icon.texture = image.Texture;
			});
		}

		public void SetUnlockStatus(bool status)
		{
			isUnlocked = status;
			UpdateVisuals();
		}
	}
}