---
sidebar_position: 5
title: Getting Started
---

# Getting Started

After [installing the package](installation), you can start using the Steamworks API. The package is designed to be as easy to use as possible as it does a lot of the heavy lifting for you.

## Set your App ID

Navigate to the [project settings](settings) window under Hertzole/Small Steamworks. Here you'll be able to modify the settings for the Steamworks API and most importantly, the [app ID](settings#app-id). You can find your app ID on the [Steamworks App Admin](https://partner.steamgames.com/apps) page.

## Initializing Steamworks

Steamworks is automatically initialized for you on game boot, so there's nothing you need to do here! Small Steamworks will also shut down the Steam API when the game shuts off, so you don't need to do anything there either!

:::info
When running the game in the editor, Steam will still say that you're playing your game when you've exited play mode. Your status will not go away until you close the editor.
:::

## Using the API

All the API calls start from `Hertzole.SmallSteamworks.SteamManager`. In there, you can find interfaces for all the different Steamworks features. For example, if you want to use [achievements](achievements), you can use `SteamManager.Achievements`.

A lot of the API is asynchronous and is therefore using callbacks. There are also awaitable C# `async` method alternatives for all callback methods if you prefer using await instead of callbacks.

### Example: Getting Current User Name

```cs
using Hertzole.SmallSteamworks;
using UnityEngine;

public class GetUserName : MonoBehaviour
{
	private void Start()
	{
		string myName = SteamManager.Friends.GetMyDisplayName();
		Debug.Log("My name is " + myName);
	}
}
```

### Example: Getting Another User's Name

```cs
using Hertzole.SmallSteamworks;
using UnityEngine;

public class GetUserName : MonoBehaviour
{
    private void Start()
    {
        SteamID friendId = new SteamID(76561198051007908);
        bool getNameOnly = true; // If false it will also download the avatar, which is slower.
        SteamManager.Friends.RequestUserInformation(friendId, getNameOnly, user => 
        {
            Debug.Log("User's name is " + user.Name);
        });
    }
}
```

### Example: Getting Another User's Name Async

```cs
using Hertzole.SmallSteamworks;
using UnityEngine;

public class GetUserName : MonoBehaviour
{
    private async void Start()
    {
        SteamID friendId = new SteamID(76561198051007908);
        bool getNameOnly = true; // If false it will also download the avatar, which is slower.
        
        // Async methods return responses.
        // In this case it returns a UserInformationRetrievedResponse.
        var response = await SteamManager.Friends.RequestUserInformationAsync(friendId, getNameOnly);
        Debug.Log("User's name is " + response.User.Name);
    }
}
```