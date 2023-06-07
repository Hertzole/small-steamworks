#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif
#nullable enable

using System;
using System.Collections.Generic;

namespace Hertzole.SmallSteamworks
{
	public partial interface ISteamApps : IDisposable
	{
		/// <summary>
		///     Checks if the current app is running in a cybercafe.
		/// </summary>
		bool IsCurrentAppInCybercafe { get; }

		/// <summary>
		///     Checks if the current app is installed.
		/// </summary>
		bool IsCurrentAppInstalled { get; }

		/// <summary>
		///     <para>Checks if the license owned by the user provides low violence depots.</para>
		///     <para>Low violence depots are useful for copies sold in countries that have content restrictions.</para>
		/// </summary>
		bool IsLowViolenceEnabled { get; }

		/// <summary>
		///     Checks if the active user is subscribed to the current App ID.
		/// </summary>
		/// <remarks>
		///     This will always return true if you're using Steam DRM or if you're using
		///     <see cref="ISteamSettings.RestartAppIfNecessary" />.
		/// </remarks>
		bool IsSubscribedToCurrentApp { get; }

		/// <summary>
		///     Checks if the active user is accessing the current app via a temporary Family Shared license owned by another user.
		/// </summary>
		/// <remarks>
		///     If you need to determine the steamID of the permanent owner of the license, use <see cref="AppOwner" />.
		/// </remarks>
		bool IsSubscribedFromFamilySharing { get; }

		/// <summary>
		///     Checks if the user is subscribed to the current appID through a free weekend.
		/// </summary>
		bool IsSubscribedFromFreeWeekend { get; }

		/// <summary>
		///     Checks if the user has a VAC ban on their account
		/// </summary>
		bool IsVACBanned { get; }

		/// <summary>
		///     <para>Gets the buildid of this app, may change at any time based on backend updates to the game.</para>
		///     <para>The current Build Id of this App. Defaults to 0 if you're not running a build downloaded from steam.</para>
		/// </summary>
		int AppBuildId { get; }

		/// <summary>
		///     Gets the Steam ID of the original owner of the current app. If it's different from the current user then it is
		///     borrowed.
		/// </summary>
		SteamID AppOwner { get; }

		/// <summary>
		///     <para>Gets the time of purchase of the current app in Unix epoch format (time since Jan 1st, 1970).</para>
		///     <para>This is useful for rewarding users based on their initial purchase date.</para>
		/// </summary>
		DateTime PurchaseTime { get; }

		/// <summary>
		///     Gets the name of the current beta branch. Will return <c>null</c> if the current branch is not a beta branch.
		/// </summary>
		string? CurrentBetaName { get; }

		/// <summary>
		///     Gets the install folder for the current app.
		///     <remarks>
		///         This works even if the application is not installed, based on where the game would be installed with the
		///         default Steam library location.
		///     </remarks>
		/// </summary>
		string InstallDirectory { get; }

		/// <summary>
		///     Gets an array of all the supported languages for this app.
		/// </summary>
		IReadOnlyList<string> AvailableGameLanguages { get; }

		/// <summary>
		///     <para>Gets the current language that the user has set.</para>
		///     <para>This falls back to the Steam UI language if the user hasn't explicitly picked a language for the title.</para>
		/// </summary>
		string GameLanguage { get; }

		/// <summary>
		///     Gets a list of all the installed depots for the current app.
		/// </summary>
		IReadOnlyList<DepotID> InstalledDepots { get; }

		/// <summary>
		///     Gets the command line if the game was launched via Steam URL.
		/// </summary>
		string? LaunchCommandLine { get; }

		/// <summary>
		///     Triggered after the current user gains ownership of DLC and that DLC is installed.
		/// </summary>
		event Action<AppID>? OnInstalledDLC;

		/// <summary>
		///     Sent every minute when a appID is owned via a timed trial.
		/// </summary>
		event TimeTrialStatusCallback? OnTimeTrialChanged;

		/// <summary>
		///     Checks if the active user is subscribed to a specified app.
		/// </summary>
		/// <param name="appId">The App ID to check.</param>
		/// <returns><c>true</c> if the active user is subscribed to the specified App ID; otherwise <c>false</c>.</returns>
		bool IsSubscribedToApp(AppID appId);

		/// <summary>
		///     <para>Gets the time of purchase of the specified app in Unix epoch format (time since Jan 1st, 1970).</para>
		///     <para>This is useful for rewarding users based on their initial purchase date.</para>
		/// </summary>
		/// <param name="appid">	The App ID to get the purchase time for.</param>
		/// <returns>The date time in Unix epoch format when the specified app was purchased.</returns>
		DateTime GetAppPurchaseTime(AppID appid);

		/// <summary>
		///     Gets the install folder for the specified app.
		///     <remarks>
		///         This works even if the application is not installed, based on where the game would be installed with the
		///         default Steam library location.
		///     </remarks>
		/// </summary>
		/// <param name="appId">The App ID to get the install directory for.</param>
		string GetAppInstallDirectory(AppID appId);

		/// <summary>
		///     <para>Allows you to force verify game content on next launch.</para>
		///     <para>
		///         If you detect the game is out-of-date (for example, by having the client detect a version mismatch with a
		///         server), you can call use MarkContentCorrupt to force a verify, show a message to the user, and then quit.
		///     </para>
		/// </summary>
		/// <param name="missingFilesOnly">If true, only scan for missing files and don't verify the checksum of each file.</param>
		void MarkContentCorrupt(bool missingFilesOnly);

		/// <summary>
		///     Gets all the optional DLC for the current app.
		/// </summary>
		/// <returns>An enumerable to enumerate over with all the DLC for the app.</returns>
		IEnumerable<SteamDLC> GetAllDLC();

		/// <summary>
		///     Checks if the user owns a specific DLC and if the DLC is installed
		/// </summary>
		/// <param name="dlcId">The App ID of the DLC to check.</param>
		/// <returns><c>true</c> if the user owns the DLC and it's currently installed; otherwise <c>false</c>.</returns>
		bool IsDLCInstalled(AppID dlcId);

		/// <summary>
		///     Allows you to install an optional DLC.
		/// </summary>
		/// <param name="dlcId">The DLC you want to install.</param>
		void InstallDLC(AppID dlcId);

		/// <summary>
		///     Allows you to uninstall an optional DLC.
		/// </summary>
		/// <param name="dlcId">The DLC you want to uninstall.</param>
		void UninstallDLC(AppID dlcId);

		/// <summary>
		///     Tries to get the download progress for a DLC.
		/// </summary>
		/// <param name="dlcId">The DLC id to get the download progress from.</param>
		/// <param name="bytesDownloaded">The number of bytes that have been downloaded.</param>
		/// <param name="bytesTotal">The number of bytes that needs to be downloaded.</param>
		/// <returns><c>true</c> if the specified DLC and is currently being downloaded; otherwise <c>false</c>.</returns>
		bool TryGetDLCDownloadProgress(AppID dlcId, out ulong bytesDownloaded, out ulong bytesTotal);

		/// <summary>
		///     Gets a list of all the installed depots for the specified app.
		/// </summary>
		/// <param name="appId">The App to list the depots for.</param>
		/// <returns>A list of all installed app depots.</returns>
		IReadOnlyList<DepotID> GetInstalledDepots(AppID appId);

		/// <summary>
		///     <para>
		///         Gets the associated launch parameter if the game is run via
		///         steam://run/{appId}/?param1=value1;param2=value2;param3=value3 etc.
		///     </para>
		///     <para>
		///         Parameter names starting with the character '@' are reserved for internal use and will always return an empty
		///         string. Parameter names starting with an underscore '_' are reserved for steam features -- they can be queried
		///         by the game, but it is advised that you not param names beginning with an underscore for your own features.
		///     </para>
		/// </summary>
		/// <param name="key">The launch key to test for. Ex: param1</param>
		/// <returns>The value associated with the key provided. Returns <c>null</c> if the specified key does not exist.</returns>
		string? GetLaunchQueryParam(string key);

		/// <summary>
		///     Asynchronously retrieves metadata details about a specific file in the depot manifest.
		/// </summary>
		/// <param name="fileName">	The absolute path and name to the file.</param>
		/// <param name="callback">Optional callback when the file details have been retrieved.</param>
		void GetFileDetails(string fileName, FileDetailsCallback? callback = null);
	}
}