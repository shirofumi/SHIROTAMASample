using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsPropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		label = EditorGUI.BeginProperty(position, label, property);

		EditorGUI.BeginChangeCheck();
		int newValue = EditorGUI.MaskField(position, label, property.intValue, property.enumDisplayNames);
		if (EditorGUI.EndChangeCheck())
		{
			property.intValue = newValue;
		}

		EditorGUI.EndProperty();
	}
}