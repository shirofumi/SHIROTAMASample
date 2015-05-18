using UnityEngine;

public class PitPanelControl : PanelBehaviour
{
	#region Fields

	private new CircleCollider2D collider;

	#endregion

	#region Messages

	private void Awake()
	{
		this.collider = GetComponent<CircleCollider2D>();
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
		float r = collider.radius;
		float rs = ball.Shape.radius;
		float rc = ball.Contact.radius;

		float min = r - rs;
		float max = r + rc;
		Vector2 v = (Vector2)transform.position - (Vector2)ball.transform.position;
		float d = v.magnitude;
		float t = Mathf.Clamp01((d - min) / (max - min));

		if (t == 0.0f)
		{
			ball.Fall();

			GameSEGlobalSource.Play(CellManager.SEs.Fall);
		}
		else
		{
			Pull(ball, v / d, t);
		}
	}

	private void Pull(BallControl ball, Vector2 direction, float t)
	{
		t = Mathf.Sin(t * (Mathf.PI / 2.0f));

		Vector2 acceleration = direction * (t * CellManager.Params.PitPanelMaxCentripetalForce);

		ball.Rigidbody.velocity += acceleration * Time.deltaTime;
	}

	#endregion
}
