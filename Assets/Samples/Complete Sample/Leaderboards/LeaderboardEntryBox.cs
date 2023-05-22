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

		private SteamImage? avatarImage;

		private void OnDestroy()
		{
			if (avatarImage != null)
			{
				avatarImage.Value.Dispose();
				avatarImage = null;
			}
		}

		public void SetEntry(SteamLeaderboardEntry entry)
		{
			if (avatarImage != null)
			{
				avatarImage.Value.Dispose();
				avatarImage = null;
			}

			rankLabel.text = $"#{entry.GlobalRank}";
			scoreLabel.text = entry.Score.ToString(CultureInfo.InvariantCulture);
			nameLabel.text = entry.UserID.Name;

			SteamManager.Friends.GetAvatar(entry.UserID.SteamID, AvatarSize.Large, (image, _, _, _) =>
			{
				avatarImage = image;
				avatar.texture = image.Texture;
			});
		}
	}
}