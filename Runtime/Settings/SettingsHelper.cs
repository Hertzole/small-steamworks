#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hertzole.SmallSteamworks
{
	internal static class SettingsHelper
	{
		private const string ROOT_FOLDER = "ProjectSettings/Packages/" + PACKAGE_NAME;
		public const string PACKAGE_NAME = "se.hertzole.small-steamworks";
		public const string SETTING_PATH = ROOT_FOLDER + "/SteamSettings.asset";

		public static void Save(SteamSettings settings)
		{
			if (!Directory.Exists(ROOT_FOLDER))
			{
				Directory.CreateDirectory(ROOT_FOLDER);
			}

			try
			{
				InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] { settings }, SETTING_PATH, true);
			}
			catch (Exception ex)
			{
				Debug.LogError("Could not save project settings!\n" + ex);
			}
		}

		public static SteamSettings Load()
		{
			SteamSettings settings = null;

			if (File.Exists(SETTING_PATH))
			{
				try
				{
					settings = (SteamSettings) InternalEditorUtility.LoadSerializedFileAndForget(SETTING_PATH)[0];
				}
				catch (Exception)
				{
					Debug.LogError("Could not load project settings. Settings will be reset.");
					settings = null;
				}
			}

			if (settings == null)
			{
				RemoveFile(SETTING_PATH);
				settings = ScriptableObject.CreateInstance<SteamSettings>();
				Save(settings);
			}

			return settings;
		}

		private static void RemoveFile(string path)
		{
			if (!File.Exists(path))
			{
				return;
			}

			FileAttributes attributes = File.GetAttributes(path);
			if ((attributes & FileAttributes.ReadOnly) != 0)
			{
				File.SetAttributes(path, attributes & ~FileAttributes.ReadOnly);
			}

			File.Delete(path);
		}
	}
}
#endif