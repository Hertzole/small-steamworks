using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine.Pool;

namespace Hertzole.SmallSteamworks
{
	/// <summary>
	///     Iterator used to iterate over all local files that are synchronized by the Steam Cloud.
	/// </summary>
	public readonly struct SteamFiles : IEnumerable<SteamFile>
	{
		private static readonly ObjectPool<Enumerator> enumeratorPool = new ObjectPool<Enumerator>(() => new Enumerator());

		public IEnumerator<SteamFile> GetEnumerator()
		{
			int count = SteamRemoteStorage.GetFileCount();
			if (count == 0)
			{
				return ((IEnumerable<SteamFile>) Array.Empty<SteamFile>()).GetEnumerator();
			}

			Enumerator enumerator = enumeratorPool.Get();
			enumerator.StartNew(count);

			return enumerator;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

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
				current = new SteamFile(name, size, timestamp);
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
	}
}