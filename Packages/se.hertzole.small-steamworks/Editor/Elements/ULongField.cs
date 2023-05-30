using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.SmallSteamworks.Editor
{
	internal static class ULongField
	{
		public static void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			long value = EditorGUI.LongField(position, label, property.longValue);
			if (value < 0)
			{
				value = 0;
			}

#if UNITY_2022_1_OR_NEWER
			property.ulongValue = (ulong) value;
#else
			property.longValue = value;
#endif
		}

		public static VisualElement CreatePropertyGUI(SerializedProperty property, string label)
		{
#if UNITY_2022_2_OR_NEWER
			return CreateFieldPost2022(property, label);
#else
			return CreateFieldPre2022(property, label);
#endif
		}

#if UNITY_2022_2_OR_NEWER
		private static VisualElement CreateFieldPost2022(SerializedProperty property, string label)
		{
			UnsignedLongField field = new UnsignedLongField(label);
			field.BindProperty(property);

			return field;
		}
#endif

#if !UNITY_2022_2_OR_NEWER
		private static VisualElement CreateFieldPre2022(SerializedProperty property, string label)
		{
			LongField longField = new LongField(label);
			longField.BindProperty(property);

			longField.RegisterCallback<ChangeEvent<long>, (LongField, SerializedProperty)>((evt, args) =>
			{
				long value = evt.newValue;

				if (evt.newValue < 0)
				{
					args.Item1.SetValueWithoutNotify(0);
					value = 0;
				}

#if UNITY_2022_1_OR_NEWER
				args.Item2.ulongValue = (ulong) value;
#else
				args.Item2.longValue = value;
#endif
				args.Item2.serializedObject.ApplyModifiedProperties();
			}, (longField, property));

			return longField;
		}
#endif
	}
}