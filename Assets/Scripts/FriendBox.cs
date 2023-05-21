using Hertzole.SmallSteamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendBox : MonoBehaviour
{
	[SerializeField]
	private RawImage icon = default;
	[SerializeField]
	private TMP_Text friendName = default;
	[SerializeField]
	private TMP_Text statusText = default;

	public void SetFriend(SteamUser user)
	{
		SteamManager.Friends.RequestUserInformation(user.SteamID, true, steamUser =>
		{
			friendName.text = steamUser.Name;
			statusText.text = steamUser.IsOnline ? "Online" : "Offline";

			if (!steamUser.IsOnline)
			{
				icon.color = new Color(1, 1, 1, 0.1f);
			}
			
			SteamManager.Friends.GetAvatar(steamUser.SteamID, AvatarSize.Large, (image, id, width, height) => { icon.texture = image.Texture; });
		});
	}
}