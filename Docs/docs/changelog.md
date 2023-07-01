---
sidebar_position: 7
title: 🆕 Changelog
---

## 0.2.0 - [2023-07-01]

### Added

- Added support for SteamApps (`SteamManager.Apps`)
- Added `Achievements.OnAchievementProgress` for when achievement progress is made

### Improvements

- Added pooling to image cache to reduce new image allocations
    - You can also control the size of the cache with `SteamManager.Settings.ImageCacheSize`
- Steam Settings no longer stay in memory in builds when `DISABLESTEAMWORKS` is applied
- Avatar and achievement images now have names in the editor and debug builds for easier identification

## 0.1.2 - [2023-06-05]

### Fixed
- Fixed SteamFiles and FriendsExtensions throwing compiler errors with `DISABLESTEAMWORKS` applied
- Fixed Small Steamworks throwing compiler errors on unsupported platforms

## 0.1.0 - [2023-06-01]

Initial release