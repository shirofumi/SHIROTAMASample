using UnityEngine;

public abstract class EffectPanelBehaviour : PanelBehaviour
{
	#region Fields

	private bool inEffect;

	#endregion

	#region Properties

	protected bool InEffect { get { return inEffect; } }

	#endregion

	#region Messages

	protected void FixedUpdate()
	{
		if (inEffect && GamePhaseManager.Phase != GamePhase.Running)
		{
			EndEffect(GameScene.Ball);

			inEffect = false;
		}
	}

	protected void OnTriggerEnter2D(Collider2D other)
	{
		if (!inEffect)
		{
			GameObject gameObject = other.gameObject;
			if (gameObject.layer == Layers.BallContact)
			{
				BeginEffect(GameScene.Ball);

				inEffect = true;
			}
		}
	}

	protected void OnTriggerExit2D(Collider2D other)
	{
		if (inEffect)
		{
			GameObject gameObject = other.gameObject;
			if (gameObject.layer == Layers.BallContact)
			{
				EndEffect(GameScene.Ball);

				inEffect = false;
			}
		}
	}

	protected void OnTriggerStay2D(Collider2D other)
	{
		if (inEffect)
		{
			GameObject gameObject = other.gameObject;
			if (gameObject.layer == Layers.BallContact)
			{
				ApplyEffect(GameScene.Ball);
			}
		}
	}

	#endregion

	#region Methods

	protected virtual void BeginEffect(BallControl ball) { }

	protected virtual void EndEffect(BallControl ball) { }

	protected abstract void ApplyEffect(BallControl ball);

	#endregion
}
