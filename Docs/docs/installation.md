---
sidebar_position: 3
title: Installation
---

## Install Steamworks.NET

Small Steamworks requires [Steamworks.NET](https://github.com/rlabrecque/Steamworks.NET) to work. See their [installation guide](https://steamworks.github.io/installation/) for more information.

## Open UPM (Recommended)

The package can be installed via [OpenUPM](https://openupm.com/packages/se.hertzole.small-steamworks/).

### Using CLI

If you have the [OpenUPM CLI](https://github.com/openupm/openupm-cli#openupm-cli) installed you can run the following command in your project folder:

```bash
openupm add se.hertzole.small-steamworks
```

That will install the latest version for you when you get back to Unity.

### Manually

1. Open **Edit/Project Settings/Package Manager**
2. Add a new Scoped Registry (or edit the existing OpenUPM entry)  
    **Name:** `package.openupm.com`  
    **URL**: `https://package.openupm.com`  
    **Scope(s)**: `se.hertzole.small-steamworks`
3. Click `Save` (or `Apply`)
4. Open **Window/Package Manager**
5. Click `+`
6. Select `Add package by name...` or `Add package from git URL...`
7. Paste `se.hertzole.runtime-small-steamworks` into name
8. Click `Add`

## Git URL

1. Open **Window/Package Manager**
2. Click `+`
3. Select `Add package from git URL...`
4. Paste in `https://github.com/Hertzole/small-steamworks.git#package`

### Development version

:::caution
The development version is not guaranteed to be stable and might not work at all. Use at your own risk!
:::

If you want to use the latest development version, you can use the following URL instead: `https://github.com/Hertzole/small-steamworks.gitdev-#package`