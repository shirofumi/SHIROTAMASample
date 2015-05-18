using UnityEngine;

public class StopperPanelControl : ActionPanelBehaviour
{
	#region Methods

	protected override void InvokeAction(BallControl ball)
	{
		if (ball.SetExcitingPoint(0.0f, true))
		{
			Rigidbody2D rigidbody = ball.Rigidbody;
			rigidbody.velocity = Vector2.zero;

			NewEffect(ball.transform.position);

			GameSEGlobalSource.Play(CellManager.SEs.Stop);

			Destroy(gameObject);

			ActionCounter.Stop();
		}
	}

	private void NewEffect(Vector2 position)
	{
		GameObject instance = FieldEffectManager.Add(EffectStore.Instance.StopperPanelEffect);
		Transform transform = instance.transform;
		transform.localPosition = new Vector3(position.x, position.y, transform.localPosition.z);
	}

	#endregion
}
