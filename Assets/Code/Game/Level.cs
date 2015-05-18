using System.Text;
using UnityEngine;

public class Level : ScriptableObject
{
	#region Fields

	public int Index;

	public MapSummary[] Maps;

	#endregion

	#region Methods

	public static string GetResourceName(int index)
	{
		StringBuilder sb = new StringBuilder(16);
		sb.Append(ResourceConstants.LevelPath);
		sb.Append(ResourceConstants.LevelPrefix);
		sb.Append(index);
		return sb.ToString();
	}

	#endregion
}