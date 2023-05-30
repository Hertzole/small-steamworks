using System;

namespace Hertzole.SmallSteamworks
{
	public delegate void UGCDownloadedCallback(bool success, SteamUGCHandle ugcHandle, AppID appId, int sizeInBytes, string fileName, SteamID ownerId);

	public readonly struct UGCDownloadedResponse : IEquatable<UGCDownloadedResponse>
	{
		public bool Success { get; }
		public SteamUGCHandle UgcHandle { get; }
		public AppID AppId { get; }
		public int SizeInBytes { get; }
		public string FileName { get; }
		public SteamID OwnerId { get; }

		public UGCDownloadedResponse(bool success, SteamUGCHandle ugcHandle, AppID appId, int sizeInBytes, string fileName, SteamID ownerId)
		{
			Success = success;
			UgcHandle = ugcHandle;
			AppId = appId;
			SizeInBytes = sizeInBytes;
			FileName = fileName;
			OwnerId = ownerId;
		}

		public bool Equals(UGCDownloadedResponse other)
		{
			return Success == other.Success && UgcHandle.Equals(other.UgcHandle) && AppId == other.AppId && SizeInBytes == other.SizeInBytes &&
			       FileName == other.FileName && OwnerId == other.OwnerId;
		}

		public override bool Equals(object obj)
		{
			return obj is UGCDownloadedResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Success.GetHashCode();
				hashCode = (hashCode * 397) ^ UgcHandle.GetHashCode();
				hashCode = (hashCode * 397) ^ AppId.GetHashCode();
				hashCode = (hashCode * 397) ^ SizeInBytes;
				hashCode = (hashCode * 397) ^ (FileName != null ? FileName.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ OwnerId.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(UGCDownloadedResponse left, UGCDownloadedResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(UGCDownloadedResponse left, UGCDownloadedResponse right)
		{
			return !left.Equals(right);
		}
	}
}