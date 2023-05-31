---
sidebar_position: 30
title: Friends
---

:::caution Unfinished API
The friends API is not yet finished. It has the important parts already, but not all functions are implemented. This page only covers what's implemented.
:::

You can use the friends API to get information about other Steam users in-game. This includes their name, avatar, and more. You can also use it to get information about the current user's friends. Sometimes, other users don't even have to be the current user's friends!

## Request User Information

You can request information about a user by their Steam ID. You must do this for most of the calls involving other users if they aren't connected to the current user in some way. You can also choose if you want to download their avatar when requesting information. It's a lot slower to download avatars so if you don't need it, skip it. The request method is asynchronous, so you must use a callback or an `async` method.

Using a callback:

```cs
SteamID steamId = new SteamID(76561198051007908);
bool nameOnly = true;

SteamManager.Friends.RequestUserInfo(steamId, nameOnly, user =>
{
    Debug.Log($"Name: {user.Name}");
    Debug.Log($"ID: {user.SteamID}");
});
```

Using async:

```cs
SteamID steamId = new SteamID(76561198051007908);
bool nameOnly = true;

UserInformationRetrievedResponse response = await SteamManager.Friends.RequestUserInfoAsync(steamId, nameOnly);
if (response.Success)
{
    Debug.Log($"Name: {response.User.Name}");
    Debug.Log($"ID: {response.User.SteamID}");
}
```

## Get Names

### Get the Current User's Name

You can get the current user's name.

```cs
string myName = SteamManager.Friends.GetCurrentUserDisplayName();
```

or

```cs
string myName = SteamManager.Friends.CurrentUser.Name;
```

### Get a User's Name

You can get another user's name.

:::tip
You may need to [request user information](#request-user-information) before you can get their name.   
However, if a user is in the same lobby, chat room, on a game server, from a leaderboard entry, or is a friend of the current user, you can get their name without requesting their information.
:::

```cs
SteamID steamId = new SteamID(76561198051007908);
string name = SteamManager.Friends.GetUserDisplayName(steamId);
```

## Get Avatars

When getting avatars, you can choose between three sizes: small, medium, and large. The small size is 32x32, the medium size is 64x64, and the large size is 128x128. The request method is asynchronous, so you must use a callback or an `async` method.

:::tip
You may need to [request user information](#request-user-information) before you can get their avatar, and be sure to request the avatar too.   
However, if a user is in the same lobby, chat room, on a game server, from a leaderboard entry, or is a friend of the current user, you can get their name without requesting their information.
:::

:::danger
`SteamImage` is a disposable type and must be disposed of when you are done with it. You can do that by calling `Dispose()` on the image. If you don't dispose of it, you will get a memory leak! See [Disposing Images](images#disposing-images).
:::

### Get the Current User's Avatar

You can get the current user's avatar. 

Using a callback:

```cs
SteamManager.Friends.GetMyAvatar(AvatarSize.Large, (image, userId, width, height) =>
{
    // You can check if the image is a valid Steam image. It may be invalid if the image didn't exist.
    Debug.Log(image.IsValid);
    // Here is the actual texture as a Texture2D.
    Debug.Log(image.Texture);
});
```

Using async:

```cs
AvatarRetrievedResponse response = await SteamManager.Friends.GetMyAvatarAsync(AvatarSize.Large);

// You can check if the image is a valid Steam image. It may be invalid if the image didn't exist.
Debug.Log(response.Image.IsValid);
// Here is the actual texture as a Texture2D.
Debug.Log(response.Image.Texture);
```

### Get a User's Avatar

You can get another user's avatar.

Using a callback:

```cs
SteamID steamId = new SteamID(76561198051007908);
SteamManager.Friends.GetAvatar(steamId, AvatarSize.Large, (image, userId, width, height) =>
{
    // You can check if the image is a valid Steam image. It may be invalid if the image didn't exist.
    Debug.Log(image.IsValid);
    // Here is the actual texture as a Texture2D.
    Debug.Log(image.Texture);
});
```

Using async:

```cs
SteamID steamId = new SteamID(76561198051007908);
AvatarRetrievedResponse response = await SteamManager.Friends.GetAvatarAsync(steamId, AvatarSize.Large);

// You can check if the image is a valid Steam image. It may be invalid if the image didn't exist.
Debug.Log(response.Image.IsValid);
// Here is the actual texture as a Texture2D.
Debug.Log(response.Image.Texture);
```

## Get Friends

You can get all friends as an enumerable that you can loop through.

```cs
IEnumerable<SteamUser> friends = SteamManager.Friends.GetFriends();
foreach (SteamUser friend in friends)
{
    Debug.Log($"Name: {friend.Name}");
    Debug.Log($"ID: {friend.SteamID}");
}
```