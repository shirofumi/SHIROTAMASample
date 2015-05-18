using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BreakableWallEffect : MonoBehaviour
{
	#region Messages

	private void Start()
	{
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.sprite = SpriteSelector.GetWall(WallType.Breakable, 2);
	}

	private void Terminate()
	{
		Destroy(gameObject);
	}

	#endregion
}