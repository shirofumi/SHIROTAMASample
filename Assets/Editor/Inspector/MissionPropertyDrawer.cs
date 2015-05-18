using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Mission))]
public class MissionPropertyDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorStyles.label.CalcHeight(label, 0.0f) * 3.0f;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		int code = property.FindPropertyRelative("Code").intValue;
		Mission mission = new Mission(code);

		float h = EditorStyles.label.CalcHeight(label, 0.0f);
		Rect r1 = new Rect(position.x, position.y + h * 0.0f, position.width, h);
		Rect r2 = new Rect(position.x, position.y + h * 1.0f, position.width, h);
		Rect r3 = new Rect(position.x, position.y + h * 2.0f, position.width, h);

		int level = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 1;
		r2 = EditorGUI.IndentedRect(r2);
		r3 = EditorGUI.IndentedRect(r3);
		EditorGUI.indentLevel = level;

		EditorGUI.LabelField(r1, label);
		EditorGUI.LabelField(r2, "Type", mission.Type.ToString());
		EditorGUI.LabelField(r3, "Option", mission.Option.ToString());
	}

}