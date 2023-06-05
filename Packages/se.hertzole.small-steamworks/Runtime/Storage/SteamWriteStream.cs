#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

namespace Hertzole.SmallSteamworks
{
	public struct SteamWriteStream : IDisposable, IEquatable<SteamWriteStream>
	{
		public bool Equals(SteamWriteStream other)
		{
#if !DISABLESTEAMWORKS
			return handle.Equals(other.handle);
#else
			return false;
#endif
		}

		public override bool Equals(object obj)
		{
			return obj is SteamWriteStream other && Equals(other);
		}

		public override int GetHashCode()
		{
#if !DISABLESTEAMWORKS
			return handle.GetHashCode();
#else
			return 0;
#endif
		}

		public static bool operator ==(SteamWriteStream left, SteamWriteStream right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SteamWriteStream left, SteamWriteStream right)
		{
			return !left.Equals(right);
		}
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