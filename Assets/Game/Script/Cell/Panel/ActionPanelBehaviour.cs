using System;
using UnityEngine;

public abstract class ActionPanelBehaviour : PanelBehaviour
{
	#region Fields

	private float startTime;

	#endregion

	#region Messages

	protected void OnTriggerEnter2D(Collider2D other)
	{
		GameObject gameObject = other.gameObject;
		if (gameObject.layer == Layers.BallContact)
		{
			startTime = Time.time;
		}
	}

	protected void OnTriggerStay2D(Collider2D other)
	{
		GameObject gameObject = other.gameObject;
		if (gameObject.layer == Layers.BallContact)
		{
			if (Time.time - startTime > CellManager.Params.ActionPanelDelayTime)
			{
				InvokeAction(GameScene.Ball);

				startTime = Single.PositiveInfinity;
			}
		}
	}

	#endregion

	#region Methods

	protected abstract void InvokeAction(BallControl ball);

	#endregion
}
