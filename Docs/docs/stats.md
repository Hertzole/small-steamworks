---
sidebar_position: 50
title: 📈 Stats
---

## Request Current Stats

If you want to interact with the current user's stats or achievements, you must have the user's stats. They are usually fetched by default when the game starts, but you can also request them manually. The request method is asynchronous, so you must use a callback or an `async` method.

Using a callback:

```cs
SteamManager.Stats.RequestCurrentStats((success, id) =>
{
	if (!success)
	{
		return;
	}
	
	// Call your method here...
});
```

Using async:

```cs
UserStatsReceivedResponse response = await SteamManager.Stats.RequestCurrentStatsAsync();
if (!response.Success)
{
	return;
}

// Call your method here...
```

## Request User Stats

Sometimes you want to get the stats or achievements of another user. Then you must request their stats. The request method is asynchronous, so you must use a callback or an `async` method.

Using a callback:

```cs
SteamID steamId = new SteamID(76561198051007908);
SteamManager.Stats.RequestUserStats(steamId, (success, id) =>
{
    if (!success)
    {
        return;
    }
    
    // Call your method here...
});
```

Using async:

```cs
SteamID steamId = new SteamID(76561198051007908);
UserStatsReceivedResponse response = await SteamManager.Stats.RequestUserStatsAsync(steamId);
if (!response.Success)
{
    return;
}

// Call your method here...
```

## Request Global Stats

Some method calls requires global stats to be fetched from Steam first. You can request the global stats using a callback or an `async` method call.

You must provide the number of days to get the stats. The maximum number of days is 60.

Using a callback:

```cs
int days = 30;
SteamManager.Stats.RequestGlobalStats(days, success =>
{
    if (!success)
    {
        return;
    }
    
    // Call your method here...
});
```

Using async:

```cs
int days = 30;
bool success = await SteamManager.Stats.RequestGlobalStatsAsync(days);
if (!success)
{
    return;
}

// Call your method here...
```

## Store Stats

When you have modified stats or achievements, you need to tell Steam to update those stats. You do that with one simple call:

```cs
SteamManager.Stats.StoreStats();
```

## Set Stats

You can set two types of stats, `int` and, `float`. By default, when setting a stat it will [store stats](#store-stats). You can disable that by passing `false` as the third parameter.

```cs
// Set int stat
SteamManager.Stats.SetStatInt("STAT_API_NAME", 42);
// Set float stat
SteamManager.Stats.SetStatFloat("STAT_API_NAME", 42.5f);
```

### Update AvgRate Stat

You can update an `AvgRate` stat. This will update the average rate of the stat. By default, when setting a stat it will [store stats](#store-stats). You can disable that by passing `false` as the third parameter.

To learn more about `AvgRate` stats, see [Steamworks.NET documentation](https://partner.steamgames.com/doc/features/achievements#AVGRATE).

```cs
SteamManager.Stats.UpdateAvgRateStat("STAT_API_NAME", 42.5f, 1.0f);
```

## Get Stats

You can get two types of stats, `int` and, `float`. These will be the stats for the current user.

```cs
// Get int stat
int statInt = SteamManager.Stats.GetStatInt("STAT_API_NAME");
// Get float stat
float statFloat = SteamManager.Stats.GetStatFloat("STAT_API_NAME");
```

### Get Stats from Other Users

You can also get stats from a specific user.

:::caution
You must request the user stats for the user before you can get the stats for a user. That is an asynchronous operation, so you must use a callback or an `async` method call. See [Request User Stats](#request-user-stats).
:::

```cs
SteamID steamId = new SteamID(76561198051007908);
SteamManager.Stats.RequestUserStats(steamId, (success, id) =>
{
	if (!success)
	{
		return;
	}

	// Get int stat
	int statInt = SteamManager.Stats.GetUserStatInt(steamId, "STAT_API_NAME");
	// Get float stat
	float statFloat = SteamManager.Stats.GetUserStatFloat(steamId, "STAT_API_NAME");
});
```

### Get Global Stats

You can get the global stats for a stat. These will be the stats for all users. Since they will most likely be large numbers, they are returned as `long` and `double` instead.

:::caution
You must request the global stats before you can get the stats. That is an asynchronous operation, so you must use a callback or an `async` method call. See [Request Global Stats](#request-global-stats).
:::

```cs
int days = 30;
SteamManager.Stats.RequestGlobalStats(days, success =>
{
	if (!success)
	{
		return;
	}
	
	// Get int stat
	long statInt = SteamManager.Stats.GetGlobalStatInt("STAT_API_NAME");
	// Get float stat
	double statFloat = SteamManager.Stats.GetGlobalStatFloat("STAT_API_NAME");
});
```

## Get Global Stat History

You can get the global stat history for a stat. You will get an array of `long`/`double` with each entry being the total stats for that day. The first entry will be the total stats for the first day, the second entry will be the total stats for the second day, and so on. You can specify the number of days to get the stats.

:::caution
You must request the global stats before you can get the stats. That is an asynchronous operation, so you must use a callback or an `async` method call. See [Request Global Stats](#request-global-stats).
:::

```cs
int days = 30;
SteamManager.Stats.RequestGlobalStats(days, success =>
{
    if (!success)
    {
        return;
    }
    
    // Get int stat
    long[] statInt = SteamManager.Stats.GetGlobalStatHistoryInt("STAT_API_NAME");
    // Get float stat
    double[] statFloat = SteamManager.Stats.GetGlobalStatHistoryFloat("STAT_API_NAME");
});
```

## Reset Stats

Sometimes during development, you want to reset the stats. You can do that with one simple call:

```cs
SteamManager.Stats.ResetAllStats();
```

If you want to reset achievements too at the same time, you can pass `true` as the first parameter:

```cs
SteamManager.Stats.ResetAllStats(true);
```

If you only want to reset achievements, see [Reset Achievements](achievements#reset-achievements).
