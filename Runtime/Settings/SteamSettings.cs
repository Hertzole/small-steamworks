using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hertzole.SmallSteamworks
{
	public sealed class SteamSettings : ScriptableObject, ISteamSettings
	{
		[SerializeField]
		private AppId appId = new AppId(480);
		[SerializeField]
		private bool restartAppIfNecessary = true;
		[FormerlySerializedAs("fetchCurrentStatsAutomatically")]
		[SerializeField]
		private bool fetchCurrentStatsOnBoot = true;

		public AppId AppId
		{
			get { return appId; }
			set
			{
				if (appId != value)
				{
					appId = value;
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
			instance = this;
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