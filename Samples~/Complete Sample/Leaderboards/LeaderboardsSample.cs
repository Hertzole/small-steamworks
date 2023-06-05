using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Hertzole.SmallSteamworks.CompleteSample
{
	public class LeaderboardsSample : BaseSample
	{
		[SerializeField]
		private TMP_InputField nameField = default;
		[SerializeField]
		private TMP_Dropdown sortTypeDropdown = default;
		[SerializeField]
		private TMP_Dropdown displayTypeDropdown = default;
		[SerializeField]
		private Button findOrCreateButton = default;
		
		[Space]
		[SerializeField] 
		private TMP_InputField maxEntriesField = default;
		[SerializeField] 
		private TMP_InputField offsetField = default;
		[SerializeField]
		private Button getScoresButton = default;
		[SerializeField] 
		private Button getFriendScoresButton = default;
		[SerializeField] 
		private TMP_InputField minRangeField = default;
		[SerializeField]
		private TMP_InputField maxRangeField = default;
		[SerializeField]
		private Button getScoresInRangeButton = default;
		
		[Space]
		[SerializeField] 
		private TMP_InputField scoreToSetField = default;
		[SerializeField] 
		private TMP_Dropdown uploadScoreDropdown = default;
		[SerializeField] 
		private Button submitScoreButton = default;
		
		[Space]
		[SerializeField]
		private TMP_Text statusLabel = default;
		[SerializeField]
		private RectTransform entriesContainer = default;
		[SerializeField]
		private LeaderboardEntryBox entryBoxPrefab = default;

		private static readonly LeaderboardDisplayType[] displayTypes =
		{
			LeaderboardDisplayType.Numeric,
			LeaderboardDisplayType.TimeSeconds,
			LeaderboardDisplayType.TimeMilliSeconds
		};

		private static readonly LeaderboardSortMethod[] sortMethods =
		{
			LeaderboardSortMethod.Ascending,
			LeaderboardSortMethod.Descending
		};
		
		private static readonly LeaderboardUploadScoreMethod[] uploadScoreMethods =
		{
			LeaderboardUploadScoreMethod.KeepBest,
			LeaderboardUploadScoreMethod.ForceUpdate
		};

		private readonly List<LeaderboardEntryBox> entryBoxes = new List<LeaderboardEntryBox>();

		private ObjectPool<LeaderboardEntryBox> entryBoxPool;

		private SteamLeaderboard? currentLeaderboard;

		private void Awake()
		{
			UpdateStatus();

			sortTypeDropdown.ClearOptions();
			displayTypeDropdown.ClearOptions();

			for (int i = 0; i < sortMethods.Length; i++)
			{
				sortTypeDropdown.options.Add(new TMP_Dropdown.OptionData(sortMethods[i].ToString()));
			}

			for (int i = 0; i < displayTypes.Length; i++)
			{
				displayTypeDropdown.options.Add(new TMP_Dropdown.OptionData(displayTypes[i].ToString()));
			}

			sortTypeDropdown.RefreshShownValue();
			displayTypeDropdown.RefreshShownValue();

			findOrCreateButton.onClick.AddListener(ClickFindOrCreateLeaderboard);

			nameField.onValueChanged.AddListener(s => ValidateButtons());

			entryBoxPool = new ObjectPool<LeaderboardEntryBox>(
				() => Instantiate(entryBoxPrefab, entriesContainer), // Create
				b =>
				{
					b.gameObject.SetActive(true);
					b.transform.SetAsLastSibling();
				}, // On Get
				b => b.gameObject.SetActive(false)); // On Release
			
			getScoresButton.onClick.AddListener(ClickGetScores);

			maxEntriesField.onValueChanged.AddListener(s => ValidateButtons());
			offsetField.onValueChanged.AddListener(s => ValidateButtons());
			
			getFriendScoresButton.onClick.AddListener(ClickGetFriendScores);
			
			minRangeField.onValueChanged.AddListener(s => ValidateButtons());
			maxRangeField.onValueChanged.AddListener(s => ValidateButtons());
			
			getScoresInRangeButton.onClick.AddListener(ClickGetScoresInRange);
			
			scoreToSetField.onValueChanged.AddListener(s => ValidateButtons());
			
			uploadScoreDropdown.ClearOptions();
			
			for (int i = 0; i < uploadScoreMethods.Length; i++)
			{
				uploadScoreDropdown.options.Add(new TMP_Dropdown.OptionData(uploadScoreMethods[i].ToString()));
			}
			
			uploadScoreDropdown.RefreshShownValue();
			
			submitScoreButton.onClick.AddListener(ClickSubmitScore);
			
			ValidateButtons();
		}

		private void ValidateButtons()
		{
			findOrCreateButton.interactable = !string.IsNullOrWhiteSpace(nameField.text);
			getScoresButton.interactable = currentLeaderboard.HasValue && int.TryParse(maxEntriesField.text, out _) && int.TryParse(offsetField.text, out _);
			getFriendScoresButton.interactable = currentLeaderboard.HasValue;
			getScoresInRangeButton.interactable = currentLeaderboard.HasValue && int.TryParse(minRangeField.text, out _) && int.TryParse(maxRangeField.text, out _);
			submitScoreButton.interactable = currentLeaderboard.HasValue && int.TryParse(scoreToSetField.text, out _);
		}

		private void UpdateStatus()
		{
			getScoresButton.interactable = currentLeaderboard.HasValue;

			if (currentLeaderboard.HasValue)
			{
				statusLabel.text = $"Current leaderboard: {currentLeaderboard.Value.Name}";
				return;
			}

			statusLabel.text = "No leaderboard loaded";
		}

		private void ClickFindOrCreateLeaderboard()
		{
			currentLeaderboard = null;
			statusLabel.text = "Finding or creating leaderboard...";
			
			ClearEntries();

			SteamManager.Leaderboards.FindOrCreateLeaderboard(nameField.text, sortMethods[sortTypeDropdown.value], displayTypes[displayTypeDropdown.value],
				(success, leaderboard) =>
				{
					if (!success)
					{
						statusLabel.text = "Failed to find or create leaderboard.";
						return;
					}

					currentLeaderboard = leaderboard;
					UpdateStatus();
					ValidateButtons();
				});
		}

		private void ClickGetScores()
		{
			if (currentLeaderboard == null)
			{
				return;
			}

			statusLabel.text = "Getting scores...";

			if (!int.TryParse(maxEntriesField.text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int maxEntries))
			{
				maxEntries = 100;
			}
			
			if (!int.TryParse(offsetField.text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int offset))
			{
				offset = 0;
			}
			
			ClearEntries();

			SteamManager.Leaderboards.GetScores(currentLeaderboard.Value, maxEntries, offset, OnGetScores);
		}
		
		private void ClickGetFriendScores()
		{
			if (currentLeaderboard == null)
			{
				return;
			}

			statusLabel.text = "Getting scores...";

			ClearEntries();

			SteamManager.Leaderboards.GetScoresFromFriends(currentLeaderboard.Value, OnGetScores);
		}
		
		private void ClickGetScoresInRange()
		{
			if (currentLeaderboard == null)
			{
				return;
			}

			statusLabel.text = "Getting scores...";

			if (!int.TryParse(minRangeField.text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int minRange))
			{
				minRange = 10;
			}
			
			if (!int.TryParse(maxRangeField.text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int maxRange))
			{
				maxRange = 10;
			}
			
			ClearEntries();

			SteamManager.Leaderboards.GetScoresAroundUser(currentLeaderboard.Value, minRange, maxRange, OnGetScores);
		}

		private void OnGetScores(bool success, SteamLeaderboard leaderboard, SteamLeaderboardEntry[] entries)
		{
			if (!success)
			{
				return;
			}

			currentLeaderboard = leaderboard;

			for (int i = 0; i < entries.Length; i++)
			{
				LeaderboardEntryBox box = entryBoxPool.Get();
				box.SetEntry(entries[i]);
				entryBoxes.Add(box);
			}

			statusLabel.text = string.Empty;
		}
		
		private void ClickSubmitScore()
		{
			if (currentLeaderboard == null)
			{
				return;
			}
			
			if (!int.TryParse(scoreToSetField.text, out int score))
			{
				score = 0;
			}
			
			SteamManager.Leaderboards.UploadScore(currentLeaderboard.Value, uploadScoreMethods[uploadScoreDropdown.value], score, null,
				(success, leaderboard, submittedScore, changed, rank, previousRank) =>
				{
					if (!success)
					{
						return;
					}
					
					Debug.Log($"Score submitted! Score was changed: {changed}, rank: {rank}, previous rank: {previousRank}, score: {submittedScore}");
				});
		}

		private void ClearEntries()
		{
			for (int i = 0; i < entryBoxes.Count; i++)
			{
				entryBoxPool.Release(entryBoxes[i]);
			}
				
			entryBoxes.Clear();
		}
	}
}