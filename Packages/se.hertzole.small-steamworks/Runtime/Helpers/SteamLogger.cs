#if !DISABLESTEAMWORKS
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Hertzole.SmallSteamworks.Helpers
{
	internal readonly struct SteamLogger<T>
	{
		// ReSharper disable Unity.PerformanceAnalysis
		[Conditional("STEAMWORKS_DEBUG")]
		public void Log(string message, [CallerMemberName] string methodName = "")
		{
#if STEAMWORKS_DEBUG
			UnityEngine.Debug.Log($"[{typeof(T).Name}] {methodName} :: {message}");
#endif
		}

		// ReSharper disable Unity.PerformanceAnalysis
		[Conditional("STEAMWORKS_DEBUG")]
		public void LogWarning(string message, [CallerMemberName] string methodName = "")
		{
#if STEAMWORKS_DEBUG
			UnityEngine.Debug.LogWarning($"[{typeof(T).Name}] {methodName} :: {message}");
#endif
		}

		// ReSharper disable Unity.PerformanceAnalysis
		[Conditional("STEAMWORKS_DEBUG")]
		public void LogError(string message, [CallerMemberName] string methodName = "")
		{
#if STEAMWORKS_DEBUG
			UnityEngine.Debug.LogError($"[{typeof(T).Name}] {methodName} :: {message}");
#endif
		}
	}
}
#endif