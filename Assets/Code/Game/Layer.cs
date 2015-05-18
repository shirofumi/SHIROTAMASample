using UnityEngine;

public class Layer : ScriptableObject
{
	#region Fields

	public Theme Theme;

	public Vector2 StartPosition;

	public int MaxBarrierCount;

	public int Points;

	public BarrierEntry[] BarrierEntries;

	public Block[] Edges;

	public Block[] Cracks;

	public Cell[] Cells;

	#endregion
}