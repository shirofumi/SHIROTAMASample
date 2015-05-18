using UnityEngine;

public abstract class WallBehaviour : MonoBehaviour
{
	#region Fields

	public Wall Wall;

	#endregion

	#region Messages

	protected void Start()
	{
		RaiseWallStateChangedEvent(true);

		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.sprite = SpriteSelector.GetWall(Wall.Type, Wall.Option);	
	}

	protected void OnDestroy()
	{
		RaiseWallStateChangedEvent(false);
	}

	#endregion

	#region Methods

	protected void RaiseWallStateChangedEvent(bool exist)
	{
		CellControl cell = GetComponentInParent<CellControl>();
		if (cell != null)
		{
			cell.WallStateChangedEvent.Invoke(exist);
		}
	}

	#endregion
}
