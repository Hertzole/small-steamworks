using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hertzole.SmallSteamworks.CompleteSample
{
	public class FileBox : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text nameLabel = default;
		[SerializeField]
		private TMP_Text sizeLabel = default;
		[SerializeField]
		private TMP_Text timestampLabel = default;
		[SerializeField]
		private Button loadButton = default;
		[SerializeField]
		private Button deleteButton = default;

		private SteamFile currentFile;

		public StorageSample Storage { get; set; }

		private void Awake()
		{
			loadButton.onClick.AddListener(LoadFile);
			deleteButton.onClick.AddListener(DeleteFile);
		}

		private void LoadFile()
		{
			Storage.LoadFile(currentFile);
		}

		private void DeleteFile()
		{
			Storage.DeleteFile(currentFile);
		}

		public void SetFile(SteamFile file)
		{
			currentFile = file;

			nameLabel.text = file.Name;
			sizeLabel.text = $"{file.Size / 1024f:F2} kb";
			timestampLabel.text = file.Timestamp.ToString(CultureInfo.InvariantCulture);
		}
	}
}