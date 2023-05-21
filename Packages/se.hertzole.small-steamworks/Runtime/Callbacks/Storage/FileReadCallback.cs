using System;

namespace Hertzole.SmallSteamworks
{
	public delegate void FileReadCallback(bool success, byte[] data);

	public readonly struct FileReadResponse : IEquatable<FileReadResponse>
	{
		public bool Success { get; }
		public byte[] Data { get; }

		internal FileReadResponse(bool success, byte[] data)
		{
			Success = success;
			Data = data;
		}

		public bool Equals(FileReadResponse other)
		{
			return Success == other.Success && Equals(Data, other.Data);
		}

		public override bool Equals(object obj)
		{
			return obj is FileReadResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Success.GetHashCode() * 397) ^ (Data != null ? Data.GetHashCode() : 0);
			}
		}

		public static bool operator ==(FileReadResponse left, FileReadResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(FileReadResponse left, FileReadResponse right)
		{
			return !left.Equals(right);
		}
	}
}