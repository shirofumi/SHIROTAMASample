using UnityEngine;

public class EdgeControl : MonoBehaviour
{
	#region Messages

	protected void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject other = collision.gameObject;
		if (other.layer == Layers.Ball)
		{
			GameSEGlobalSource.Play(CellManager.SEs.Bounce);
		}
	}

	#endregion
}