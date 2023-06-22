---
sidebar_position: 21
title: 🖥 Apps
---

Use the app API to manage DLCs, get information about the current app, and more.

## Properties

### Is Current App In Cybercafe

Checks if the current app is running in cybercafe mode.

```cs
bool isInCybercafe = SteamManager.Apps.IsCurrentAppInCybercafe;
```

### Is Current App Installed

Checks if the current app is installed.

```cs
bool isInstalled = SteamManager.Apps.IsCurrentAppInstalled;
```

### Is Low Violence Enabled

Checks if low violence mode is enabled for the current app.

```cs
bool isLowViolence = SteamManager.Apps.IsLowViolenceEnabled;
```

### Is Subscribed To Current App

Checks if the current user is subscribed to the current app.

This will always return `true` if you're using Steam DRM or Restart App If Necessary.

```cs
bool isSubscribed = SteamManager.Apps.IsSubscribedToCurrentApp;
```

### Is Subscribed From Family Sharing

Checks if the active user is accessing the current app via a temporary Family Shared license owned by another user.

If you need to determine the steamID of the permanent owner of the license, use [app owner](#app-owner).

```cs
bool isSubscribed = SteamManager.Apps.IsSubscribedFromFamilySharing;
```

### Is Subscribed From Free Weekend

Checks if the current user is playing a free weekend version of the current app.

```cs
bool isSubscribed = SteamManager.Apps.IsSubscribedFromFreeWeekend;
```

### Is VAC Banned

Checks if the current user is VAC banned.

```cs
bool isVACBanned = SteamManager.Apps.IsVACBanned;
```

### App Build ID

Gets the build id of this app. This may change at any time based on backend updates to the game. Defaults to 0 if you're not running a build downloaded from Steam.

```cs
int buildId = SteamManager.Apps.AppBuildId;
```

### App Owner

Gets the Steam ID of the original owner of the current app. If it's different from the current user then it is borrowed.

```cs
SteamID steamId = SteamManager.Apps.AppOwner;
```

### Purchase Time

Gets the time of purchase of the current app in Unix epoch format (time since Jan 1st, 1970). This is useful for rewarding users based on their initial purchase date.

```cs
DateTime purchaseTime = SteamManager.Apps.PurchaseTime;
```

### Current Beta Name

Gets the name of the current beta branch. Will return `null `if the current branch is not a beta branch.

```cs
string? betaName = SteamManager.Apps.CurrentBetaName;
```

### Install Directory

Gets the install directory for the current app.

This works even if the application is not installed, based on where the game would be installed with the default Steam library location.

```cs
string installDir = SteamManager.Apps.InstallDirectory;
```

### Available Game Languages

Gets a list of all available languages for the current app.

The languages are returned as lower case English names (`english`, `german`, `french`, etc). For a full list of languages and their API language code, see the [Steamworks documentation](https://partner.steamgames.com/doc/store/localization/languages).

```cs
IReadOnlyList<string> languages = SteamManager.Apps.AvailableGameLanguages;
```

### Current Game Language

Gets the current language that the app is running in.

The language is returned as a lower case English name (`english`, `german`, `french`, etc). For a full list of languages and their API language code, see the [Steamworks documentation](https://partner.steamgames.com/doc/store/localization/languages).

```cs
string language = SteamManager.Apps.CurrentGameLanguage;
```

### Installed Depots

Gets a list of all installed depots for the current app.

```cs
IReadOnlyList<DepotID> depots = SteamManager.Apps.InstalledDepots;
```

### Launch Command Line

Gets the command line if the game was launched via Steam URL. May be `null` if the game was not launched this way.

```cs
string? commandLine = SteamManager.Apps.LaunchCommandLine;
```

## Events

### On Installed DLC

Triggered after the current user gains ownership of DLC and that DLC is installed.

```cs
SteamManager.Apps.OnInstalledDLC += (dlcId) => 
{
    Console.WriteLine($"DLC {dlcId} installed!");
};
```

### On Time Trial Changed

Sent every minute when the app is owned via a timed trial.

```cs
SteamManager.Apps.OnTimeTrialChanged += (appId, isOffline, totalSeconds, secondsPlayed, secondsRemaining) => 
{
    Console.WriteLine($"The app id is {appId}");
    Console.WriteLine($"Is in offline mode: {isOffline}");
    Console.WriteLine($"Total seconds in time trial: {totalSeconds}");
    Console.WriteLine($"Seconds played in time trial: {secondsPlayed}");
    Console.WriteLine($"Seconds remaining of time trial: {secondsRemaining}");
};
```

## Methods

### Is Subscribed To App

Checks if the active user is subscribed to a specified app.

```cs
AppID appId = new AppID(480);
bool isSubscribed = SteamManager.Apps.IsSubscribedToApp(appId);
```

### Get App Purchase Time

Gets the time of purchase of the specified app in Unix epoch format (time since Jan 1st, 1970).

```cs
AppID appId = new AppID(480);
DateTime purchaseTime = SteamManager.Apps.GetAppPurchaseTime(appId);
```

### Get App Install Directory

Gets the install folder for the specified app.

This works even if the application is not installed, based on where the game would be installed with the default Steam library location.

```cs
AppID appId = new AppID(480);
string installDir = SteamManager.Apps.GetAppInstallDirectory(appId);
```

### Mark Content Corrupt

Marks content as corrupt. This will cause the Steam client to validate the game's files and re-download any corrupt files.

```cs
// If true, only scan for missing files and don't verify the checksum of each file.
bool missingFilesOnly = false;
SteamManager.Apps.MarkContentCorrupt(missingFilesOnly);
```

### Get All DLC

Gets a list of all installed DLC for the current app.

```cs
foreach (SteamDLC dlc in SteamManager.Apps.GetAllDLC())
{
    Console.WriteLine($"Name: {dlc.Name}");
    Console.WriteLine($"App ID: {dlc.AppID}");
    Console.WriteLine($"Available: {dlc.IsAvailableOnSteamStore}");
    Console.WriteLine($"Installed: {dlc.IsInstalled}");
}
```

### Is DLC Installed

Checks if the user owns a specific DLC and if the DLC is installed

```cs
AppID dlcId = new AppID(480);
bool isInstalled = SteamManager.Apps.IsDLCInstalled(dlcId);
```

### Install DLC

Installs a DLC and triggers the [On Installed DLC](#on-installed-dlc) event.

```cs
AppID dlcId = new AppID(480);
SteamManager.Apps.InstallDLC(dlcId);
```

### Uninstall DLC

Uninstalls a DLC.

```cs
AppID dlcId = new AppID(480);
SteamManager.Apps.UninstallDLC(dlcId);
```

### Try Get DLC Download Progress

Tries to get the download progress for a DLC.

Only returns `true` if the specified DLC is currently being downloaded.

```cs
AppID dlcId = new AppID(480);
if (SteamManager.Apps.TryGetDLCDownloadProgress(dlcId, out ulong bytesDownloaded, out ulong bytesTotal))
{
    Console.WriteLine($"DLC {dlcId} has downloaded {bytesDownloaded} of {bytesTotal} bytes");
}
```

### Get Installed Depots

Gets a list of all installed depots for the specified app.

```cs
AppID appId = new AppID(480);
IReadOnlyList<DepotID> depots = SteamManager.Apps.GetInstalledDepots(appId);
```

### Get Launch Query Parameter

Gets the associated launch parameter if the game is run via `steam://run/<appId>/?param1=value1;param2=value2;param3=value3`.

Parameter names starting with the character '@' are reserved for internal use and will always return an empty string. Parameter names starting with an underscore '_' are reserved for Steam features -- they can be queried by the game, but it is advised that you begin parameter names beginning with an underscore for your own features.

Returns `null` if the parameter does not exist.

```cs
string key = "_myParam"
string? value = SteamManager.Apps.GetLaunchQueryParam(key);
```

### Get File Details

Asynchronously retrieves metadata details about a specific file in the depot manifest.

```cs
string fileName = "myFile.txt";
SteamManager.Apps.GetFileDetails(fileName, (fileExists, fileSize, fileSHA) => 
{
    if (fileExists)
    {
        Console.WriteLine($"File {fileName} exists!");
        Console.WriteLine($"File size: {fileSize}");
        Console.WriteLine($"File SHA length: {fileSHA.Length}"); // SHA is a byte array
    }
});
```

### Get File Details Async

Asynchronously retrieves metadata details about a specific file in the depot manifest.

```cs
string fileName = "myFile.txt";
FileDetailsResponse response = await SteamManager.Apps.GetFileDetailsAsync(fileName);

if (response.FileExists)
{
    Console.WriteLine($"File {fileName} exists!");
    Console.WriteLine($"File size: {response.FileSize}");
    Console.WriteLine($"File SHA length: {response.FileSHA.Length}"); // SHA is a byte array
}
```