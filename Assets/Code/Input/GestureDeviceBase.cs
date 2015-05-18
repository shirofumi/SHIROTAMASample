using System;
using UnityEngine;

public abstract class GestureDeviceBase
{
	#region Fields

	private readonly BareList<Gesture> gestures = new BareList<Gesture>();

	#endregion

	#region Properties

	public int GestureCount
	{
		get { return gestures.Count; }
	}

	protected abstract GestureDeviceParams Params { get; }

	#endregion

	#region Methods

	public void GetGesture(int index, out Gesture gesture)
	{
		if (index < 0 || index >= gestures.Count) throw new ArgumentOutOfRangeException("index");

		gesture = gestures[index];
	}

	public void SetHandled(int index, bool handled)
	{
		if (index < 0 || index >= gestures.Count) throw new ArgumentOutOfRangeException("index");

		gestures.Items[index].Handled = handled;
	}

	public abstract void Update();

	public abstract void Reset();

	protected void AddGesture(ref Gesture gesture)
	{
		gestures.Add(ref gesture);
	}

	protected void ClearGesture()
	{
		gestures.Clear();
	}

	protected virtual void DetectGesture(InputRecorder recorder, bool touched)
	{
		if (recorder.Count == 0 || touched) return;

		Vector2 startPosition = recorder.StartPosition;
		Vector2 lastPosition = recorder.LastPosition;
		float startTime = recorder.StartTime;
		float lastTime = recorder.LastTime;

		Vector2 deltaPosition = lastPosition - startPosition;
		float sqrDistance = deltaPosition.sqrMagnitude;
		if (sqrDistance < Params.MaxTapDistance * Params.MaxTapDistance)
		{
			float deltaTime = lastTime - startTime;
			if (deltaTime < Params.MaxTapDuration)
			{
				Gesture gesture;
				gesture.Type = GestureType.Tap;
				gesture.Position = startPosition;
				gesture.DeltaPosition = deltaPosition;
				gesture.Velocity = Vector2.zero;
				gesture.DeltaTime = deltaTime;
				gesture.Handled = false;

				bool cancel = false;
				OnGestureDetected(ref gesture, ref cancel);

				if (!cancel) AddGesture(ref gesture);
			}
		}

		if (recorder.Moved)
		{
			float deltaTime = lastTime - recorder.StartMovingTime;
			if (deltaTime < Params.MaxFlickDuration)
			{
				Vector2 p = recorder.GetPosition(lastTime - Params.FlickEdgeInterval);
				Vector2 velocity = (lastPosition - p) / Params.FlickEdgeInterval;
				if (velocity.sqrMagnitude > Params.MinFlickVelocity * Params.MinFlickVelocity)
				{
					Gesture gesture;
					gesture.Type = GestureType.Flick;
					gesture.Position = startPosition;
					gesture.DeltaPosition = deltaPosition;
					gesture.Velocity = velocity * Params.FlickVelocityAdjustment;
					gesture.DeltaTime = deltaTime;
					gesture.Handled = false;

					bool cancel = false;
					OnGestureDetected(ref gesture, ref cancel);

					if (!cancel) AddGesture(ref gesture);
				}
			}
		}
	}

	protected virtual void OnGestureDetected(ref Gesture gesture, ref bool cancel) { }

	#endregion
}
