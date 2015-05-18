using UnityEngine;

public class RotationPanelControl : EffectPanelBehaviour
{
	#region Fields

	public RotationPanelDirection Direction;

	private bool rotating;

	private new AudioSource audio;

	#endregion

	#region Messages

	protected void Awake()
	{
		this.audio = GetComponent<AudioSource>();
	}

	protected void Update()
	{
		if (rotating)
		{
			Quaternion rotation = GetRotation(Time.deltaTime);
			transform.localRotation = rotation * transform.localRotation;
		}
	}

	#endregion

	#region Methods

	protected override void BeginEffect(BallControl ball)
	{
		rotating = true;

		audio.clip = CellManager.SEs.Rotate;
		audio.Play();

		ActionCounter.EnterRotationPanel();
	}

	protected override void EndEffect(BallControl ball)
	{
		rotating = false;

		audio.Stop();
	}

	protected override void ApplyEffect(BallControl ball)
	{
		if (ball.IsOutOfControl) return;

		Rigidbody2D rigidbody = ball.Rigidbody;

		Quaternion rotation = GetRotation(Time.deltaTime);
		
		Vector2 position = rigidbody.position;
		Vector2 center = transform.position;
		position = center + (Vector2)(rotation * (position - center));
		rigidbody.position = position;

		Vector2 velocity = rigidbody.velocity;
		velocity = rotation * velocity;
		rigidbody.velocity = velocity;
	}

	private Quaternion GetRotation(float time)
	{
		float angle = CellManager.Params.RotationPanelAngularVelocity * time;

		Vector3 axis;
		if (Direction == RotationPanelDirection.CW)
		{
			axis = Vector3.back;
		}
		else if (Direction == RotationPanelDirection.CCW)
		{
			axis = Vector3.forward;
		}
		else
		{
			axis = Vector3.zero;
		}

		return Quaternion.AngleAxis(angle, axis);
	}

	#endregion
}

#region RotationPanelDirection

public enum RotationPanelDirection
{
	None,
	CW,
	CCW,
}

#endregion
