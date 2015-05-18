using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

using Table = System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, int>>;

public static class PointTableCalculator
{
	#region Constants

	public const string AssetName = "PointTable";

	#endregion

	#region Methods

	[MenuItem("Assets/Create/PointTable")]
	public static void Generate()
	{
		Table table = new Table();

		string res = ResourceConstants.MapPath;
		res = res.Substring(0, res.Length - 1);

		Map[] maps = Resources.LoadAll<Map>(res);
		foreach (Map map in maps)
		{
			SetPoint(table, map);
		}

		string dir = AssetHelper.GetCurrentFolder();
		string path = AssetHelper.GetAssetPath(dir, AssetName + ".asset", false);

		PointTable asset = ToPointTable(table);

		Object current = AssetDatabase.LoadMainAssetAtPath(path);
		if (current != null)
		{
			EditorUtility.CopySerialized(asset, current);
		}
		else
		{
			AssetDatabase.CreateAsset(asset, path);
		}
		AssetDatabase.SaveAssets();
	}

	private static void SetPoint(Table table, Map map)
	{
		MapID id = map.ID;
		int levelIndex = id.Primary - 1;
		int mapIndex = id.Secondary - 1;

		Dictionary<int, int> points;
		if (!table.TryGetValue(levelIndex, out points))
		{
			table.Add(levelIndex, points = new Dictionary<int, int>());
		}

		points.Add(mapIndex, map.Points);
	}

	private static PointTable ToPointTable(Table table)
	{
		PointTable result = ScriptableObject.CreateInstance<PointTable>();

		PointList[] points = new PointList[table.Keys.Max() + 1];
		foreach (var level in table)
		{
			PointList pts = points[level.Key] = new int[level.Value.Keys.Max() + 1];
			foreach (var map in level.Value)
			{
				pts[map.Key] = map.Value;
			}
		}
		result.PointLists = points;

		return result;
	}

	#endregion
}