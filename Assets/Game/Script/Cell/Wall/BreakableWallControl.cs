using UnityEngine;

public class BreakableWallControl : WallBehaviour
{
	#region Messages

	protected void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject other = collision.gameObject;
		if (other.layer == Layers.Ball)
		{
			GameObject effect = Instantiate(EffectStore.Instance.BreakableWallEffect);
			effect.transform.SetParent(this.transform.parent, false);

			RaiseWallStateChangedEvent(false);

			GameSEGlobalSource.Play(CellManager.SEs.Break);

			Destroy(gameObject);

			ActionCounter.BreakWall();
		}
	}

	#endregion
}
