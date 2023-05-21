using System;
using System.Runtime.InteropServices;

namespace Hertzole.SmallSteamworks
{
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public readonly struct QuotaResponse : IEquatable<QuotaResponse>
	{
		public ulong TotalBytes { get; }
		public ulong AvailableBytes { get; }

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