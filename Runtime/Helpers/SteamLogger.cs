#if !DISABLESTEAMWORKS
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Debug = UnityEngine.Debug;

namespace Hertzole.SmallSteamworks.Helpers
{
	internal readonly struct SteamLogger<T> : IEquatable<SteamLogger<T>>
	{
		public bool Equals(SteamLogger<T> other)
		{
			return true;
		}

		public override bool Equals(object obj)
		{
			return obj is SteamLogger<T> other && Equals(other);
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public static bool operator ==(SteamLogger<T> left, SteamLogger<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SteamLogger<T> left, SteamLogger<T> right)
		{
			return !left.Equals(right);
		} 
		
		// ReSharper disable Unity.PerformanceAnalysis
		[Conditional("STEAMWORKS_DEBUG")]
		public void Log(string message, [CallerMemberName] string methodName = "")
		{
#if STEAMWORKS_DEBUG
			Debug.Log($"[{typeof(T).Name}] {methodName} :: {message}");
#endif
		}

		// ReSharper disable Unity.PerformanceAnalysis
		[Conditional("STEAMWORKS_DEBUG")]
		public void LogWarning(string message, [CallerMemberName] string methodName = "")
		{
#if STEAMWORKS_DEBUG
			Debug.LogWarning($"[{typeof(T).Name}] {methodName} :: {message}");
#endif
		}

		// ReSharper disable Unity.PerformanceAnalysis
		[Conditional("STEAMWORKS_DEBUG")]
		public void LogError(string message, [CallerMemberName] string methodName = "")
		{
#if STEAMWORKS_DEBUG
			Debug.LogError($"[{typeof(T).Name}] {methodName} :: {message}");
#endif
		}
	}
}
#endif