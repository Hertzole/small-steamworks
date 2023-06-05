---
sidebar_position: 6
title: ❓ FAQ
---

## How do I disable Steamworks?

If you want to disable Steamworks, you can do so by going to `Project Settings > Player > Other Settings` and adding `DISABLESTEAMWORKS` to the Scripting Define Symbols. This will disable all Steamworks functionality. You can still call the SteamManager methods, but they will throw an exception. `SteamManager.IsInitialized` will also return false.

## Do I need steam_appid.txt?

No, you don't need to include a `steam_appid.txt` file. Steamworks.NET may create one for you, but it does not need to be included in your project, or your build. You should just set the app ID in the [Steam Settings](settings#app-id) and it will be used instead.

## Something isn't working!

If something isn't working, first make sure you've followed all the steps in the [Getting Started](getting-started) guide. If you're still having issues, you can go to `Project Settings > Player > Other Settings` and add `STEAMWORKS_DEBUG` to the Scripting Define Symbols. This will cause a lot of logs to be printed to the console. You can use them to figure out what's going wrong. If you can't figure it out, you can [open an issue](https://github.com/Hertzole/small-steamworks/issues/new) and I'll try to help you. Make sure to include the logs.

