---
sidebar_position: 40
title: 🥇 Leaderboards
---

## Find Leaderboard

To find a leaderboard, you need to know its name. You can find the name of a leaderboard on the Steamworks partner site. The name is case-sensitive. You will get a `SteamLeaderboard` object back from the call that you will use for most calls regarding leaderboards. The call is asynchronous, so you will need to get it using a callback or an `async` method call.

Using a callback:

```cs
SteamLeaderboard myLeaderboard;

SteamManager.Leaderboards.FindLeaderboard("LEADERBOARD_NAME", (success, leaderboard) =>
{
    if (!success)
    {
        return;
    }
    
    // Store your leaderboard from here
    myLeaderboard = leaderboard;
});
```

Using async:

```cs
FindLeaderboardResponse leaderboard = await SteamManager.Leaderboards.FindLeaderboardAsync("LEADERBOARD_NAME");
if (!leaderboard.Success)
{
    return;
}

// Store your leaderboard from here
myLeaderboard = leaderboard.Leaderboard;
```

## Find or Create Leaderboard

If you want to find a leaderboard, but it doesn't exist, you can create it. This is useful if you want to create a leaderboard on the fly. The name is case-sensitive. You will get a `SteamLeaderboard` object back from the call that you will use for most calls regarding leaderboards. The call is asynchronous, so you will need to get it using a callback or an `async` method call. You also need to provide the sort method and the display type of the leaderboard that will be used if it is created.

Using a callback:

```cs
SteamLeaderboard myLeaderboard;

SteamManager.Leaderboards.FindOrCreateLeaderboard(
    "LEADERBOARD_NAME", LeaderboardSortMethod.Ascending, LeaderboardDisplayType.Numeric, 
    (success, leaderboard) =>
    {
        if (!success)
        {
            return;
        }
        
        // Store your leaderboard from here
        myLeaderboard = leaderboard;
    });
```

Using async:

```cs
FindLeaderboardResponse leaderboard = await SteamManager.Leaderboards.FindOrCreateLeaderboardAsync(
    "LEADERBOARD_NAME", LeaderboardSortMethod.Ascending, LeaderboardDisplayType.Numeric);

if (!leaderboard.Success)
{
    return;
}

// Store your leaderboard from here
myLeaderboard = leaderboard.Leaderboard;
```

## Upload Score

Uploading a score to a leaderboard is straightforward. You need to provide the score and the leaderboard. The call is asynchronous, so you will need to get it using a callback or an `async` method call. You get the leaderboard from the [Find Leaderboard](#find-leaderboard) or [Find or Create Leaderboard](#find-or-create-leaderboard) calls.

You will need to provide the upload method. `KeepBest` will only upload the score if it is better than the current score. `ForceUpdate` will always upload the score.

You can also provide a score details array. You can use that to store additional details about the score. For example, if it's a racing game, you can store the timestamps when the players hit each checkpoint.

Using a callback:

```cs
SteamLeaderboard myLeaderboard; // Get this from Find Leaderboard or Find or Create Leaderboard
int score = 100;
int[] scoreDetails = new int[] { 1000, 2000, 3000 };

SteamManager.Leaderboards.UploadScore(myLeaderboard, score, LeaderboardUploadMethod.KeepBest, scoreDetails, 
    (success, leaderboard, submittedScore, scoreChanged, newRank, previousRank) =>
    {
        if (!success)
        {
            return;
        }
        
        // The score was uploaded successfully
        Debug.Log($"The score was uploaded: {submittedScore}");
        Debug.Log($"The score changed: {scoreChanged}");
        Debug.Log($"The new rank: {newRank}");
        Debug.Log($"The previous rank: {previousRank}");
    });
```

Using async:

```cs
SteamLeaderboard myLeaderboard; // Get this from Find Leaderboard or Find or Create Leaderboard
int score = 100;

UploadScoreResponse response = await SteamManager.Leaderboards.UploadScoreAsync(
    myLeaderboard, score, LeaderboardUploadMethod.KeepBest, null);

if (!response.Success)
{
    return;
}

// The score was uploaded successfully
Debug.Log($"The score was uploaded: {response.SubmittedScore}");
Debug.Log($"The score changed: {response.ScoreChanged}");
Debug.Log($"The new rank: {response.NewRank}");
Debug.Log($"The previous rank: {response.PreviousRank}");
```

## Get Scores

You can get the scores from a leaderboard. You can get the scores around the current user's score, the user's friends' scores, or you can get the top scores. The call is asynchronous, so you will need to get it using a callback or an `async` method call. You get the leaderboard from the [Find Leaderboard](#find-leaderboard) or [Find or Create Leaderboard](#find-or-create-leaderboard) calls.

### Get Top Scores

You can get the top scores from a leaderboard. You can specify how many scores you want to get and the offset if you want to skip some scores. The call is asynchronous, so you will need to get it using a callback or an `async` method call.

Using a callback:

```cs
SteamLeaderboard myLeaderboard; // Get this from Find Leaderboard or Find or Create Leaderboard
int count = 10;
int offset = 0; 

SteamManager.Leaderboards.GetScores(myLeaderboard, count, offset,
    (success, leaderboard, entries =>
    {
        if (!success)
        {
            return;
        }
        
        // The scores were fetched successfully
        foreach (SteamLeaderboardEntry entry in entries)
        {
            Debug.Log($"The score: {entry.Score}");
            Debug.Log($"The rank: {entry.GlobalRank}");
            Debug.Log($"The user: {entry.User}");
        }
    });
```

Using async:

```cs
SteamLeaderboard myLeaderboard; // Get this from Find Leaderboard or Find or Create Leaderboard
int count = 10;
int offset = 0;

GetScoresResponse response = await SteamManager.Leaderboards.GetScoresAsync(myLeaderboard, count, offset);

if (!response.Success)
{
    return;
}

// The scores were fetched successfully
foreach (SteamLeaderboardEntry entry in response.Entries)
{
    Debug.Log($"The score: {entry.Score}");
    Debug.Log($"The rank: {entry.GlobalRank}");
    Debug.Log($"The user: {entry.User}");
}
```

### Get Scores Around User

You can get the scores around the current user's score from a leaderboard. You can specify the start range and the end range. The start range will fetch X amount of users above the current user's score and the end score will fetch X amount of users below the current user's score. For example: If you set the start range to 2 and the end range to 2, you will get 5 entries: 2 above the current user, the current user, and two below the current user. The call is asynchronous, so you will need to get it using a callback or an `async` method call.

Using a callback:

```cs
SteamLeaderboard myLeaderboard; // Get this from Find Leaderboard or Find or Create Leaderboard
int startRange = 2;
int endRange = 2;

SteamManager.Leaderboards.GetScoresAroundUser(myLeaderboard, startRange, endRange,
    (success, leaderboard, entries =>
    {
        if (!success)
        {
            return;
        }
        
        // The scores were fetched successfully
        foreach (SteamLeaderboardEntry entry in entries)
        {
            Debug.Log($"The score: {entry.Score}");
            Debug.Log($"The rank: {entry.GlobalRank}");
            Debug.Log($"The user: {entry.User}");
        }
    });
```

Using async:

```cs
SteamLeaderboard myLeaderboard; // Get this from Find Leaderboard or Find or Create Leaderboard
int startRange = 2;
int endRange = 2;

GetScoresResponse response = await SteamManager.Leaderboards.GetScoresAroundUserAsync(myLeaderboard, startRange, endRange);

if (!response.Success)
{
    return;
}

// The scores were fetched successfully
foreach (SteamLeaderboardEntry entry in response.Entries)
{
    Debug.Log($"The score: {entry.Score}");
    Debug.Log($"The rank: {entry.GlobalRank}");
    Debug.Log($"The user: {entry.User}");
}
```

### Get Scores For Friends

You can get the scores for the current user's friends from a leaderboard. There's no max value for this method, it will just fetch *all* of the scores. The call is asynchronous, so you will need to get it using a callback or an `async` method call.

Using a callback:

```cs
SteamLeaderboard myLeaderboard; // Get this from Find Leaderboard or Find or Create Leaderboard

SteamManager.Leaderboards.GetScoresForFriends(myLeaderboard,
    (success, leaderboard, entries =>
    {
        if (!success)
        {
            return;
        }
        
        // The scores were fetched successfully
        foreach (SteamLeaderboardEntry entry in entries)
        {
            Debug.Log($"The score: {entry.Score}");
            Debug.Log($"The rank: {entry.GlobalRank}");
            Debug.Log($"The user: {entry.User}");
        }
    });
```

Using async:

```cs
SteamLeaderboard myLeaderboard; // Get this from Find Leaderboard or Find or Create Leaderboard

GetScoresResponse response = await SteamManager.Leaderboards.GetScoresForFriendsAsync(myLeaderboard);

if (!response.Success)
{
    return;
}

// The scores were fetched successfully
foreach (SteamLeaderboardEntry entry in response.Entries)
{
    Debug.Log($"The score: {entry.Score}");
    Debug.Log($"The rank: {entry.GlobalRank}");
    Debug.Log($"The user: {entry.User}");
}
```

## Attach UGC

You can attach user-generated content that you can get from [Storage](storage). You can only attach a single UGC item to a leaderboard. The item will be attached to the user's current score, so it's advised to only attach UGC if the score changes. The call is asynchronous, so you will need to get it using a callback or an `async` method call. You get the leaderboard from the [Find Leaderboard](#find-leaderboard) or [Find or Create Leaderboard](#find-or-create-leaderboard) calls.

Using a callback:

```cs
SteamLeaderboard myLeaderboard; // Get this from Find Leaderboard or Find or Create Leaderboard
SteamUGCHandle ugcHandle; // Get this from Storage

SteamManager.Leaderboards.AttachLeaderboardUGC(myLeaderboard, ugcHandle, (success, leaderboard) =>
{
    if (!success)
    {
        return;
    }
    
    // The UGC was attached successfully
});
```

Using async:

```cs
SteamLeaderboard myLeaderboard; // Get this from Find Leaderboard or Find or Create Leaderboard
SteamUGCHandle ugcHandle; // Get this from Storage

AttachLeaderboardUGCResponse response = await SteamManager.Leaderboards.AttachLeaderboardUGCAsync(
    myLeaderboard, ugcHandle);

if (!response.Success)
{
    return;
}

// The UGC was attached successfully
```