using UnityEngine;

public class PointTable : ScriptableObject
{
	#region Fields

	public PointList[] PointLists;

	#endregion

	#region Properties

	public PointList this[int index]
	{
		get { return this.PointLists[index]; }
		set { this.PointLists[index] = value; }
	}

	#endregion
}