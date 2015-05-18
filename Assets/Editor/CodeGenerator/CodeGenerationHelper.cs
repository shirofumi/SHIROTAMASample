using UnityEditor;
using System.IO;

public static class CodeGenerationHelper
{
	#region Methods

	public static void Generate(string assetName, params ICSharpClassBuilderFragment[] fragments)
	{
		CSharpClassBuilder builder = new CSharpClassBuilder();
		string code = builder.Build(fragments);

		string path = GetDestinationPath(assetName);
		File.WriteAllText(path, code);

		AssetDatabase.Refresh();
	}

	private static string GetDestinationPath(string assetName)
	{
		string assetpath = AssetDatabase.GetAssetPath(Selection.activeObject);
		if (System.String.IsNullOrEmpty(assetpath))
		{
			assetpath = "Assets/";
		}
		assetpath = assetpath.Replace('/', Path.DirectorySeparatorChar);

		string root = Directory.GetCurrentDirectory();
		string path = Path.Combine(root, assetpath);
		string dir = Directory.Exists(path) ? path : Path.GetDirectoryName(path);

		if (!Directory.Exists(dir))
		{
			throw new FileNotFoundException("destination directory not found.", dir);
		}

		return Path.Combine(dir, assetName);
	}

	#endregion
}
