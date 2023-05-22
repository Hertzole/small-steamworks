using System;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

namespace Hertzole.SmallSteamworks
{
	public readonly struct SteamWriteBatch : IDisposable
	{
		public void Dispose()
		{
#if !DISABLESTEAMWORKS
			SteamRemoteStorage.EndFileWriteBatch();
#endif
		}
	}
}