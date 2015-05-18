using UnityEngine;

public class DirectionChangePanelControl : ActionPanelBehaviour
{
	#region Fields

	private DirectionChangePanelEffect effect;

	private static readonly float InvSqrt2 = Mathf.Sqrt(2.0f) * 0.5f;

	private static readonly Vector2[] Directions =
	{
		new Vector2(0.0f, 1.0f),
		new Vector2(InvSqrt2, InvSqrt2),
		new Vector2(1.0f, 0.0f),
		new Vector2(InvSqrt2, -InvSqrt2),
		new Vector2(0.0f, -1.0f),
		new Vector2(-InvSqrt2, -InvSqrt2),
		new Vector2(-1.0f, 0.0f),
		new Vector2(-InvSqrt2, InvSqrt2),
	};

	#endregion

	#region Properties

	public float Angle
	{
		get { return (Panel.Option * (360.0f / 8.0f)); }
	}

	public Vector2 Direction
	{
		get { return Directions[Panel.Option]; }
	}

	#endregion

	#region Messages

	protected new void Start()
	{
		base.Start();

		GameObject instance = Instantiate(EffectStore.Instance.DirectionChangePanelEffect);
		instance.transform.SetParent(this.transform, false);
		this.effect = instance.GetComponent<DirectionChangePanelEffect>();

		Quaternion rotation = Quaternion.AngleAxis(Angle, Vector3.back);
		transform.localRotation = rotation;
	}

	#endregion

	#region Methods

	protected override void InvokeAction(BallControl ball)
	{
		if (!ball.IsOutOfControl)
		{
			Rigidbody2D rigidbody = ball.Rigidbody;

			float speed = rigidbody.velocity.magnitude;
			Vector2 velocity = Direction * speed;

			rigidbody.velocity = velocity;
		}

		effect.Play();

		GameSEGlobalSource.Play(CellManager.SEs.ChangeDirection);

		ActionCounter.EnterDirectionChangePanel();
	}

	#endregion
}
