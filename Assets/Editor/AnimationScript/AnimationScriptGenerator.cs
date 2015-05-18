using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

using UnityObject = UnityEngine.Object;

public static class AnimationScriptGenerator
{
	#region Methods

	[MenuItem("Assets/Create/Animation Script")]
	public static void Generate()
	{
		Dictionary<string, MonoScript> scripts = new Dictionary<string, MonoScript>();
		List<AnimatorController> controllers = new List<AnimatorController>();

		foreach (UnityObject obj in Selection.objects)
		{
			MonoScript script = obj as MonoScript;
			if (script != null)
			{
				scripts.Add(script.name, script);
			}

			AnimatorController controller = obj as AnimatorController;
			if (controller != null)
			{
				controllers.Add(controller);
			}
		}

		if (controllers.Count != 0)
		{
			foreach (AnimatorController source in controllers)
			{
				CSharpClassBuilder builder = new CSharpClassBuilder();
				AnimationScriptBuilderFragment fragment = new AnimationScriptBuilderFragment(source);
				string code = builder.Build(fragment);

				MonoScript script;
				UnityObject asset = scripts.TryGetValue(source.name, out script) ? (UnityObject)script : (UnityObject)source;
				string folder = AssetHelper.GetFolder(asset);
				string path = AssetHelper.GetAssetPath(folder, source.name + ".cs");
				string systemPath = AssetHelper.ToSystemPath(path);
				File.WriteAllText(systemPath, code);

				AssetDatabase.Refresh();
			}
		}
		else
		{
			Debug.LogError("No AnimatorController selected.");
		}
	}

	#endregion
}