using System.Linq;
using Hertzole.SmallSteamworks.Helpers;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Hertzole.SmallSteamworks.Editor
{
	internal sealed class SteamSettingsBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
	{
		private bool removeFromPreloadedAssets;

		private SteamSettings settingsInstance;

		private static readonly SteamLogger<SteamSettingsBuildProcessor> logger = new SteamLogger<SteamSettingsBuildProcessor>();

		public int callbackOrder
		{
			get { return -1_000_000; }
		}

		private const string STEAM_SETTINGS_PATH = "Assets/" + SettingsHelper.PACKAGE_NAME + "_SteamSettings.asset";

		public void OnPreprocessBuild(BuildReport report)
		{
			Application.logMessageReceivedThreaded += OnGetLog;

			removeFromPreloadedAssets = false;

			SteamSettings oldInstance = AssetDatabase.LoadAssetAtPath<SteamSettings>(STEAM_SETTINGS_PATH);

			if (oldInstance != null)
			{
				logger.Log("Found old instance, removing it");
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(oldInstance));
			}

			SteamSettings.Instance.hideFlags = HideFlags.None;

			AssetDatabase.CreateAsset(SteamSettings.Instance, STEAM_SETTINGS_PATH);
			AssetDatabase.ImportAsset(STEAM_SETTINGS_PATH);

			logger.Log("Created new instance");

			settingsInstance = AssetDatabase.LoadAssetAtPath<SteamSettings>(STEAM_SETTINGS_PATH);

			Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();
			bool wasDirty = IsPlayerSettingsDirty();

			if (!preloadedAssets.Contains(settingsInstance))
			{
				logger.Log("Adding to preloaded assets");

				ArrayUtility.Add(ref preloadedAssets, settingsInstance);
				PlayerSettings.SetPreloadedAssets(preloadedAssets);

				removeFromPreloadedAssets = true;

				if (!wasDirty)
				{
					ClearPlayerSettingsDirtyFlag();
				}
			}

			EditorBuildSettings.AddConfigObject(SettingsHelper.PACKAGE_NAME, settingsInstance, true);
		}

		public void OnPostprocessBuild(BuildReport report)
		{
			Application.logMessageReceivedThreaded -= OnGetLog;

			RemoveInstance();
		}

		private void RemoveInstance()
		{
			if (removeFromPreloadedAssets)
			{
				logger.Log("Removing from preloaded assets");

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
				logger.Log("Removing from build settings");
				EditorBuildSettings.RemoveConfigObject(SettingsHelper.PACKAGE_NAME);
			}

			if (settingsInstance != null)
			{
				logger.Log("Removing asset");
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
	}
}