using System;
using UnityEngine;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

namespace Hertzole.SmallSteamworks
{
	public struct SteamImage : IEquatable<SteamImage>
	{
		private readonly int handle;

#if !DISABLESTEAMWORKS
		private Texture2D texture;
#endif

		public Texture2D Texture
		{
			get
			{
#if !DISABLESTEAMWORKS
				if (texture == null)
				{
					texture = GetTexture(handle);
				}

				return texture;
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
				return handle != 0;
#else
				return false;
#endif
			}
		}

		public SteamImage(int handle)
		{
			this.handle = handle;

#if !DISABLESTEAMWORKS
			texture = null;
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

#if !DISABLESTEAMWORKS
		private static Texture2D GetTexture(int handle)
		{
			if (!SteamUtils.GetImageSize(handle, out uint width, out uint height))
			{
				return null;
			}

			byte[] buffer = new byte[width * height * 4];
			if (!SteamUtils.GetImageRGBA(handle, buffer, (int) (width * height * 4)))
			{
				return null;
			}

			// Flip the image vertically.
			// Steam gives us the image upside down and flipped, so we need to flip it back.
			for (int y = 0; y < height / 2; y++)
			{
				for (int x = 0; x < width; x++)
				{
					int y1 = y * (int) width * 4;
					int y2 = (int) (height - y - 1) * (int) width * 4;

					int i1 = y1 + x * 4;
					int i2 = y2 + x * 4;

					byte r = buffer[i1 + 0];
					byte g = buffer[i1 + 1];
					byte b = buffer[i1 + 2];
					byte a = buffer[i1 + 3];

					buffer[i1 + 0] = buffer[i2 + 0];
					buffer[i1 + 1] = buffer[i2 + 1];
					buffer[i1 + 2] = buffer[i2 + 2];
					buffer[i1 + 3] = buffer[i2 + 3];

					buffer[i2 + 0] = r;
					buffer[i2 + 1] = g;
					buffer[i2 + 2] = b;
					buffer[i2 + 3] = a;
				}
			}

			Texture2D texture = new Texture2D((int) width, (int) height, TextureFormat.RGBA32, false);
			texture.LoadRawTextureData(buffer);
			texture.Apply();
			return texture;
		}
#endif
	}
}