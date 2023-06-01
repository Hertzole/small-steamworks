using System;

namespace Hertzole.SmallSteamworks
{
	/// <summary>
	///     Represents a local file that is synchronized by the Steam Cloud.
	/// </summary>
	public readonly struct SteamFile : IEquatable<SteamFile>
	{
		/// <summary>
		///     The name of the file.
		/// </summary>
		public string Name { get; }
		/// <summary>
		///     The size of the file in bytes.
		/// </summary>
		public int Size { get; }
		/// <summary>
		///     The timestamp of the file when it was last updated. This is a Unix timestamp.
		/// </summary>
		public DateTime Timestamp { get; }

		internal SteamFile(string name, int size, long timestamp)
		{
			Name = name;
			Size = size;
			Timestamp = DateTime.UnixEpoch.AddSeconds(timestamp);
		}

		public override string ToString()
		{
			return $"{nameof(Name)}: {Name}, {nameof(Size)}: {Size}, {nameof(Timestamp)}: {Timestamp}";
		}

		public bool Equals(SteamFile other)
		{
			return Name == other.Name && Size == other.Size;
		}

		public override bool Equals(object obj)
		{
			return obj is SteamFile other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ Size;
			}
		}

		public static bool operator ==(SteamFile left, SteamFile right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SteamFile left, SteamFile right)
		{
			return !left.Equals(right);
		}
	}
}