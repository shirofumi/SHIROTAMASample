using UnityEngine;

public class AccelerationPanelControl : EffectPanelBehaviour
{
	#region Properties

	public float Angle
	{
		get { return (Panel.Option * (360.0f / 8.0f)); }
	}

	#endregion

	#region Messages

	protected new void Start()
	{
		base.Start();

		Quaternion rotation = Quaternion.AngleAxis(Angle, Vector3.back);
		transform.localRotation = rotation;
	}

	#endregion

	#region Methods

	protected override void BeginEffect(BallControl ball)
	{
		AccelerationPanelEffect.Play();

		GameSEGlobalSource.Play(CellManager.SEs.Accelerate);

		ActionCounter.EnterAccelerationPanel();
	}

	protected override void ApplyEffect(BallControl ball)
	{
		if (ball.IsOutOfControl) return;

		Rigidbody2D rigidbody = ball.Rigidbody;
		
		Vector2 velocity = rigidbody.velocity;
		Quaternion rotation = Quaternion.AngleAxis(Angle, Vector3.back);
		Vector2 direction = rotation * Vector2.up;
		velocity += direction * (CellManager.Params.AccelerationPanelAcceleration * Time.deltaTime);

		rigidbody.velocity = velocity;
	}

	#endregion
}
