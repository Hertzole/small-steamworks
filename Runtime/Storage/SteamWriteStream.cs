using System;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

namespace Hertzole.SmallSteamworks
{
	public struct SteamWriteStream : IDisposable
	{
#if !DISABLESTEAMWORKS
		private bool isCanceled;
		private readonly UGCFileWriteStreamHandle_t handle;

		internal SteamWriteStream(UGCFileWriteStreamHandle_t handle)
		{
			this.handle = handle;
			isCanceled = false;
		}
#endif

		public void Dispose()
		{
#if !DISABLESTEAMWORKS
			if (isCanceled)
			{
				SteamRemoteStorage.FileWriteStreamCancel(handle);
			}
			else
			{
				SteamRemoteStorage.FileWriteStreamClose(handle);
			}
#endif
		}

		public void Cancel()
		{
#if !DISABLESTEAMWORKS
			isCanceled = true;
#endif
		}

		public bool WriteChunk(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}
#if !DISABLESTEAMWORKS
			if (data.Length > Constants.k_unMaxCloudFileChunkSize)
			{
				throw new FileSizeException($"Data is too large. Max size is 100MB ({Constants.k_unMaxCloudFileChunkSize} bytes).");
			}

			return SteamRemoteStorage.FileWriteStreamWriteChunk(handle, data, data.Length);
#else
			return false;
#endif
		}
	}
}