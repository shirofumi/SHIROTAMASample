using UnityEngine;

public struct Gesture
{
	#region Fields

	public GestureType Type;

	public Vector2 Position;

	public Vector2 DeltaPosition;

	public Vector2 Velocity;

	public float DeltaTime;

	public bool Handled;

	#endregion
}

public enum GestureType
{
	None,
	Tap,
	// DoubleTap,
	// LongTap,
	// Pan,
	Flick,
}