using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.SmallSteamworks.Editor
{
	internal static class UIntField
	{
		public static void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			long value = EditorGUI.LongField(position, label, property.longValue);
			if (value < uint.MinValue)
			{
				value = uint.MinValue;
			}
			else if (value > uint.MaxValue)
			{
				value = uint.MaxValue;
			}

#if UNITY_2022_1_OR_NEWER
			property.uintValue = (uint) value;
#else
			property.intValue = (int) value;
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
			UnsignedIntegerField field = new UnsignedIntegerField(label);
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

				if (evt.newValue < uint.MinValue)
				{
					args.Item1.SetValueWithoutNotify(uint.MinValue);
					value = uint.MinValue;
				}
				else if (evt.newValue > uint.MaxValue)
				{
					args.Item1.SetValueWithoutNotify(uint.MaxValue);
					value = uint.MaxValue;
				}

#if UNITY_2022_1_OR_NEWER
				args.Item2.uintValue = (uint) value;
#else
				args.Item2.intValue = (int) value;
#endif
				args.Item2.serializedObject.ApplyModifiedProperties();
			}, (longField, property));

			return longField;
		}
#endif
	}
}