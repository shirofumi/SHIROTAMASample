using UnityEngine;

public abstract class PanelBehaviour : MonoBehaviour
{
	#region Fields

	public Panel Panel;

	#endregion

	#region Messages

	protected void Start()
	{
		CellControl cell = GetComponentInParent<CellControl>();
		if (cell != null)
		{
			cell.WallStateChanged += OnWallStateChanged;
		}

		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.sprite = SpriteSelector.GetPanel(Panel.Type, Panel.Option);
	}

	protected void OnDestroy()
	{
		CellControl cell = GetComponentInParent<CellControl>();
		if (cell != null)
		{
			cell.WallStateChanged -= OnWallStateChanged;
		}
	}

	protected void OnWallStateChanged(bool exist)
	{
		Collider2D collider = GetComponent<Collider2D>();
		collider.enabled = !exist;
	}

	#endregion
}
