using System;
using UnityEngine;

public class TouchGesture : GestureDeviceBase
{
	#region Constants

	private const int MaxTouch = 10;

	private const int RecordSize = 32;

	#endregion

	#region Fields

	private static GestureDeviceParams parameters;

	private readonly InputRecorder[] recorders = new InputRecorder[MaxTouch];

	private readonly bool[] touched = new bool[MaxTouch];

	#endregion

	#region Properties

	protected override GestureDeviceParams Params
	{
		get { return parameters; }
	}

	#endregion

	#region Constructors

	static TouchGesture()
	{
		parameters = new GestureDeviceParams();
		parameters.MinMoveDistance = 0.005f;
		parameters.MaxTapDuration = 0.25f;
		parameters.MaxTapDistance = 0.005f;
		parameters.MaxFlickDuration = 1.0f;
		parameters.MinFlickVelocity = 0.03f;
		parameters.FlickEdgeInterval = 0.25f;
		parameters.FlickVelocityAdjustment = 1.0f;
	}

	#endregion

	#region Methods

	public override void Update()
	{
		if (Time.timeScale == 0.0f)
		{
			Reset();

			return;
		}

		RecordInput();

		ClearGesture();
		for (int i = 0; i < recorders.Length; i++)
		{
			InputRecorder recorder = recorders[i];
			if (recorder != null)
			{
				DetectGesture(recorder, touched[i]);

				if (!touched[i])
				{
					recorder.Clear();
				}
			}
		}
	}

	public override void Reset()
	{
		ClearGesture();

		for (int i = 0; i < recorders.Length; i++)
		{
			InputRecorder recorder = recorders[i];
			if (recorder != null) recorder.Clear();
		}
		Array.Clear(touched, 0, touched.Length);
	}

	protected override void OnGestureDetected(ref Gesture gesture, ref bool cancel)
	{
		float m2p = 1.0f / ScreenMonitor.PixelToMeter;
		gesture.Position *= m2p;
		gesture.DeltaPosition *= m2p;
		gesture.Velocity *= m2p;
	}

	private void RecordInput()
	{
		Array.Clear(touched, 0, touched.Length);
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			if (IsTouched(touch.phase))
			{
				int index = touch.fingerId;
				if (index < MaxTouch)
				{
					if (recorders[index] == null)
					{
						recorders[index] = new InputRecorder(parameters.MinMoveDistance, RecordSize);
					}

					InputRecorder recorder = recorders[index];
					Vector2 position = touch.position * ScreenMonitor.PixelToMeter;
					float time = Time.time;
					recorder.Add(position, time);

					touched[index] = true;
				}
			}
		}
	}

	private bool IsTouched(TouchPhase phase)
	{
		return (phase == TouchPhase.Moved || phase == TouchPhase.Stationary || phase == TouchPhase.Began);
	}

	#endregion
}
