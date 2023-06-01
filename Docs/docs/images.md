---
sidebar_position: 15
title: 🖼 Images
---

Images are used all over Steam. They are used for avatars, achievements, leaderboards, and more. 

This page contains important information when dealing with images.

## Disposing Images

When you are done with an image, you can dispose of it by calling `Dispose()` on it. This will free up the memory used by the image. **If you don't dispose of an image, you will get a memory leak!**

Here is an example based on a leaderboard entry:

```cs
private SteamImage avatarImage;

public void SetEntry(SteamLeaderboardEntry entry)
{
    SteamManager.Friends.GetAvatar(entry.User.SteamID, AvatarSize.Large, (image, _, _, _) =>
    {
        avatarImage = image;
    });
}

private void OnDestroy()
{
    if (avatarImage.IsValid)
    {
        avatarImage.Dispose();
    }
}
```