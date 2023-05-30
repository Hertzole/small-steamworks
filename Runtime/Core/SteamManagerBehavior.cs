#if !DISABLESTEAMWORKS
using System;
using Hertzole.SmallSteamworks.Helpers;
using Steamworks;
using UnityEngine;

namespace Hertzole.SmallSteamworks
{
	public static partial class SteamManager
	{
		/// <summary>
		///     Internal behavior class that lives in the Unity scene runs all the required callbacks and initializes Steam.
		/// </summary>
		internal sealed class SteamManagerBehavior : MonoBehaviour
		{
			internal bool isInitialized;

			internal ISteamAchievements achievements;
			internal ISteamFriends friends;
			internal ISteamLeaderboards leaderboards;
			internal ISteamStats stats;
			internal ISteamStorage storage;

			private readonly SteamLogger<SteamManagerBehavior> logger = new SteamLogger<SteamManagerBehavior>();

			internal static SteamManagerBehavior instance;

			private void Awake()
			{
				if (instance != null)
				{
					Destroy(gameObject);
					return;
				}

				instance = this;

				bool error = false;

				logger.Log("Checking packsize...");
				if (!Packsize.Test())
				{
					error = true;
					Debug.LogError("Steamworks packsize test failed! The wrong version of Steamworks.NET is being run in this platform.");
				}

				logger.Log("Checking dlls...");
				if (!DllCheck.Test())
				{
					error = true;
					Debug.LogError("Steamworks DllCheck test failed! One or more of the Steamworks binaries seems to be the wrong version.");
				}

				if (error)
				{
					return;
				}

				ISteamSettings settings = Settings;

				logger.Log($"Setting Steam App Id to {settings.AppID.ToString()}");

				Environment.SetEnvironmentVariable("SteamAppId", settings.AppID.ToString());
				Environment.SetEnvironmentVariable("SteamGameId", settings.AppID.ToString());

				if (settings.RestartAppIfNecessary)
				{
					logger.Log("Restarting app if necessary...");

					try
					{
						// If Steam is not running or the game wasn't started through Steam, SteamAPI.RestartAppIfNecessary starts the
						// Steam client and also launches this game again if the User owns it. This can act as a rudimentary form of DRM.
						if (SteamAPI.RestartAppIfNecessary(settings.AppID))
						{
							Application.Quit();
							return;
						}
					}
					catch (DllNotFoundException e)
					{
						Debug.LogError($"Could not load steam_api.dll/so/dylib. It's likely not in the correct location.\n{e}");
						Application.Quit();
						return;
					}
				}

				logger.Log("Initializing Steam API...");

				bool didInitialize = SteamAPI.Init();
				if (!didInitialize)
				{
					Debug.LogError("Steam failed to initialize. This could be due to one of the following conditions:" +
					               "\n1. The Steam client is not running. A running Steam client is required to provide implementations of the various Steamworks interfaces." +
					               "\n2. The Steam client could not determine the App ID of the game." +
					               "\n3. The application is not running under the same OS user context as the Steam client, such as a different user or administration access level." +
					               "\n4. The user does not own the game." +
					               "\n5. The app is not completely set up, i.e in Release State: Unavailable, or it's missing default packages.");

					return;
				}

				achievements = new SteamAchievements();
				stats = new SteamStats(settings);
				leaderboards = new SteamLeaderboards();
				storage = new SteamStorage();
				friends = new SteamFriends();

				isInitialized = true;

				logger.Log("Steam API fully initialized!");
			}

			private void Update()
			{
				if (!isInitialized)
				{
					return;
				}

				SteamAPI.RunCallbacks();
			}

			private void OnDestroy()
			{
				if (instance == this)
				{
					instance = null;
					isInitialized = false;

					achievements.Dispose();
					stats.Dispose();
					leaderboards.Dispose();
					storage.Dispose();
					friends.Dispose();
					
					SteamImageCache.DisposeAll();

					SteamAPI.Shutdown();
				}
			}
		}
	}
}
#endif