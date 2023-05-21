using Hertzole.SmallSteamworks;
using UnityEngine;

public class FriendLoader : MonoBehaviour
{
	[SerializeField]
	private FriendBox myBox = default;
	[SerializeField]
	private FriendBox box = default;

	private bool hasLoaded;

	private void Awake()
	{
		box.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (SteamManager.IsInitialized && !hasLoaded)
		{
			hasLoaded = true;

			myBox.SetFriend(SteamManager.Friends.Me);

			foreach (SteamUser friend in SteamManager.Friends.GetFriends())
			{
				FriendBox newBox = Instantiate(box, box.transform.parent);
				newBox.gameObject.SetActive(true);
				newBox.SetFriend(friend);
			}
		}
	}
}