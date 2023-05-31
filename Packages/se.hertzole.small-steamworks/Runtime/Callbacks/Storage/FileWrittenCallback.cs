using System;

namespace Hertzole.SmallSteamworks
{
	public delegate void FileWrittenCallback(bool success);

	public readonly struct FileWrittenResponse : IEquatable<FileWrittenResponse>
	{
		/// <summary>
		///     Whether the file was written successfully.
		/// </summary>
		public bool Success { get; }

		internal FileWrittenResponse(bool success)
		{
			Success = success;
		}

		public bool Equals(FileWrittenResponse other)
		{
			return Success == other.Success;
		}

		public override bool Equals(object obj)
		{
			return obj is FileWrittenResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			return Success.GetHashCode();
		}

		public static bool operator ==(FileWrittenResponse left, FileWrittenResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(FileWrittenResponse left, FileWrittenResponse right)
		{
			return !left.Equals(right);
		}
	}
}