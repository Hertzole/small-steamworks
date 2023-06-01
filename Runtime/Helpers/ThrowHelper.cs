﻿#nullable enable
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Hertzole.SmallSteamworks.Helpers
{
	internal static class ThrowHelper
	{
		[Conditional("DEBUG")]
		public static void ThrowIfNull(
#if NETSTANDARD2_1
			[NotNull]
#endif
			object? obj,
			string name)
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
#if NETSTANDARD2_1
			[NotNull]
#endif
			T[]? array,
			string name)
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