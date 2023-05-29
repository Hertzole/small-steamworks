using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Hertzole.SmallSteamworks.CompleteSample
{
	public class FriendsSample : BaseSample
	{
		[SerializeField]
		private FriendBox friendBoxPrefab = default;
		[SerializeField]
		private RectTransform friendsContainer = default;

		private bool isGettingFriends;

		private readonly List<FriendBox> entryBoxes = new List<FriendBox>();

		private ObjectPool<FriendBox> entryBoxPool;

		private void Awake()
		{
			entryBoxPool = new ObjectPool<FriendBox>(
				() => Instantiate(friendBoxPrefab, friendsContainer), // Create
				b =>
				{
					b.gameObject.SetActive(true);
					b.transform.SetAsLastSibling();
				}, // On Get
				b => b.gameObject.SetActive(false)); // On Release
		}

		public override void ShowSample()
		{
			if (isGettingFriends)
			{
				return;
			}

			isGettingFriends = true;

			LoadFriends();
		}

		private void LoadFriends()
		{
			for (int i = 0; i < entryBoxes.Count; i++)
			{
				entryBoxPool.Release(entryBoxes[i]);
			}

			entryBoxes.Clear();

			foreach (SteamUser friend in SteamManager.Friends.GetFriends())
			{
				FriendBox box = entryBoxPool.Get();
				box.SetUser(friend);
				entryBoxes.Add(box);
			}
		}
	}
}