﻿using System;
using UnityEngine;
#if !DISABLESTEAMWORKS
using Hertzole.SmallSteamworks.Helpers;
using Steamworks;
#endif

namespace Hertzole.SmallSteamworks
{
	public readonly struct SteamImage : IEquatable<SteamImage>, IDisposable
	{
		private readonly int handle;
		private readonly uint id;

#if !DISABLESTEAMWORKS
		private static readonly SteamLogger<SteamImage> logger = new SteamLogger<SteamImage>();
#endif

		public Texture2D Texture
		{
			get
			{
#if !DISABLESTEAMWORKS
				return SteamImageCache.GetTexture(id, handle);
#else
				throw new SteamworksDisabledException();
#endif
			}
		}

		public bool IsValid
		{
			get
			{
#if !DISABLESTEAMWORKS
				return handle != 0 && !SteamImageCache.IsDisposed(id);
#else
				return false;
#endif
			}
		}

		internal SteamImage(int handle)
		{
			this.handle = handle;

#if !DISABLESTEAMWORKS
			id = SteamImageCache.GetNextId();
#else
			id = 0;
#endif
		}

		public bool Equals(SteamImage other)
		{
			return handle == other.handle;
		}

		public override bool Equals(object obj)
		{
			return obj is SteamImage other && Equals(other);
		}

		public override int GetHashCode()
		{
			return handle;
		}

		public static bool operator ==(SteamImage left, SteamImage right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SteamImage left, SteamImage right)
		{
			return !left.Equals(right);
		}

		public void Dispose()
		{
#if !DISABLESTEAMWORKS
			logger.Log("Disposing image " + id + " (" + handle + ")");

			SteamImageCache.DisposeTexture(id);
#endif
		}
	}
}