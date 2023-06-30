#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
#nullable enable
using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine.Pool;

namespace Hertzole.SmallSteamworks.Helpers
{
	internal enum CallbackType
	{
		Callback = 0,
		CallResult = 1
	}

	internal sealed class SteamCallback<T> : IDisposable where T : struct
	{
		private readonly Callback<T>? callback;
		private readonly CallResult<T>? callResult;

		private readonly Action<T>? onCallback;
		private readonly Predicate<T>? globalPredicate;

		private List<CallbackRegistration>? registrations;

		public SteamCallback(in CallbackType type, in Predicate<T>? globalPredicate = null)
		{
			this.globalPredicate = globalPredicate;

			switch (type)
			{
				case CallbackType.Callback:
					callback = new Callback<T>(OnCallback);
					break;
				case CallbackType.CallResult:
					callResult = new CallResult<T>();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		public SteamCallback(in CallbackType type, in Action<T> onCallback, in Predicate<T>? globalPredicate = null) : this(type, globalPredicate)
		{
			this.onCallback = onCallback;
		}

		public void RegisterOnce(in Action<T> onCalled, in Predicate<T>? predicate = null)
		{
			registrations ??= new List<CallbackRegistration>();

			CallbackRegistration? registration = GenericPool<CallbackRegistration>.Get();
			registration.callback = onCalled;
			registration.predicate = predicate;

			registrations.Add(registration);
		}

		public void RegisterOnce(in SteamAPICall_t call, CallResultCallback<T> onCalled)
		{
			callResult?.Set(call, (param, failure) => { onCalled(param, failure); });
		}

		private void OnCallback(T param)
		{
			if (globalPredicate != null && !globalPredicate(param))
			{
				return;
			}

			onCallback?.Invoke(param);

			if (registrations != null)
			{
				int count = registrations.Count;
				for (int i = 0; i < count; i++)
				{
					CallbackRegistration registration = registrations[i];
					if (registration.predicate == null || registration.predicate(param))
					{
						registration.callback?.Invoke(param);
						GenericPool<CallbackRegistration>.Release(registration);
						registrations.RemoveAt(i);
						i--;
						count--;
					}
				}
			}
		}

		public void Dispose()
		{
			callback?.Dispose();
			callResult?.Dispose();
		}

		private sealed class CallbackRegistration
		{
			public Action<T>? callback;
			public Predicate<T>? predicate;
		}
	}
}
#endif