---
sidebar_position: 10
title: Steam Settings
---

![Steam Settings](/img/steam_settings.webp)

Steam settings are located in the project settings window under Hertzole/Small Steamworks. Here you'll be able to modify the settings for the Steamworks API.

## General Settings

### App ID
This is the app ID for your game. It is used to initialize the Steamworks API. You can find your app ID on the [Steamworks App Admin](https://partner.steamgames.com/apps) page.

### Restart App If Necessary
If this is enabled, the game will restart if it wasn't started through Steam. This can be used as a rudimentary anti-piracy measure.

## Achievements & Stats

### Fetch Current Stats On Boot
If true, the game will fetch the current user's stats from Steam on boot. This is useful if you want to display the current stats or achievements on a UI element. Otherwise, you'd have to request the stats yourself. See [Request Current Stats](stats#request-current-stats) for more information.