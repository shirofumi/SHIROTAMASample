using UnityEngine;
using System;

public class RoughGroundControl : GroundBehaviour
{
	#region Fields

	private float prevTime;

	private static bool locked;

	#endregion

	#region Constructors

	static RoughGroundControl()
	{
		CellManager.Reset += () => locked = false;
	}

	#endregion

	#region Messages

	protected new void Start()
	{
		base.Start();

		prevTime = Single.NegativeInfinity;
	}

	protected void OnTriggerStay2D(Collider2D other)
	{
		GameObject gameObject = other.gameObject;
		if (gameObject.layer == Layers.BallContact)
		{
			ApplyEffect(GameScene.Ball);
		}
	}

	#endregion

	#region Methods

	protected void ApplyEffect(BallControl ball)
	{
		if (locked) return;

		locked = true;

		if (ball.IsOutOfControl) return;

		if (Time.time - prevTime > CellManager.Params.RoughGroundInterval)
		{
			Rigidbody2D rigidbody = ball.Rigidbody;

			float prev = rigidbody.velocity.magnitude;
			if (prev != 0.0f)
			{
				float next = Mathf.Max(0.0f, prev - CellManager.Params.RoughGroundFriction);
				if (next != 0.0f)
				{
					rigidbody.velocity *= next / prev;
				}
				else
				{
					rigidbody.velocity = Vector2.zero;

					ActionCounter.Stop();
				}

				GameSEGlobalSource.Play(CellManager.SEs.Rough);
			}

			ball.AddExcitingPoint(-CellManager.Params.RoughGroundHealingPoint);

			prevTime = Time.time;
		}
	}

	#endregion
}
