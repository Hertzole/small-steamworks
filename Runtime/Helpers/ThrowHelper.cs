#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

#nullable enable
using System;
using System.Diagnostics;
#if NETSTANDARD2_1 || NETSTANDARD2_1_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Hertzole.SmallSteamworks.Helpers
{
	internal static class ThrowHelper
	{
		[Conditional("DEBUG")]
		public static void ThrowIfNull(
#if NETSTANDARD2_1 || NETSTANDARD2_1_OR_GREATER
			[NotNull]
#endif
			in object? obj,
			in string name)
		{
#if DEBUG
			if (obj == null)
			{
				throw new ArgumentNullException(name, $"{name} is null.");
			}
#endif
		}

		[Conditional("DEBUG")]
		public static void ThrowIfNullOrEmpty<T>(
#if NETSTANDARD2_1 || NETSTANDARD2_1_OR_GREATER
			[NotNull]
#endif
			in T[]? array,
			in string name)
		{
#if DEBUG
			if (array == null)
			{
				throw new ArgumentNullException(name, $"{name} is null.");
			}

			if (array.Length == 0)
			{
				throw new ArgumentException($"{name} is empty.", name);
			}
#endif
		}
	}
}