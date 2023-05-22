#if !DISABLESTEAMWORKS
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Hertzole.SmallSteamworks
{
	internal static class SteamImageCache
	{
		private static readonly Dictionary<uint, Texture2D> cache = new Dictionary<uint, Texture2D>();

		private static uint nextId;

#if UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			foreach (Texture2D value in cache.Values)
			{
				if (value != null)
				{
					Object.Destroy(value);
				}
			}

			cache.Clear();

			nextId = 0;
		}
#endif

		public static uint GetNextId()
		{
			uint result = nextId;
			nextId++;
			return result;
		}

		public static Texture2D GetTexture(uint id, int handle)
		{
			if (cache.TryGetValue(id, out Texture2D texture))
			{
				return texture;
			}

			texture = CreateTexture(handle);
			cache.Add(id, texture);
			return texture;
		}

		public static void DisposeTexture(uint id)
		{
			if (cache.TryGetValue(id, out Texture2D texture))
			{
				Object.Destroy(texture);
				cache.Remove(id);
			}
		}

		private static Texture2D CreateTexture(int handle)
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
	}
}
#endif