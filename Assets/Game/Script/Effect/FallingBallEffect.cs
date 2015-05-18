using System;
using UnityEngine;

public class FallingBallEffect : MonoBehaviour
{
	#region Fields

	[NonSerialized]
	public float AngularVelocity;

	[NonSerialized]
	public Vector3 Axis;

	#endregion

	#region Events

	public event Action End;

	#endregion

	#region Messages

	private void Update()
	{
		Quaternion rotation = Quaternion.AngleAxis(AngularVelocity * Time.deltaTime, Axis);
		transform.localRotation = rotation * transform.localRotation;
	}

	private void Terminate()
	{
		if (End != null) End();

		Destroy(gameObject);
	}

	#endregion
}
