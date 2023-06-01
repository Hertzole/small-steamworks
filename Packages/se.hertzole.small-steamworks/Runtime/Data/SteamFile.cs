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
		/// <summary>
		///     Is the file persisted? This is only true if the file is stored in the Steam Cloud.
		/// </summary>
		public bool IsPersisted { get; }

		internal SteamFile(string name, int size, long timestamp, bool isPersisted)
		{
			Name = name;
			Size = size;
			Timestamp = DateTime.UnixEpoch.AddSeconds(timestamp);
			IsPersisted = isPersisted;
		}

		public override string ToString()
		{
			return $"{nameof(Name)}: {Name}, {nameof(Size)}: {Size}, {nameof(Timestamp)}: {Timestamp}, {nameof(IsPersisted)}: {IsPersisted}";
		}

		public bool Equals(SteamFile other)
		{
			return Name == other.Name && Size == other.Size && Timestamp.Equals(other.Timestamp) && IsPersisted == other.IsPersisted;
		}

		public override bool Equals(object obj)
		{
			return obj is SteamFile other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Name != null ? Name.GetHashCode() : 0;
				hashCode = (hashCode * 397) ^ Size;
				hashCode = (hashCode * 397) ^ Timestamp.GetHashCode();
				hashCode = (hashCode * 397) ^ IsPersisted.GetHashCode();
				return hashCode;
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