---
sidebar_position: 60
title: Storage
---

:::caution Unfinished API
The storage API is not yet finished. It has the important parts already, but not all functions are implemented. This page only covers what's implemented.
:::

You can use the storage API to save and load data from the cloud. This is useful for saving user data like settings or progress. You can also use it to share data between users.

## Writing Files

There are two ways to write data to the cloud. You can write a file synchronously or you can write a file asynchronously. The asynchronous method is recommended as it won't block the main thread. The async method has a callback and an `async` method for when the file is written.

If a file already exists, it will be overwritten.

### Write a File Synchronously

:::caution
This method will block the main thread until the file is written. This is not recommended.
:::

```cs
string fileName = "my_file.txt";
byte[] data = Encoding.UTF8.GetBytes("Hello, world!");
FileWrittenResponse response = SteamManager.Storage.WriteFileSynchronous(fileName, data);
if (response.Result == FileWrittenResult.Success)
{
    // The file was written successfully.
}
```

### Write a File Asynchronously

Using a callback:

```cs
string fileName = "my_file.txt";
byte[] data = Encoding.UTF8.GetBytes("Hello, world!");
SteamManager.Storage.WriteFile(result =>
{
    if (result == FileWrittenResult.Success)
    {
        // The file was written successfully.
    }    
});
```

Using async:

```cs
string fileName = "my_file.txt";
byte[] data = Encoding.UTF8.GetBytes("Hello, world!");
FileWrittenResponse response = await SteamManager.Storage.WriteFileAsync(fileName, data);
if (response.Result == FileWrittenResult.Success)
{
    // The file was written successfully.
}
```

### Batch Writing

You can batch-write multiple files at once. This is useful if you want to save multiple files at once so Steam doesn't have to make multiple requests.

```cs
string fileName1 = "my_file1.txt";
byte[] data1 = Encoding.UTF8.GetBytes("Hello, world!");
string fileName2 = "my_file2.txt";
byte[] data2 = Encoding.UTF8.GetBytes("Hello, world!");

using (SteamManager.Storage.WriteBatch())
{
	await SteamManager.Storage.WriteFileAsync(fileName1, data1);
    await SteamManager.Storage.WriteFileAsync(fileName2, data2);
}
```

### Write Stream

You can write chunks of a file at a time in a stream. This is useful if you want to write a really large file in chunks

```cs
string fileName = "my_file.txt";
byte[] data = Encoding.UTF8.GetBytes("Hello, world!");

using (SteamWriteStream stream = SteamManager.Storage.WriteStream(fileName))
{
    stream.Write(data);
}
```

You can also cancel a stream at any time. **You still need to dispose the stream, even if you cancel it!**

```cs
string fileName = "my_file.txt";
byte[] data = Encoding.UTF8.GetBytes("Hello, world!");
using (SteamWriteStream stream = SteamManager.Storage.WriteStream(fileName))
{
    stream.Write(data);
    stream.Cancel();
}
```

## Reading Files

There are two ways to read data from the cloud. You can read a file synchronously or you can read a file asynchronously. The asynchronous method is recommended as it won't block the main thread. The async method has a callback and an `async` method for when the file is read.

### Read a File Synchronously

:::caution
This method will block the main thread until the file is read. This is not recommended.
:::

```cs
string fileName = "my_file.txt";
FileReadResponse response = SteamManager.Storage.ReadFileSynchronous(fileName);
if (response.Success)
{
    byte[] data = response.Data;
}
```

### Read a File Asynchronously

Using a callback:

```cs
string fileName = "my_file.txt";
SteamManager.Storage.ReadFile(fileName, (success, data) =>
{
    if (success)
    {
        byte[] data = data;
    }
});
```

Using async:

```cs
string fileName = "my_file.txt";
FileReadResponse response = await SteamManager.Storage.ReadFileAsync(fileName);
if (response.Success)
{
    byte[] data = response.Data;
}
```

## Get All Files

You can enumerate all the files that are available locally that have been synchronized with the Steam Cloud.

:::info
You must use a `foreach` loop to enumerate the files. You cannot use a `for` loop.
:::

```cs
foreach (SteamFile file in SteamManager.Storage.GetAllFiles())
{
    string fileName = file.Name; // The name of the file.
    int fileSize = file.Size; // The size of the file in bytes.
    bool persisted = file.IsPersisted; // Whether the file is persisted in the cloud.
    DateTime timestamp = file.Timestamp; // The timestamp of the last update.
}
```

## File Exists

You can check if a file exists in the cloud.

```cs
string fileName = "my_file.txt";
bool exists = SteamManager.Storage.FileExists(fileName);
```

## File Persisted

You can check if a file is persisted in the cloud.

```cs
string fileName = "my_file.txt";
bool persisted = SteamManager.Storage.FilePersisted(fileName);
```

## Forget File

Deletes the file from remote storage, but leaves it on the local disk and remains accessible from the API.

```cs
string fileName = "my_file.txt";
bool success = SteamManager.Storage.ForgetFile(fileName);
if (success)
{
    // The file was forgotten successfully.
}
```

## Delete File

Deletes the file from the remote storage and deletes the local copy.

If you want to only delete the remote copy but keep the local one, use [Forget File](#forget-file).

```cs
string fileName = "my_file.txt";
bool success = SteamManager.Storage.DeleteFile(fileName);
if (success)
{
    // The file was deleted successfully.
}
```

## Get File Size

Gets the size of the file in bytes.

```cs
string fileName = "my_file.txt";
int size = SteamManager.Storage.GetFileSize(fileName);
```

## Get Quota

Gets the total number of bytes that can be used in the cloud. The response contains the total number of bytes that can be used in the cloud and the number of bytes that are currently available.

```cs
QuotaResponse response = SteamManager.Storage.GetQuota();
if (response.Success)
{
    int totalBytes = response.TotalBytes;
    int availableBytes = response.AvailableBytes;
}
```

## Share File to UGC

You can share a file with the community. This can be used to [attach a file to a leaderboard entry](leaderboards#attach-ugc). The file must be persisted in the cloud. You can check if a file is persisted with [File Persisted](#file-persisted). The request method is asynchronous, so you must use a callback or an `async` method.

Using a callback:

```cs
string fileName = "my_file.txt";
SteamManager.Storage.ShareFileToUGC(fileName, (success, ugcHandle, fileName) =>
{
    if (success)
    {
        // The file was shared successfully.
    }
});
```

Using async:

```cs
string fileName = "my_file.txt";
UGCSharedResponse response = await SteamManager.Storage.ShareFileToUGCAsync(fileName);
if (response.Success)
{
    // The file was shared successfully.
}
```

## Download Shared File

You can download a shared file from the community. This can be used to [download a file attached to a leaderboard entry](leaderboards#attach-ugc). The request method is asynchronous, so you must use a callback or an `async` method.

Using a callback:

```cs
SteamUGCHandle ugcHandle; // This is the handle of the UGC item.
int priority = 0; // This is the priority of the download. 0 is the highest priority.
SteamManager.Storage.DownloadSharedFile(ugcHandle, priority, 
    (success, handle, appId, sizeInBytes, name, ownerId) =>
    {
        if (success)
        {
            // The file was downloaded successfully.
        }
    });
```

Using async:

```cs
SteamUGCHandle ugcHandle; // This is the handle of the UGC item.
int priority = 0; // This is the priority of the download. 0 is the highest priority.
UGCDownloadedResponse response = await SteamManager.Storage.DownloadSharedFileAsync(ugcHandle, priority);
if (response.Success)
{
    // The file was downloaded successfully.
}
```

### Download to Location

You can also download a shared file to a specific location. This is useful if you want to download a file to a specific location instead of the default location.

Using a callback:

```cs
SteamUGCHandle ugcHandle; // This is the handle of the UGC item.
int priority = 0; // This is the priority of the download. 0 is the highest priority.
string location = "C:/my_file.txt"; // This is the location to download the file to.
SteamManager.Storage.DownloadSharedFileToLocation(ugcHandle, location, priority, 
    (success, handle, appId, sizeInBytes, name, ownerId) =>
    {
        if (success)
        {
            // The file was downloaded successfully.
        }
    });
```

Using async:

```cs
SteamUGCHandle ugcHandle; // This is the handle of the UGC item.
int priority = 0; // This is the priority of the download. 0 is the highest priority.
string location = "C:/my_file.txt"; // This is the location to download the file to.
UGCDownloadedResponse response = await SteamManager.Storage.DownloadSharedFileToLocationAsync(
    ugcHandle, location, priority);
if (response.Success)
{
    // The file was downloaded successfully.
}
```

## Read Shared File

You can read a shared file from the community. This can be used to [read a file attached to a leaderboard entry](leaderboards#attach-ugc).

```cs
SteamUGCHandle ugcHandle; // This is the handle of the UGC item.
byte[] data = SteamManager.Storage.ReadSharedFile(ugcHandle, out int fileSize);
```