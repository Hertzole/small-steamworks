#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;
using System.Collections;
using System.Collections.Generic;
#if !DISABLESTEAMWORKS
using UnityEngine.Pool;
using Steamworks;
#endif

namespace Hertzole.SmallSteamworks
{
	/// <summary>
	///     Iterator used to iterate over all local files that are synchronized by the Steam Cloud.
	/// </summary>
	public readonly struct SteamFiles : IEnumerable<SteamFile>
	{
#if !DISABLESTEAMWORKS
		private static readonly ObjectPool<Enumerator> enumeratorPool = new ObjectPool<Enumerator>(() => new Enumerator());
#endif

		public IEnumerator<SteamFile> GetEnumerator()
		{
#if !DISABLESTEAMWORKS
			int count = SteamRemoteStorage.GetFileCount();
			if (count == 0)
			{
				return ((IEnumerable<SteamFile>) Array.Empty<SteamFile>()).GetEnumerator();
			}

			Enumerator enumerator = enumeratorPool.Get();
			enumerator.StartNew(count);

			return enumerator;
#else
			return ((IEnumerable<SteamFile>) Array.Empty<SteamFile>()).GetEnumerator();
#endif
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

#if !DISABLESTEAMWORKS
		private sealed class Enumerator : IEnumerator<SteamFile>
		{
			private int index;
			private int count;

			private SteamFile current;

			public SteamFile Current
			{
				get { return current; }
			}

			object IEnumerator.Current
			{
				get { return Current; }
			}

			public void StartNew(int newCount)
			{
				Reset();
				count = newCount;
			}

			public bool MoveNext()
			{
				if (index >= count)
				{
					enumeratorPool.Release(this);
					return false;
				}

				string name = SteamRemoteStorage.GetFileNameAndSize(index, out int size);
				long timestamp = SteamRemoteStorage.GetFileTimestamp(name);
				bool isPersisted = SteamRemoteStorage.IsCloudEnabledForAccount() && SteamRemoteStorage.IsCloudEnabledForApp() &&
				                   SteamRemoteStorage.FilePersisted(name);

				current = new SteamFile(name, size, timestamp, isPersisted);
				index++;

				return true;
			}

			public void Reset()
			{
				current = default;
				index = 0;
			}

			public void Dispose() { }
		}
#endif
	}
}