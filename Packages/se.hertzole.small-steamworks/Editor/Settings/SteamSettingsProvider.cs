using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.SmallSteamworks.Editor
{
	public sealed class SteamSettingsProvider : SettingsProvider
	{
		private SteamSettings settings;

		public SteamSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords) { }

		public override void OnActivate(string searchContext, VisualElement rootElement)
		{
			settings = SteamSettings.Instance;

			CreateUI(rootElement);
		}

		private void CreateUI(VisualElement rootElement)
		{
			ScrollView root = new ScrollView(ScrollViewMode.Vertical)
			{
				style =
				{
					marginLeft = 9,
					marginTop = 1
				}
			};

			root.Add(new Label(label)
			{
				style =
				{
					marginBottom = 12,
					fontSize = 19,
					unityFontStyleAndWeight = FontStyle.Bold
				}
			});

			CreateGeneralSettings(root);
			CreateAchievementsAndStatsSettings(root);
			CreateImagesSettings(root);

			rootElement.Add(root);
		}

		private void CreateGeneralSettings(VisualElement root)
		{
			LongField appIdField = CreateField<LongField, long>("App ID");
			appIdField.SetValueWithoutNotify(settings.AppID.value);

			Toggle restartAppToggle = CreateField<Toggle, bool>("Restart App If Necessary");
			restartAppToggle.SetValueWithoutNotify(settings.RestartAppIfNecessary);

			appIdField.RegisterCallback<ChangeEvent<long>, (LongField field, ISteamSettings settings)>((evt, args) =>
			{
				if (evt.newValue < uint.MinValue)
				{
					args.field.SetValueWithoutNotify(uint.MinValue);
				}
				else if (evt.newValue > uint.MaxValue)
				{
					args.field.SetValueWithoutNotify(uint.MaxValue);
				}

				args.settings.AppID = new AppID((uint) args.field.value);
			}, (appIdField, settings));

			restartAppToggle.RegisterCallback<ChangeEvent<bool>, ISteamSettings>((evt, args) => { args.RestartAppIfNecessary = evt.newValue; }, settings);

			root.Add(CreateLabel("General Settings"));
			root.Add(appIdField);
			root.Add(restartAppToggle);
		}

		private void CreateAchievementsAndStatsSettings(VisualElement root)
		{
			Toggle fetchStatsToggle = CreateField<Toggle, bool>("Fetch Current Stats On Boot");
			fetchStatsToggle.SetValueWithoutNotify(settings.FetchCurrentStatsOnBoot);

			fetchStatsToggle.RegisterCallback<ChangeEvent<bool>, ISteamSettings>((evt, args) => { args.FetchCurrentStatsOnBoot = evt.newValue; }, settings);

			root.Add(CreateLabel("Achievements & Stats"));
			root.Add(fetchStatsToggle);
		}

		private void CreateImagesSettings(VisualElement root)
		{
			IntegerField imageCacheSizeField = CreateField<IntegerField, int>("Image Cache Size");
			imageCacheSizeField.SetValueWithoutNotify(settings.ImageCacheSize);
			
			imageCacheSizeField.RegisterCallback<ChangeEvent<int>, (IntegerField field, ISteamSettings settings)>((evt, args) =>
			{
				if (evt.newValue < 0)
				{
					args.field.SetValueWithoutNotify(0);
				}
				
				args.settings.ImageCacheSize = args.field.value;
			}, (imageCacheSizeField, settings));
            
			root.Add(CreateLabel("Images"));
			root.Add(imageCacheSizeField);
		}

		private static TField CreateField<TField, TValue>(string label) where TField : BaseField<TValue>, new()
		{
			TField field = new TField
			{
				label = label
			};

			field.labelElement.style.minWidth = 250;
			field.labelElement.style.maxWidth = 250;

			return field;
		}

		private static Label CreateLabel(string text)
		{
			return new Label(text)
			{
				style =
				{
					unityFontStyleAndWeight = FontStyle.Bold,
					marginLeft = 4,
					paddingLeft = 0,
					marginTop = 6
				}
			};
		}

		[SettingsProvider]
		public static SettingsProvider CreateProvider()
		{
			return new SteamSettingsProvider("Hertzole/Small Steamworks", SettingsScope.Project)
			{
				label = "Small Steamworks",
				keywords = new[]
				{
					"steam",
					"steamworks"
				}
			};
		}
	}
}