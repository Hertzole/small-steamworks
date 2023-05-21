---
sidebar_position: 20
title: Achievements
---

# Achievements

Achievements are a great way to encourage and reward your players with milestones within your game. They can be used to track progress, unlock content, or just to show off.

This page will teach you how you can use Small Steamworks to interact with your game's achievements.

## Unlocking Achievements

```cs
SteamManager.Achievements.UnlockAchievement("API_NAME");
```

:::info
Each call to `UnlockAchievement` will ask the Steam API to store the user stats. This could be undesirable if you are unlocking multiple achievements at once or storing stats at the same time!
:::

To avoid storing stats when calling `UnlockAchievement`, you can pass `false` as the second parameter:

```cs
SteamManager.Achievements.UnlockAchievement("API_NAME", false);
```

To then store the stats, see [Storing Stats in Stats](stats#store-stats).

## Reset Achievements

You can reset the unlock status of an achievement for the current user:

```cs
SteamManager.Achievements.ResetAchievement("API_NAME");
```

You can also reset all achievements for the current user:

```cs
SteamManager.Achievements.ResetAllAchievements();
```

:::info
Each call to `ResetAchievement` and `ResetAllAchievements` will ask the Steam API to store the user stats. This could be undesirable if you are resetting multiple achievements at once or storing stats at the same time!
:::

To avoid storing stats when calling `ResetAchievement` or `ResetAllAchievements`, you can pass `false` as the second parameter:

```cs
SteamManager.Achievements.ResetAchievement("API_NAME", false);
SteamManager.Achievements.ResetAllAchievements(false);
```

To then store the stats, see [Storing Stats in Stats](stats#store-stats).

## Show Achievement Progress

You may want to show the progress of an achievement to the user. For example, you may want to show the user how many enemies they have killed out of the total number of enemies they need to kill to unlock the achievement.

```cs
uint currentProgress = 50;
uint maxProgress = 100;
SteamManager.Achievements.IndicateAchievementProgress("API_NAME", currentProgress, maxProgress);
```

## Get Unlock Status

Check if an achievement is unlocked for the current user:

```cs
bool isUnlocked = SteamManager.Achievements.IsAchievementUnlocked("API_NAME", out DateTime unlockTime);
```

You also get the time the achievement was unlocked. If the achievement is not unlocked, the time will be [`DateTime.MinValue`](https://learn.microsoft.com/en-us/dotnet/api/system.datetime.minvalue).  
If you don't care about the time, you can discard it like this:

```cs
bool isUnlocked = SteamManager.Achievements.IsAchievementUnlocked("API_NAME", out _);
```

Check if an achievement is unlocked for a specific user:

```cs
// You can either create the Steam ID directly or you get it from somewhere else.
SteamID steamId = new SteamID(76561198051007908);
bool isUnlocked = SteamManager.Achievements.IsAchievementUnlocked(steamId "API_NAME", out DateTime unlockTime);
```

## Get Achievement Display Values

There are multiple methods to get different display values for achievements.

```cs
string displayName = SteamManager.Achievements.GetAchievementDisplayName("API_NAME");
string description = SteamManager.Achievements.GetAchievementDescription("API_NAME");
bool isHidden = SteamManager.Achievements.IsAchievementHidden("API_NAME");
```

You can also get all the display info you would need with one method:

```cs
SteamAchievement achievement = SteamManager.Achievements.GetAchievementInfo("API_NAME");
Debug.Log(achievement.ApiName);
Debug.Log(achievement.DisplayName);
Debug.Log(achievement.Description);
Debug.Log(achievement.IsHidden);
Debug.Log(achievement.IsUnlocked);
Debug.Log(achievement.UnlockTime);
```

## Get Achievement Icon

Getting an achievement icon is an asynchronous operation. That means you have to either get it using a callback or an `async` method call. You can get the icon as a `Texture2D` from a Steam image.

Using a callback:

```cs
SteamManager.Achievements.GetAchievementIcon("API_NAME", (SteamImage image) => 
{
    // You can check if the image is a valid Steam image. It may be invalid if the image didn't exist.
    Debug.Log(image.IsValid);
    // Here is the actual texture as a Texture2D.
    Debug.Log(image.Texture);
})
```

Using async:

```cs
SteamImage image = await SteamManager.Achievements.GetAchievementIconAsync("API_NAME");
```

## Getting All Achievements

Generally, you will want your achievements to be baked into your game and not loaded from Steam, but you can still load achievements from Steam if you want to.

```cs
// Get the number of achievements.
// It's important to call this method as this will prepare Steam to load the achievements!
uint achievementCount = SteamManager.Achievements.NumberOfAchievements;
for (uint i = 0; i < achievementCount; i++)
{
	// Get the API name from the Steamworks dashboard.
	string apiName = SteamManager.Achievements.GetAchievementName(i);
	// Get the display name using the API name.
	string displayName = SteamManager.Achievements.GetAchievementDisplayName(apiName);
	// Get the description using the API name.
	string description = SteamManager.Achievements.GetAchievementDescription(apiName);
}
```

## Get Global Achievement Unlock Percentage

You can get the global unlock percentage of an achievement.

:::caution
You must request the global achievement stats before you can get the global unlock percentage. That is an asynchronous operation, so you must use a callback or an `async` method call. See [Request Global Stats](#request-global-stats).
:::

```cs
float progress = SteamManager.Achievements.GetGlobalAchievementPercent("API_NAME");
```

## Get The Most Achieved Achievements

You can get the most achieved achievements for your game.

:::caution
You must request the global achievement stats before you can get the most achieved achievements. That is an asynchronous operation, so you must use a callback or an `async` method call. See [Request Global Stats](#request-global-stats).
:::

```cs
foreach (SteamGlobalAchievementInfo achievement in SteamManager.Achievements.GetMostAchievedAchievements())
{
    // The API name of the achievement.
	Debug.Log(achievement.ApiName);
    // How much percent of players have unlocked the achievement, between 0 to 100.
	Debug.Log(achievement.Percent);
    // If the current player has unlocked the achievement.
	Debug.Log(achievement.IsUnlocked);
}
```

## Request Global Stats

Some method calls requires global achievement stats to be fetched from Steam first. You can request the global achievement stats using a callback or an `async` method call.

Using a callback:

```cs
SteamManager.Achievements.RequestGlobalAchievementStats(result =>
{
	if (result == GlobalAchievementStatsResult.Success)
	{
        // Call your method here...
	}
});
```

Using async:

```cs
GlobalAchievementStatsReceivedResponse response = await SteamManager.Achievements.RequestGlobalAchievementStatsAsync();
if (response.Result == GlobalAchievementStatsResult.Success)
{
	// Call your method here...
}
```

If you've already requested the global achievement stats, you can check if they are available before getting the global unlock percentage:

```cs
if (!SteamManager.Achievements.HasGlobalStats)
{
    GlobalAchievementStatsReceivedResponse response = await SteamManager.Achievements.RequestGlobalAchievementStatsAsync();
    if (response.Result != GlobalAchievementStatsResult.Success)
    {
        return;
    }
}

// Call your method here...
```