using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public static class ConstantClassGenerator
{
	#region Constants

	public const string FileName = "UnityConstants.cs";

	#endregion

	#region Methods

	[MenuItem("Assets/Create/" + FileName)]
	public static void Generate()
	{
		CodeGenerationHelper.Generate(FileName, new[]{
			new NameConstantBuilderFragment("Scenes", GetSceneNames()),
			new NameToValueConstantBuilderFragment("SceneNumbers", GetSceneTable()),
			new NameConstantBuilderFragment("Tags", GetTagNames()),
			new NameConstantBuilderFragment("SortingLayers", GetSortingLayerNames()),
			new NameToValueConstantBuilderFragment("Layers", GetLayerTable()),
		});
	}

	private static string[] GetSceneNames()
	{
		return EditorBuildSettings.scenes.Select(x => Path.GetFileNameWithoutExtension(x.path)).ToArray();
	}

	private static Dictionary<string, int> GetSceneTable()
	{
		return EditorBuildSettings.scenes.Select((x, i) => new { Name = Path.GetFileNameWithoutExtension(x.path), Value = i }).ToDictionary(x => x.Name, x => x.Value);
	}

	private static string[] GetTagNames()
	{
		return InternalEditorUtility.tags;
	}

	private static string[] GetSortingLayerNames()
	{
		System.Type type = typeof(InternalEditorUtility);
		PropertyInfo prop = type.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		return (string[]) prop.GetValue(null, null);
	}

	private static int[] GetSortingLayerIDs()
	{
		System.Type type = typeof(InternalEditorUtility);
		PropertyInfo prop = type.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
		return (int[])prop.GetValue(null, null);
	}

	private static Dictionary<string, int> GetLayerTable()
	{
		return InternalEditorUtility.layers.ToDictionary(x => x, x => LayerMask.NameToLayer(x));
	}

	#endregion
}
