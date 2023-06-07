using System;

namespace Hertzole.SmallSteamworks
{
	public delegate void FileDetailsCallback(bool fileExists, ulong fileSize, byte[] fileSHA);

	public readonly struct FileDetailsResponse : IEquatable<FileDetailsResponse>
	{
		public bool FileExists { get; }
		public ulong FileSize { get; }
		public byte[] FileSHA { get; }

		internal FileDetailsResponse(bool fileExists, ulong fileSize, byte[] fileSHA)
		{
			FileExists = fileExists;
			FileSize = fileSize;
			FileSHA = fileSHA;
		}

		public bool Equals(FileDetailsResponse other)
		{
			return FileExists == other.FileExists && FileSize == other.FileSize && Equals(FileSHA, other.FileSHA);
		}

		public override bool Equals(object obj)
		{
			return obj is FileDetailsResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = FileExists.GetHashCode();
				hashCode = (hashCode * 397) ^ FileSize.GetHashCode();
				hashCode = (hashCode * 397) ^ (FileSHA != null ? FileSHA.GetHashCode() : 0);
				return hashCode;
			}
		}

		public override string ToString()
		{
			return $"{nameof(FileExists)}: {FileExists}, {nameof(FileSize)}: {FileSize}, {nameof(FileSHA)}: {FileSHA}";
		}

		public static bool operator ==(FileDetailsResponse left, FileDetailsResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(FileDetailsResponse left, FileDetailsResponse right)
		{
			return !left.Equals(right);
		}
	}
}