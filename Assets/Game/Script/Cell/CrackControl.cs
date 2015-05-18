using UnityEngine;

public class CrackControl : MonoBehaviour
{
	#region Fields

	private new BoxCollider2D collider;

	#endregion

	#region Messages

	private void Awake()
	{
		this.collider = GetComponent<BoxCollider2D>();
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
		Vector2 size = collider.size;
		float rs = ball.Shape.radius;
		float rc = ball.Contact.radius;

		Vector2 pos1 = transform.position;
		Vector2 pos2 = ball.transform.position;
		
		Vector2 t, v;
		float l, min, max, d;

		l = size.x * 0.5f;
		min = l - rs;
		max = l + rc;
		d = pos1.x - pos2.x;
		t.x = Mathf.Clamp01((Mathf.Abs(d) - min) / (max - min));
		v.x = Mathf.Sign(d);

		l = size.y * 0.5f;
		min = l - rs;
		max = l + rc;
		d = pos1.y - pos2.y;
		t.y = Mathf.Clamp01((Mathf.Abs(d) - min) / (max - min));
		v.y = Mathf.Sign(d);

		if (t.x == 0.0f && t.y == 0.0f)
		{
			ball.Fall();

			GameSEGlobalSource.Play(CellManager.SEs.Fall);
		}
		else
		{
			Pull(ball, v, t);
		}
	}

	private void Pull(BallControl ball, Vector2 direction, Vector2 t)
	{
		t.x = Mathf.Sin(t.x * (Mathf.PI / 2.0f));
		t.y = Mathf.Sin(t.y * (Mathf.PI / 2.0f));

		Vector2 acceleration;
		acceleration.x = direction.x * t.x;
		acceleration.y = direction.y * t.y;
		acceleration *= CellManager.Params.CrackPanelMaxAttractiveForce;

		ball.Rigidbody.velocity += acceleration * Time.deltaTime;
	}

	#endregion
}