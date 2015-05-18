using System;
using System.IO;
using UnityEditor;

using UnityObject = UnityEngine.Object;

public static class AssetHelper
{
	#region Methods

	public static string GetAssetPath(string folder, string name)
	{
		return GetAssetPath(folder, name, false);
	}

	public static string GetAssetPath(string folder, string name, bool unique)
	{
		if (!folder.EndsWith('/'.ToString()))
		{
			folder += '/';
		}

		string assetpath = folder + name;
		if(unique) assetpath = AssetDatabase.GenerateUniqueAssetPath(assetpath);

		return assetpath;
	}

	public static string GetCurrentFolder()
	{
		return GetFolder(Selection.activeObject);
	}

	public static string GetFolder(UnityObject obj)
	{
		string assetpath = AssetDatabase.GetAssetPath(obj);
		if (System.String.IsNullOrEmpty(assetpath))
		{
			assetpath = "Assets/";
		}

		string path = ToSystemPath(assetpath);
		string dir = Directory.Exists(path) ? path : Path.GetDirectoryName(path);

		if (!Directory.Exists(dir))
		{
			throw new FileNotFoundException("Directory not found.", dir);
		}

		return FromSystemPath(dir);
	}

	public static string ToSystemPath(string assetPath)
	{
		return Path.Combine(Directory.GetCurrentDirectory(), assetPath.Replace('/', Path.DirectorySeparatorChar));
	}

	public static string FromSystemPath(string systemPath)
	{
		return GetRelativePath(systemPath, Directory.GetCurrentDirectory()).Replace(Path.DirectorySeparatorChar, '/');
	}

	private static string GetRelativePath(string fullpath, string basepath)
	{
		if (!basepath.EndsWith(Path.DirectorySeparatorChar.ToString()))
		{
			basepath += Path.DirectorySeparatorChar;
		}

		Uri uri = new Uri(fullpath);
		Uri baseuri = new Uri(basepath);

		string relative = baseuri.MakeRelativeUri(uri).ToString();
		relative = relative.Replace('/', Path.DirectorySeparatorChar);
		relative = Uri.UnescapeDataString(relative);

		return relative;
	}

	#endregion
}