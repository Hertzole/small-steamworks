#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
// The reason we don't use DISABLESTEAMWORKS here is because sometimes in CI, DISABLESTEAMWORKS may not be removed if it was present and should be removed.
// We only disable the inclusion of the build if it's on a platform that doesn't support Steamworks.
#define DISABLE_BUILD
#endif

using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#if !DISABLE_BUILD
using System.Linq;
using UnityEditor;
using UnityEngine;
#endif

#if !DISABLESTEAMWORKS
using Hertzole.SmallSteamworks.Helpers;
#endif

namespace Hertzole.SmallSteamworks.Editor
{
	internal sealed class SteamSettingsBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
	{
#if !DISABLE_BUILD
		private bool removeFromPreloadedAssets;

		private SteamSettings settingsInstance;

		private const string STEAM_SETTINGS_PATH = "Assets/" + SettingsHelper.PACKAGE_NAME + "_SteamSettings.asset";
#endif

#if !DISABLESTEAMWORKS && !DISABLE_BUILD
		private static readonly SteamLogger<SteamSettingsBuildProcessor> logger = new SteamLogger<SteamSettingsBuildProcessor>();
#endif

		public int callbackOrder
		{
			get { return -1_000_000; }
		}

		public void OnPreprocessBuild(BuildReport report)
		{
#if !DISABLE_BUILD
			Application.logMessageReceivedThreaded += OnGetLog;

			removeFromPreloadedAssets = false;

			SteamSettings oldInstance = AssetDatabase.LoadAssetAtPath<SteamSettings>(STEAM_SETTINGS_PATH);

			if (oldInstance != null)
			{
				Log("Found old instance, removing it");
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(oldInstance));
			}

			SteamSettings.Instance.hideFlags = HideFlags.None;

			AssetDatabase.CreateAsset(SteamSettings.Instance, STEAM_SETTINGS_PATH);
			AssetDatabase.ImportAsset(STEAM_SETTINGS_PATH);

			Log("Created new instance");

			settingsInstance = AssetDatabase.LoadAssetAtPath<SteamSettings>(STEAM_SETTINGS_PATH);

			Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();
			bool wasDirty = IsPlayerSettingsDirty();

			if (!preloadedAssets.Contains(settingsInstance))
			{
				Log("Adding to preloaded assets");

				ArrayUtility.Add(ref preloadedAssets, settingsInstance);
				PlayerSettings.SetPreloadedAssets(preloadedAssets);

				removeFromPreloadedAssets = true;

				if (!wasDirty)
				{
					ClearPlayerSettingsDirtyFlag();
				}
			}

			EditorBuildSettings.AddConfigObject(SettingsHelper.PACKAGE_NAME, settingsInstance, true);
#endif
		}

		public void OnPostprocessBuild(BuildReport report)
		{
#if !DISABLE_BUILD
			Application.logMessageReceivedThreaded -= OnGetLog;

			RemoveInstance();
#endif
		}

		[Conditional("STEAMWORKS_DEBUG")]
		private static void Log(string message, [CallerMemberName] string methodName = "")
		{
#if !DISABLESTEAMWORKS
            logger.Log(message, methodName);
#endif
		}

#if !DISABLE_BUILD
		private void RemoveInstance()
		{
			if (removeFromPreloadedAssets)
			{
				Log("Removing from preloaded assets");

				bool wasDirty = IsPlayerSettingsDirty();

				Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();
				ArrayUtility.Remove(ref preloadedAssets, settingsInstance);
				PlayerSettings.SetPreloadedAssets(preloadedAssets);

				if (!wasDirty)
				{
					ClearPlayerSettingsDirtyFlag();
				}
			}

			if (EditorBuildSettings.TryGetConfigObject<SteamSettings>(SettingsHelper.PACKAGE_NAME, out _))
			{
				Log("Removing from build settings");
				EditorBuildSettings.RemoveConfigObject(SettingsHelper.PACKAGE_NAME);
			}

			if (settingsInstance != null)
			{
				Log("Removing asset");
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(settingsInstance));
			}

			settingsInstance = null;
		}

		private void OnBuildError()
		{
			RemoveInstance();
		}

		private void OnGetLog(string condition, string stacktrace, LogType type)
		{
			if (type == LogType.Error || type == LogType.Exception)
			{
				Application.logMessageReceivedThreaded -= OnGetLog;
				OnBuildError();
			}
		}

		private static bool IsPlayerSettingsDirty()
		{
			PlayerSettings[] settings = Resources.FindObjectsOfTypeAll<PlayerSettings>();
			if (settings != null && settings.Length > 0)
			{
				return EditorUtility.IsDirty(settings[0]);
			}

			return false;
		}

		private static void ClearPlayerSettingsDirtyFlag()
		{
			PlayerSettings[] settings = Resources.FindObjectsOfTypeAll<PlayerSettings>();
			if (settings != null && settings.Length > 0)
			{
				EditorUtility.ClearDirty(settings[0]);
			}
		}
#endif
	}
}