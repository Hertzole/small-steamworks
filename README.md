# Small Steamworks

[![Documentation](https://img.shields.io/badge/-documentation-informational)](https://hertzole.github.io/small-steamworks/)
[![openupm](https://img.shields.io/npm/v/se.hertzole.small-steamworks?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/se.hertzole.small-steamworks/)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=small-steamworks&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=small-steamworks)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=small-steamworks&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=small-steamworks)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=small-steamworks&metric=bugs)](https://sonarcloud.io/summary/new_code?id=small-steamworks)

Small Steamworks is an easy-to-use wrapper for the Steamworks API, based on [Steamworks.NET](https://github.com/rlabrecque/Steamworks.NET) for Unity. That means all assets that use Steamworks.NET works out of the box!

It's designed to be easy to use and to make it easy to add Steamworks support to your game using callbacks and normal C# `async`.

## 📦 Installation

See the [installation guide](https://hertzole.github.io/small-steamworks/installation) for more information.

## ✨ Features
- Supports fast enter play mode
- Supports all assets that use Steamworks.NET
- Fully supports `DISABLESTEAMWORKS` compilation symbol
- Has both callbacks and `async`/`await` support
- No need to initialize and shutdown Steamworks manually

See the [getting started guide](https://hertzole.github.io/small-steamworks/getting-started) for more information on how to use Small Steamworks.

## 💥 Small Steamworks vs Steamworks.NET

Steamworks.NET stays very true to the original Steamworks API. This means that it's very low-level and not very easy to use. Steamworks' way of working with callbacks can be a bit cumbersome for C# developers. The naming can also be a bit confusing.  

Small Steamworks aims to wrap most of the Steamworks API in a more C#-friendly way. It uses normal C# delegates instead of the Steamworks way of using callbacks. It also has `async`/`await` for those that prefer using tasks! Small Steamworks also renames some of the functions to a more understandable name. It also moves some of the functions to more logical places. For example, achievements and leaderboards are together in the `SteamStats` class. Now they are in their own achievement and leaderboards classes.

The biggest downside to Small Steamworks is that the name changes may make it harder to use the official Steamworks API reference, but for that, you can use [Small Steamworks' documentation](https://hertzole.github.io/small-steamworks/).

Small Steamworks also does not support all of the Steamworks API yet. It's a work in progress and I'm adding more and more as I need it and when I have the time. If you need something that's not supported, feel free to open an issue or a pull request! You can see what's supported [here](https://hertzole.se/small-steamworks/#development-progress).

## 🛠 Small Steamworks vs Steamworks.NET Examples

### Unlocking an achievement

Steamworks.NET:
```csharp
bool succes = SteamUserStats.SetAchievement("ACH_WIN_GAME");
if(success)
{
    SteamUserStats.StoreStats();
}
```

Small Steamworks:
```csharp
SteamManager.Achievements.UnlockAchievement("ACH_WIN_GAME", storeStats: true);
```

### Uploading a leaderboard score

Steamworks.NET:
```csharp
CallResult<LeaderboardScoreUploaded_t> callResult;

...

SteamLeaderboard_t leaderboard;
int score = 100;
ELeaderboardUploadScoreMethod uploadMethod = ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest;
var call = SteamUserStats.UploadLeaderboardScore(leaderboard, uploadMethod, score, null, 0);
callResult.Set(call, (t, failure) => {})
```

Small Steamworks:
```csharp
SteamLeaderboard leaderboard;
int score = 100;
LeaderboardUploadScoreMethod uploadMethod = LeaderboardUploadScoreMethod.KeepBest;
SteamManager.Leaderboards.UploadScore(leaderboard, uploadMethod, score, null, (success, leaderboard, score, scoreChanged, newRank, previousRank) => {});
```

## 📃 License

Small Steamworks is licensed under the [MIT license](https://github.com/Hertzole/small-steamworks/blob/master/LICENSE.md). You can basically do anything with this. I'm just not liable if something breaks.

## ♥ Support

If you like this project, please consider supporting me on GitHub by sponsoring me or by buying me a coffee!

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/I2I4IHAK)