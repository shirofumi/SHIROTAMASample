using System;
using UnityEngine;
using UnityEditor;

using UnityObject = UnityEngine.Object;

public static class AssetGenerator
{
	#region Methods

	[MenuItem("Assets/Create/ScriptableObject Asset")]
	public static void Generate()
	{
		bool done = false;

		foreach (UnityObject obj in Selection.objects)
		{
			MonoScript script = obj as MonoScript;
			if (script != null)
			{
				Type type = script.GetClass();
				if (type != null && type.IsSubclassOf(typeof(ScriptableObject)))
				{
					ScriptableObject asset = ScriptableObject.CreateInstance(type);

					string folder = AssetHelper.GetFolder(script);
					string path = AssetHelper.GetAssetPath(folder, type.Name + ".asset");
					AssetDatabase.CreateAsset(asset, path);

					done = true;
				}
			}
		}

		if (!done)
		{
			Debug.LogError("No ScriptableObject selected.");

			return;
		}

		AssetDatabase.SaveAssets();
	}

	#endregion
}