using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.SmallSteamworks.Editor
{
	[CustomPropertyDrawer(typeof(AppId))]
	public sealed class AppIdDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty valueProperty = property.FindPropertyRelative("value");
			UIntField.DrawGUI(position, valueProperty, label);
		}

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			string label =
#if UNITY_2022_2_OR_NEWER
				preferredLabel;
#else
				property.displayName;
#endif

			VisualElement field = UIntField.CreatePropertyGUI(property.FindPropertyRelative("value"), label);
			field.AddToClassList(BaseField<object>.alignedFieldUssClassName);

			return field;
		}
	}
}