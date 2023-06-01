using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Hertzole.SmallSteamworks.CompleteSample
{
	public class StorageSample : BaseSample
	{
		[SerializeField]
		private TMP_InputField fileNameField = default;
		[SerializeField]
		private TMP_InputField contentField = default;
		[SerializeField]
		private Button uploadButton = default;

		[Space]
		[SerializeField]
		private TMP_Text totalBytesLabel = default;
		[SerializeField]
		private TMP_Text availableBytesLabel = default;
		[SerializeField]
		private TMP_Text usedBytesLabel = default;

		[Space]
		[SerializeField]
		private FileBox fileBoxPrefab = default;
		[SerializeField]
		private RectTransform filesContainer = default;

		private bool isUploading = false;
		private bool hasLoadedFiles = false;

		private readonly List<FileBox> fileBoxes = new List<FileBox>();

		private ObjectPool<FileBox> fileBoxesPool;

		private void Awake()
		{
			fileNameField.onValueChanged.AddListener(s => ValidateButton());
			contentField.onValueChanged.AddListener(s => ValidateButton());

			uploadButton.onClick.AddListener(UploadFile);

			fileBoxesPool = new ObjectPool<FileBox>(
				() =>
				{
					FileBox box = Instantiate(fileBoxPrefab, filesContainer);
					box.Storage = this;
					return box;
				}, // Create
				b =>
				{
					b.gameObject.SetActive(true);
					b.transform.SetAsLastSibling();
				}, // On Get
				b => b.gameObject.SetActive(false)); // On Release

			ValidateButton();
		}

		public override void ShowSample()
		{
			if (!hasLoadedFiles)
			{
				hasLoadedFiles = true;
				LoadFiles();
			}

			UpdateQuota();
		}

		private void UploadFile()
		{
			isUploading = true;
			ValidateButton();

			SteamManager.Storage.WriteFile(fileNameField.text.Trim(), Encoding.UTF8.GetBytes(contentField.text.Trim()), result =>
			{
				isUploading = false;
				ValidateButton();

				if (result == FileWrittenResult.QuotaExceeded)
				{
					Debug.LogWarning("Quota exceeded!");
				}

				if (result != FileWrittenResult.Success)
				{
					isUploading = false;
					ValidateButton();
					return;
				}

				UpdateQuota();
				LoadFiles();
			});
		}

		private void ValidateButton()
		{
			uploadButton.interactable = !isUploading && !string.IsNullOrWhiteSpace(fileNameField.text) && !string.IsNullOrWhiteSpace(contentField.text);
		}

		private void UpdateQuota()
		{
			QuotaResponse response = SteamManager.Storage.GetQuota();
			totalBytesLabel.text = response.TotalBytes.ToString("G", CultureInfo.InvariantCulture);
			availableBytesLabel.text = response.AvailableBytes.ToString("G", CultureInfo.InvariantCulture);
			usedBytesLabel.text = response.BytesUsed.ToString("G", CultureInfo.InvariantCulture);
		}

		private void LoadFiles()
		{
			for (int i = 0; i < fileBoxes.Count; i++)
			{
				fileBoxesPool.Release(fileBoxes[i]);
			}

			fileBoxes.Clear();

			foreach (SteamFile file in SteamManager.Storage.GetAllFiles())
			{
				FileBox box = fileBoxesPool.Get();
				box.SetFile(file);
				fileBoxes.Add(box);
			}
		}

		public void LoadFile(SteamFile file)
		{
			SteamManager.Storage.ReadFile(file.Name, (success, data) =>
			{
				if (!success)
				{
					return;
				}

				fileNameField.text = file.Name;
				contentField.text = Encoding.UTF8.GetString(data);
			});
		}

		public void DeleteFile(SteamFile file)
		{
			SteamManager.Storage.DeleteFile(file.Name);
			UpdateQuota();
			LoadFiles();
		}
	}
}