#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;

namespace Hertzole.SmallSteamworks
{
	public delegate void FileWrittenCallback(FileWrittenResult result);

	public readonly struct FileWrittenResponse : IEquatable<FileWrittenResponse>
	{
		/// <summary>
		///     The result of the file write.
		/// </summary>
		public FileWrittenResult Result { get; }

		internal FileWrittenResponse(FileWrittenResult result)
		{
			Result = result;
		}

		public bool Equals(FileWrittenResponse other)
		{
			return Result == other.Result;
		}

		public override bool Equals(object obj)
		{
			return obj is FileWrittenResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			return (int) Result;
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