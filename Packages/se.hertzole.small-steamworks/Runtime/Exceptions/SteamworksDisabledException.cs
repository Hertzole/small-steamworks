using System;

namespace Hertzole.SmallSteamworks
{
	public sealed class SteamworksDisabledException : Exception
	{
		public SteamworksDisabledException() : base("Steamworks is disabled.") { }
	}
}