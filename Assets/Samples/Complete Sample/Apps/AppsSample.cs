using System.Globalization;
using TMPro;
using UnityEngine;

namespace Hertzole.SmallSteamworks.CompleteSample
{
	public class AppsSample : BaseSample
	{
		[SerializeField]
		private TMP_Text appOwner = default;
		[SerializeField]
		private TMP_Text buildId = default;
		[SerializeField]
		private TMP_Text installDirectory = default;
		[SerializeField]
		private TMP_Text currentBeta = default;
		[SerializeField]
		private TMP_Text purchaseTime = default;
		[SerializeField]
		private TMP_Text cyberCafe = default;
		[SerializeField]
		private TMP_Text currentAppInstalled = default;
		[SerializeField]
		private TMP_Text lowViolence = default;
		[SerializeField]
		private TMP_Text subscribedToCurrentApp = default;
		[SerializeField]
		private TMP_Text subscribedFromFamilySharing = default;
		[SerializeField]
		private TMP_Text subscribedFromFreeWeekend = default;
		[SerializeField]
		private TMP_Text isVacBanned = default;
		[SerializeField]
		private TMP_Text currentLanguage = default;
		[SerializeField]
		private TMP_Text availableLanguages = default;
		[SerializeField]
		private TMP_Text installedDepots = default;
		[SerializeField]
		private TMP_Text launchCommandLine = default;

		[Space]
		
		[SerializeField] 
		private DlcBox dlcBoxPrefab = default;
		[SerializeField] 
		private RectTransform dlcContent = default;

		private bool hasDLC = false;
		
		private void Awake()
		{
			appOwner.text = SteamManager.Apps.AppOwner.ToString();
			buildId.text = SteamManager.Apps.AppBuildId.ToString();
			installDirectory.text = SteamManager.Apps.InstallDirectory;
			currentBeta.text = string.IsNullOrEmpty(SteamManager.Apps.CurrentBetaName) ? "<i>none</i>" : SteamManager.Apps.CurrentBetaName;
			purchaseTime.text = SteamManager.Apps.PurchaseTime.ToString(CultureInfo.InvariantCulture);
			cyberCafe.text = SteamManager.Apps.IsCurrentAppInCybercafe.ToString();
			currentAppInstalled.text = SteamManager.Apps.IsCurrentAppInstalled.ToString();
			lowViolence.text = SteamManager.Apps.IsLowViolenceEnabled.ToString();
			subscribedToCurrentApp.text = SteamManager.Apps.IsSubscribedToCurrentApp.ToString();
			subscribedFromFamilySharing.text = SteamManager.Apps.IsSubscribedFromFamilySharing.ToString();
			subscribedFromFreeWeekend.text = SteamManager.Apps.IsSubscribedFromFreeWeekend.ToString();
			isVacBanned.text = SteamManager.Apps.IsVACBanned.ToString();
			currentLanguage.text = SteamManager.Apps.GameLanguage;
			availableLanguages.text = string.Join(", ", SteamManager.Apps.AvailableGameLanguages);
			installedDepots.text = SteamManager.Apps.InstalledDepots.Count == 0 ? "<i>none</i>" : string.Join(", ", SteamManager.Apps.InstalledDepots);
			launchCommandLine.text = string.IsNullOrEmpty(SteamManager.Apps.LaunchCommandLine) ? "<i>none</i>" : SteamManager.Apps.LaunchCommandLine;
		}

		public override void ShowSample()
		{
			if (!hasDLC)
			{
				hasDLC = true;

				foreach (SteamDLC dlc in SteamManager.Apps.GetAllDLC())
				{
					DlcBox box = Instantiate(dlcBoxPrefab, dlcContent);
					box.SetDLC(dlc);
				}
			}
		}
	}
}