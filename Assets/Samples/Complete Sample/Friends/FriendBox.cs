using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hertzole.SmallSteamworks.CompleteSample
{
	public class FriendBox : MonoBehaviour
	{
		[SerializeField]
		private bool isMe = default;

		[Space]
		[SerializeField]
		private RawImage icon = default;
		[SerializeField]
		private TMP_Text nameLabel = default;
		[SerializeField]
		private TMP_Text statusLabel = default;

		private SteamImage avatarImage;

		private void Start()
		{
			if (isMe)
			{
				SetUser(SteamManager.Friends.Me);
			}
		}

		private void OnDestroy()
		{
			ClearAvatar();
		}

		public void SetUser(SteamUser user)
		{
			nameLabel.text = user.Name;

			if (statusLabel != null)
			{
				statusLabel.text = user.IsOnline ? "Online" : "Offline";
			}

			icon.color = user.IsOnline ? Color.white : new Color(1, 1, 1, 0.1f);

			ClearAvatar();

			SteamManager.Friends.GetAvatar(user.SteamID, AvatarSize.Large, (image, id, width, height) =>
			{
				avatarImage = image;
				icon.texture = image.Texture;
			});
		}

		private void ClearAvatar()
		{
			if (avatarImage.IsValid)
			{
				avatarImage.Dispose();
			}
		}
	}
}