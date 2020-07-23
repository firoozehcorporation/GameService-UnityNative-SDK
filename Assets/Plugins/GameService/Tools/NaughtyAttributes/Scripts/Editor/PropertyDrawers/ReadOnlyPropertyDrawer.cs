using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.DrawerAttributes;
using UnityEditor;
using UnityEngine;

namespace Plugins.GameService.Tools.NaughtyAttributes.Scripts.Editor.PropertyDrawers
{
	[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
	public class ReadOnlyPropertyDrawer : PropertyDrawerBase
	{
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			return GetPropertyHeight(property);
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			GUI.enabled = false;
			EditorGUI.PropertyField(rect, property, label, true);
			GUI.enabled = true;

			EditorGUI.EndProperty();
		}
	}
}