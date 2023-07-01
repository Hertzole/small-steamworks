#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Steamworks;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Hertzole.SmallSteamworks
{
	internal static class SteamImageCache
	{
		private static readonly Dictionary<uint, Texture2D> cache = new Dictionary<uint, Texture2D>();
		private static readonly HashSet<uint> disposedImages = new HashSet<uint>();

		private static uint nextId;

		private static readonly Dictionary<ImageResolution, ObjectPool<Texture2D>> texturePools = new Dictionary<ImageResolution, ObjectPool<Texture2D>>();
		private static readonly Dictionary<ImageResolution, ObjectPool<byte[]>> imageBufferPools = new Dictionary<ImageResolution, ObjectPool<byte[]>>();

#if DEBUG
		private static readonly Dictionary<uint, string> imageNames = new Dictionary<uint, string>();
#endif

#if UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			CleanUp();
		}
#endif

		public static uint GetNextId()
		{
			uint result = nextId;
			nextId++;

			if (nextId == uint.MaxValue)
			{
				nextId = 0;
				disposedImages.Clear();
			}

			return result;
		}

		public static Texture2D GetTexture(uint id, int handle)
		{
			if (IsDisposed(id))
			{
				throw new ObjectDisposedException(nameof(id), "The image has been disposed.");
			}

			if (cache.TryGetValue(id, out Texture2D texture))
			{
				return texture;
			}

			texture = CreateTexture(handle);
			cache.Add(id, texture);

#if DEBUG
			if (texture != null && imageNames.TryGetValue(id, out string imageName))
			{
				texture.name = imageName;
			}
#endif

			return texture;
		}

		public static void DisposeTexture(uint id)
		{
			if (cache.TryGetValue(id, out Texture2D texture))
			{
				if (texture != null)
				{
#if DEBUG
					texture.name = $"Cached Steam Image ({texture.width}x{texture.height})";
#endif

					texturePools[new ImageResolution(texture.width, texture.height)].Release(texture);
				}

				cache.Remove(id);
			}

#if DEBUG
			imageNames.Remove(id);
#endif

			disposedImages.Add(id);
		}

		public static bool IsDisposed(uint id)
		{
			return disposedImages.Contains(id);
		}

		public static void DisposeAll()
		{
			foreach (KeyValuePair<uint, Texture2D> value in cache)
			{
				if (value.Value != null)
				{
#if DEBUG
					value.Value.name = $"Cached Steam Image ({value.Value.width}x{value.Value.height})";
#endif

					texturePools[new ImageResolution(value.Value.width, value.Value.height)].Release(value.Value);
				}
			}

			cache.Clear();
			disposedImages.Clear();

#if DEBUG
			imageNames.Clear();
#endif

			nextId = 0;
		}

		internal static void CleanUp()
		{
			foreach (Texture2D value in cache.Values)
			{
				if (value != null)
				{
					Object.Destroy(value);
				}
			}

			cache.Clear();
			disposedImages.Clear();

			foreach (ObjectPool<Texture2D> pool in texturePools.Values)
			{
				pool.Clear();
			}

			foreach (ObjectPool<byte[]> pool in imageBufferPools.Values)
			{
				pool.Clear();
			}

			texturePools.Clear();
			imageBufferPools.Clear();

#if DEBUG
			imageNames.Clear();
#endif

			nextId = 0;
		}

		[Conditional("DEBUG")]
		internal static void SetImageName(in SteamImage image, in string name)
		{
#if DEBUG
			imageNames[image.id] = name;
#endif
		}

		private static Texture2D CreateTexture(int handle)
		{
			if (!SteamUtils.GetImageSize(handle, out uint width, out uint height))
			{
				return null;
			}

			ImageResolution resolution = new ImageResolution((int) width, (int) height);

			if (!imageBufferPools.TryGetValue(resolution, out ObjectPool<byte[]> bufferPool))
			{
				// Create new values here to avoid a closure allocation with width and height.
				int newWidth = (int) width;
				int newHeight = (int) height;

				bufferPool = new ObjectPool<byte[]>(() => new byte[newWidth * newHeight * 4]);
				imageBufferPools.Add(resolution, bufferPool);
			}

			using (imageBufferPools[resolution].Get(out byte[] buffer))
			{
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

				if (!texturePools.TryGetValue(resolution, out ObjectPool<Texture2D> pool))
				{
					// Create new values here to avoid a closure allocation with width and height.
					int newWidth = (int) width;
					int newHeight = (int) height;

					pool = new ObjectPool<Texture2D>(() => new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, false), null, null,
						Object.DestroyImmediate, false);

					texturePools.Add(resolution, pool);
				}

				Texture2D texture = pool.Get();
				texture.LoadRawTextureData(buffer);
				texture.Apply();
				return texture;
			}
		}

		private readonly struct ImageResolution : IEquatable<ImageResolution>
		{
			private readonly int width;
			private readonly int height;

			public ImageResolution(int width, int height)
			{
				this.width = width;
				this.height = height;
			}

			public bool Equals(ImageResolution other)
			{
				return width == other.width && height == other.height;
			}

			public override bool Equals(object obj)
			{
				return obj is ImageResolution other && Equals(other);
			}

			public override int GetHashCode()
			{
				unchecked
				{
					return (width * 397) ^ height;
				}
			}

			public static bool operator ==(ImageResolution left, ImageResolution right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(ImageResolution left, ImageResolution right)
			{
				return !left.Equals(right);
			}

			public override string ToString()
			{
				return $"{nameof(width)}: {width}, {nameof(height)}: {height}";
			}
		}
	}
}
#endif