using System;
using UnityEngine;

using UnityObject = UnityEngine.Object;

public class CellManager : SingletonMonoBehaviour<CellManager>
{
	#region Fields

	public GameObject Cell;

	[SerializeField]
	private CellParams parameters;

	[SerializeField]
	private CellSEs ses;

	#endregion

	#region Properties

	public static CellParams Params
	{
		get { return Instance.parameters; }
	}

	public static CellSEs SEs
	{
		get { return Instance.ses; }
	}

	#endregion

	#region Events

	public static event Action Reset;

	#endregion

	#region Messages

	private void Start()
	{
		InitMap();
	}

	private void FixedUpdate()
	{
		if (Reset != null) Reset();
	}

	#endregion

	#region Methods

	public static Vector3 CellToWorld(Vector2 position, float z)
	{
		Map map = GameScene.Map;

		Vector3 pos;
		pos.x = position.x - (map.Width - 1) * 0.5f;
		pos.y = -position.y + (map.Height - 1) * 0.5f;
		pos.z = z;

		return pos;
	}

	private void InitMap()
	{
		Map map = GameScene.Map;
		for (int index = 0, size = map.Width * map.Height; index < size; index++)
		{
			int x = index % map.Width;
			int y = index / map.Width;
			float px = (x * 2 - map.Width + 1) * +0.5f;
			float py = (y * 2 - map.Height + 1) * -0.5f;

			GameObject cellObj = (GameObject)UnityObject.Instantiate(Cell, new Vector3(px, py, 0), Quaternion.identity);
			cellObj.transform.SetParent(transform, false);

			CellControl cell = cellObj.GetComponent<CellControl>();
			cell.Index = index;
		}
	}

	#endregion
}