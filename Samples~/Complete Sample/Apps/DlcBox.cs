using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hertzole.SmallSteamworks.CompleteSample
{
	public class DlcBox : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text nameLabel = default;
		[SerializeField]
		private TMP_Text storeAvailability = default;
		[SerializeField]
		private TMP_Text downloadLabel = default;
		[SerializeField]
		private Slider downloadProgressBar = default;
		[SerializeField]
		private Button installUninstallButton = default;
		[SerializeField]
		private TMP_Text installUninstallLabel = default;

		private bool isInstalled;
		private bool isDownloading;
		private bool hasCanceled;

		private AppID appID;

		private void Awake()
		{
			installUninstallButton.onClick.AddListener(OnInstallUninstallClicked);
		}

		private void Update()
		{
			isDownloading = SteamManager.Apps.TryGetDLCDownloadProgress(appID, out ulong current, out ulong total);

			if (!isDownloading && hasCanceled)
			{
				installUninstallLabel.text = "Install";
				
				downloadLabel.gameObject.SetActive(false);
				downloadProgressBar.gameObject.SetActive(false);
				installUninstallButton.interactable = true;
			}
			else if (current > 0 && current == total)
			{
				installUninstallButton.interactable = true;
				downloadLabel.gameObject.SetActive(false);
				downloadProgressBar.gameObject.SetActive(false);
				installUninstallLabel.text = "Uninstall";
				isInstalled = true;
			}
			else if (isDownloading)
			{
				UpdateDownloadProgress(current, total);
			}
		}

		private void OnDestroy()
		{
			installUninstallButton.onClick.RemoveListener(OnInstallUninstallClicked);
		}

		public void SetDLC(SteamDLC dlc)
		{
			appID = dlc.AppID;
			nameLabel.text = $"{dlc.Name} ({dlc.AppID})";
			storeAvailability.text = dlc.IsAvailableInStore ? "Available In Store" : "Not available In Store";
			downloadLabel.gameObject.SetActive(false);
			downloadProgressBar.gameObject.SetActive(false);
			downloadProgressBar.value = 0;
			installUninstallLabel.text = dlc.IsInstalled ? "Uninstall" : "Install";

			isInstalled = dlc.IsInstalled;
		}

		private void OnInstallUninstallClicked()
		{
			if (isDownloading && !hasCanceled)
			{
				SteamManager.Apps.UninstallDLC(appID);
				isInstalled = false;
				hasCanceled = true;
				installUninstallButton.interactable = false;
				return;
			}
			
			if (isInstalled)
			{
				SteamManager.Apps.UninstallDLC(appID);
				isInstalled = false;
				installUninstallLabel.text = "Install";
				
				downloadLabel.gameObject.SetActive(false);
				downloadProgressBar.gameObject.SetActive(false);
			}
			else
			{
				installUninstallLabel.text = "Cancel";
				downloadLabel.gameObject.SetActive(true);
				downloadProgressBar.gameObject.SetActive(true);
				downloadLabel.text = "Starting download...";
				downloadProgressBar.value = 0;
				SteamManager.Apps.InstallDLC(appID);
			}
		}

		private void UpdateDownloadProgress(ulong current, ulong total)
		{
			installUninstallLabel.text = "Cancel";
			downloadLabel.gameObject.SetActive(true);
			downloadProgressBar.gameObject.SetActive(true);
			downloadLabel.text = $"{current} bytes / {total} bytes";
			downloadProgressBar.value = (float) current / total;
		}
	}
}