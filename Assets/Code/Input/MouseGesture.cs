using UnityEngine;

public class MouseGesture : GestureDeviceBase
{
	#region Constants

	private const int RecordSize = 32;

	#endregion

	#region Fields

	private static GestureDeviceParams parameters;

	private readonly InputRecorder recorder = new InputRecorder(parameters.MinMoveDistance, RecordSize);

	#endregion

	#region Properties

	protected override GestureDeviceParams Params
	{
		get { return parameters; }
	}

	#endregion

	#region Constructors

	static MouseGesture()
	{
		parameters = new GestureDeviceParams();
		parameters.MinMoveDistance = 0.005f;
		parameters.MaxTapDuration = 0.75f;
		parameters.MaxTapDistance = 0.005f;
		parameters.MaxFlickDuration = 1.0f;
		parameters.MinFlickVelocity = 0.05f;
		parameters.FlickEdgeInterval = 0.25f;
		parameters.FlickVelocityAdjustment = 0.5f;
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

		bool pressed = Record();

		ClearGesture();
		DetectGesture(recorder, pressed);

		if (!pressed)
		{
			recorder.Clear();
		}
	}

	public override void Reset()
	{
		ClearGesture();

		recorder.Clear();
	}

	protected override void OnGestureDetected(ref Gesture gesture, ref bool cancel)
	{
		float m2p = 1.0f / ScreenMonitor.PixelToMeter;
		gesture.Position *= m2p;
		gesture.DeltaPosition *= m2p;
		gesture.Velocity *= m2p;
	}

	private bool Record()
	{
		bool pressed = Input.GetMouseButton(0);
		if (pressed)
		{
			Vector2 position = Input.mousePosition * ScreenMonitor.PixelToMeter;
			float time = Time.time;
			recorder.Add(position, time);
		}

		return pressed;
	}

	#endregion
}
