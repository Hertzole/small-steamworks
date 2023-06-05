#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;

namespace Hertzole.SmallSteamworks
{
	public readonly struct QuotaResponse : IEquatable<QuotaResponse>
	{
		/// <summary>
		///     The total number of bytes available for the user.
		/// </summary>
		public ulong TotalBytes { get; }
		/// <summary>
		///     The amount of bytes available for the user.
		/// </summary>
		public ulong AvailableBytes { get; }
		/// <summary>
		///     The amount of bytes used by the user.
		/// </summary>
		public ulong BytesUsed
		{
			get { return TotalBytes - AvailableBytes; }
		}

		internal QuotaResponse(ulong totalBytes, ulong availableBytes)
		{
			TotalBytes = totalBytes;
			AvailableBytes = availableBytes;
		}

		public bool Equals(QuotaResponse other)
		{
			return TotalBytes == other.TotalBytes && AvailableBytes == other.AvailableBytes;
		}

		public override bool Equals(object obj)
		{
			return obj is QuotaResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (TotalBytes.GetHashCode() * 397) ^ AvailableBytes.GetHashCode();
			}
		}

		public static bool operator ==(QuotaResponse left, QuotaResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(QuotaResponse left, QuotaResponse right)
		{
			return !left.Equals(right);
		}
	}
}