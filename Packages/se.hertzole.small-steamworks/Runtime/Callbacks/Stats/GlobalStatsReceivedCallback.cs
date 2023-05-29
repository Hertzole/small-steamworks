using System;

namespace Hertzole.SmallSteamworks
{
	public delegate void GlobalStatsReceivedCallback(bool success);

	public readonly struct GlobalStatsReceivedResponse : IEquatable<GlobalStatsReceivedResponse>
	{
		public bool Success { get; }

		internal GlobalStatsReceivedResponse(bool success)
		{
			Success = success;
		}

		public bool Equals(GlobalStatsReceivedResponse other)
		{
			return Success == other.Success;
		}

		public override bool Equals(object obj)
		{
			return obj is GlobalStatsReceivedResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			return Success.GetHashCode();
		}

		public static bool operator ==(GlobalStatsReceivedResponse left, GlobalStatsReceivedResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(GlobalStatsReceivedResponse left, GlobalStatsReceivedResponse right)
		{
			return !left.Equals(right);
		}
	}
}