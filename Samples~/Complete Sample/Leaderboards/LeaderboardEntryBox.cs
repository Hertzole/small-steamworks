using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hertzole.SmallSteamworks.CompleteSample
{
	public class LeaderboardEntryBox : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text rankLabel = default;
		[SerializeField]
		private RawImage avatar = default;
		[SerializeField]
		private TMP_Text nameLabel = default;
		[SerializeField]
		private TMP_Text scoreLabel = default;

		private SteamImage avatarImage;

		private void OnDestroy()
		{
			if (avatarImage.IsValid)
			{
				avatarImage.Dispose();
			}
		}

		public void SetEntry(SteamLeaderboardEntry entry)
		{
			if (avatarImage.IsValid)
			{
				avatarImage.Dispose();
			}

			rankLabel.text = $"#{entry.GlobalRank}";
			scoreLabel.text = entry.Score.ToString(CultureInfo.InvariantCulture);
			nameLabel.text = entry.User.Name;

			SteamManager.Friends.GetAvatar(entry.User.SteamID, AvatarSize.Large, (image, _, _, _) =>
			{
				avatarImage = image;
				avatar.texture = image.Texture;
			});
		}
		
		public void Clear()
		{
			rankLabel.text = string.Empty;
			scoreLabel.text = string.Empty;
			nameLabel.text = string.Empty;
			avatar.texture = null;
			avatarImage.Dispose();
		}
	}
}