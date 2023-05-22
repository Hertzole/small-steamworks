using System;

namespace Hertzole.SmallSteamworks
{
	public delegate void FileSharedCallback(bool success, SteamUGCHandle ugcHandle, string fileName);

	public readonly struct FileSharedResponse : IEquatable<FileSharedResponse>
	{
		public bool Success { get; }
		public SteamUGCHandle UgcHandle { get; }
		public string FileName { get; }

		public FileSharedResponse(bool success, SteamUGCHandle ugcHandle, string fileName)
		{
			Success = success;
			UgcHandle = ugcHandle;
			FileName = fileName;
		}

		public bool Equals(FileSharedResponse other)
		{
			return Success == other.Success && UgcHandle.Equals(other.UgcHandle) && FileName == other.FileName;
		}

		public override bool Equals(object obj)
		{
			return obj is FileSharedResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Success.GetHashCode();
				hashCode = (hashCode * 397) ^ UgcHandle.GetHashCode();
				hashCode = (hashCode * 397) ^ (FileName != null ? FileName.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(FileSharedResponse left, FileSharedResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(FileSharedResponse left, FileSharedResponse right)
		{
			return !left.Equals(right);
		}
	}
}