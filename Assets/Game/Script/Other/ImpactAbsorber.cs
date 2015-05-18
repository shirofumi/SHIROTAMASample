using UnityEngine;

public class ImpactAbsorber : MonoBehaviour
{
	#region Messages

	private void OnCollisionEnter2D(Collision2D collision)
	{
		collision.rigidbody.velocity = Vector2.zero;
		collision.rigidbody.angularVelocity = 0.0f;
	}

	#endregion
}