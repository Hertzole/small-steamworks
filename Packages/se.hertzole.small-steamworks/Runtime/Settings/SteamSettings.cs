#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Hertzole.SmallSteamworks
{
	public sealed class SteamSettings : ScriptableObject, ISteamSettings
	{
		[SerializeField]
		private AppID appID = new AppID(480);
		[SerializeField]
		private bool restartAppIfNecessary = true;
		[SerializeField]
		private bool fetchCurrentStatsOnBoot = true;
		[SerializeField] 
		private int imageCacheSize = 200;

		public AppID AppID
		{
			get { return appID; }
			set
			{
				if (appID != value)
				{
					appID = value;
					EditorSave();
				}
			}
		}

		public bool RestartAppIfNecessary
		{
			get { return restartAppIfNecessary; }
			set
			{
				if (restartAppIfNecessary != value)
				{
					restartAppIfNecessary = value;
					EditorSave();
				}
			}
		}
		public bool FetchCurrentStatsOnBoot
		{
			get { return fetchCurrentStatsOnBoot; }
			set
			{
				if (fetchCurrentStatsOnBoot != value)
				{
					fetchCurrentStatsOnBoot = value;
					EditorSave();
				}
			}
		}
		
		public int ImageCacheSize
		{
			get { return imageCacheSize; }
			set
			{
				if (imageCacheSize != value)
				{
					imageCacheSize = value;
					EditorSave();
				}
			}
		}

		private static SteamSettings instance;

		public static SteamSettings Instance
		{
			get
			{
#if UNITY_EDITOR
				if (instance != null)
				{
					return instance;
				}

				if (!File.Exists(SettingsHelper.SETTING_PATH))
				{
					instance = CreateInstance<SteamSettings>();
					SettingsHelper.Save(instance);
				}
				else
				{
					instance = SettingsHelper.Load();
				}

				instance.hideFlags = HideFlags.HideAndDontSave;
#endif
				return instance;
			}
		}

#if !UNITY_EDITOR
		private void OnEnable()
		{
#if DISABLESTEAMWORKS
			Destroy(this);
#else
			instance = this;
#endif
		}
#endif

		[Conditional("UNITY_EDITOR")]
		private void EditorSave()
		{
#if UNITY_EDITOR
			SettingsHelper.Save(this);
#endif
		}
	}
}