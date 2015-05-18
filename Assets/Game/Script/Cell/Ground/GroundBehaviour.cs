using UnityEngine;

public abstract class GroundBehaviour : MonoBehaviour
{
	#region Fields

	public Ground Ground;

	#endregion

	#region Messages

	protected void Start()
	{
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.sprite = SpriteSelector.GetGround(Ground.Type, Ground.Option);
	}

	#endregion
}
