using UnityEngine;

public class BoosterPanelControl : ActionPanelBehaviour
{
	#region Methods

	protected override void InvokeAction(BallControl ball)
	{
		if (ball.SetExcitingPoint(GameConstants.MaxExcitedLevel, true))
		{
			NewEffect(ball.transform.position);

			GameSEGlobalSource.Play(CellManager.SEs.Boost);

			Destroy(gameObject);
		}
	}

	private void NewEffect(Vector2 position)
	{
		GameObject instance = FieldEffectManager.Add(EffectStore.Instance.BoosterPanelEffect);
		Transform transform = instance.transform;
		transform.localPosition = new Vector3(position.x, position.y, transform.localPosition.z);
	}
	
	#endregion
}
