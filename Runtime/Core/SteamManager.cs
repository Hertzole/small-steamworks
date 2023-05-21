#nullable enable

using UnityEngine;

namespace Hertzole.SmallSteamworks
{
	/// <summary>
	///     Main class for all Steam related functionality.
	/// </summary>
	public static partial class SteamManager
	{
		/// <summary>
		///     Is Steam fully initialized and ready to be used?
		///     <para>Always returns false if DISABLESTEAMWORKS define is set.</para>
		/// </summary>
#if !DISABLESTEAMWORKS || (DISABLESTEAMWORKS && UNITY_EDITOR)
		public static bool IsInitialized
		{
			get
			{
#if !DISABLESTEAMWORKS
				return SteamManagerBehavior.instance.isInitialized;
#else
				return false;
#endif
			}
		}
#else
		public const bool IsInitialized = false;
#endif

		public static ISteamSettings Settings { get { return new SteamSettings(); } }

		/// <summary>
		///     Interface for interacting with Steam achievements.
		/// </summary>
		public static ISteamAchievements Achievements
		{
			get
			{
#if !DISABLESTEAMWORKS
				return SteamManagerBehavior.instance.achievements;
#else
				throw new SteamworksDisabledException();
#endif
			}
		}

		/// <summary>
		///     Interface for interacting with Steam stats.
		/// </summary>
		public static ISteamStats Stats
		{
			get
			{
#if !DISABLESTEAMWORKS
				return SteamManagerBehavior.instance.stats;
#else
 				throw new SteamworksDisabledException();
#endif
			}
		}

		/// <summary>
		///     Interface for interacting with Steam leaderboards.
		/// </summary>
		public static ISteamLeaderboards Leaderboards
		{
			get
			{
#if !DISABLESTEAMWORKS
				return SteamManagerBehavior.instance.leaderboards;
#else
	 			throw new SteamworksDisabledException();
#endif
			}
		}

		/// <summary>
		///     Interface for interacting with Steam storage.
		/// </summary>
		public static ISteamStorage Storage
		{
			get
			{
#if !DISABLESTEAMWORKS
				return SteamManagerBehavior.instance.storage;
#else
				throw new SteamworksDisabledException();
#endif
			}
		}

		/// <summary>
		///     Interface for interacting with Steam friends.
		/// </summary>
		public static ISteamFriends Friends
		{
			get
			{
#if !DISABLESTEAMWORKS
				return SteamManagerBehavior.instance.friends;
#else
 				throw new SteamworksDisabledException();
#endif
			}
		}

#if !DISABLESTEAMWORKS
		/// <summary>
		///     Initializes the Steam Manager object that runs all the required callbacks and initializes Steam.
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			GameObject go = new GameObject("Steam Manager", typeof(SteamManagerBehavior));
			Object.DontDestroyOnLoad(go);
		}
#endif
	}
}