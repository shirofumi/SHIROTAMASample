using UnityEngine;
using UnityEngine.Events;

public class ScreenMonitor : SingletonMonoBehaviour<ScreenMonitor>
{
	#region Constants

	private const float DefaultDpi = 144.0f;

	private const float InchToMm = 25.4f;

	#endregion

	#region Fields

	[SerializeField]
	private bool hybridMode;

	[SerializeField]
	private ScreenStateChangedEvent screenStateChangedEvent;

	private ScreenOrientation orientation;

	private int width;

	private int height;

	private ScreenOrientation screenOrientation;

	#endregion

	#region Properties

	public static bool HybridMode
	{
		get { return Instance.hybridMode; }
		set { Instance.hybridMode = value; }
	}

	public static ScreenOrientation Orientation
	{
#if UNITY_EDITOR
		get
		{
			if (!UnityEditor.EditorApplication.isPlaying)
			{
				return ScreenOrientation.LandscapeLeft;
			}

			return Instance.orientation;
		}
#else
		get { return Instance.orientation; }
#endif
	}

	public static bool Portrait
	{
		get
		{
			ScreenOrientation orientation = Orientation;
			return (orientation == ScreenOrientation.Portrait || orientation == ScreenOrientation.PortraitUpsideDown);
		}
	}

	public static bool Landscape
	{
		get
		{
			ScreenOrientation orientation = Orientation;
			return (orientation == ScreenOrientation.LandscapeLeft || orientation == ScreenOrientation.LandscapeRight);
		}
	}

	public static float Dpi
	{
		get { return (Screen.dpi != 0.0f ? Screen.dpi : DefaultDpi); }
	}

	public static float PixelToMeter
	{
		get { return (InchToMm * 1.0e-3f / Dpi); }
	}

	public static float PixelToMillimeter
	{
		get { return (InchToMm / Dpi); }
	}

	#endregion

	#region Events

	public static event UnityAction<ScreenStateChangedEventData> ScreenStateChanged
	{
#if UNITY_EDITOR
		add { if (UnityEditor.EditorApplication.isPlaying) Instance._ScreenStateChanged += value; }
		remove { if (UnityEditor.EditorApplication.isPlaying) Instance._ScreenStateChanged -= value; }
#else
		add { Instance._ScreenStateChanged += value; }
		remove { Instance._ScreenStateChanged -= value; }
#endif
	}

	private event UnityAction<ScreenStateChangedEventData> _ScreenStateChanged
	{
		add
		{
			if (screenStateChangedEvent == null) screenStateChangedEvent = new ScreenStateChangedEvent();

			screenStateChangedEvent.AddListener(value);
		}
		remove
		{
			if (screenStateChangedEvent == null) return;

			screenStateChangedEvent.RemoveListener(value);
		}
	}

	#endregion

	#region Messages

	private new void Awake()
	{
		base.Awake();

		this.width = Screen.width;
		this.height = Screen.height;
		this.screenOrientation = GetScreenOrientation();

		this.orientation = this.screenOrientation;
	}

	private void Start()
	{
		RaiseScreenStateChangedEvent(ScreenStateChange.All);
	}

	private void Update()
	{
		ScreenStateChange flags = ScreenStateChange.None;
		UpdateResolution(ref flags);
		UpdateOrientation(ref flags);

		if (flags != ScreenStateChange.None)
		{
			RaiseScreenStateChangedEvent(flags);
		}
	}

	#endregion

	#region Methods

	private void UpdateResolution(ref ScreenStateChange flags)
	{
		int width = Screen.width;
		int height = Screen.height;

		if (this.width != width) flags |= ScreenStateChange.Width;
		if (this.height != height) flags |= ScreenStateChange.Height;

		this.width = width;
		this.height = height;
	}

	private void UpdateOrientation(ref ScreenStateChange flags)
	{
		ScreenOrientation screenOrientation = GetScreenOrientation();

		bool upsidedown;
		if (this.screenOrientation != screenOrientation)
		{
			if (OrientationIsChanged(this.orientation, screenOrientation, out upsidedown))
			{
				flags |= ScreenStateChange.Orientation;
				if (!upsidedown) flags |= ScreenStateChange.OrientationType;

				this.orientation = screenOrientation;
			}
		}
		else if (HybridMode)
		{
			ScreenOrientation deviceOrientation = ConvertDeviceOrientation(GetDeviceOrientation());
			if (OrientationIsChanged(this.orientation, deviceOrientation, out upsidedown) && upsidedown)
			{
				flags |= ScreenStateChange.Orientation;

				this.orientation = deviceOrientation;
			}
		}

		this.screenOrientation = screenOrientation;
	}

	private void RaiseScreenStateChangedEvent(ScreenStateChange flags)
	{
		if (screenStateChangedEvent != null)
		{
			ScreenStateChangedEventData data = new ScreenStateChangedEventData(flags);

			screenStateChangedEvent.Invoke(data);
		}
	}

	private static ScreenOrientation GetScreenOrientation()
	{
#if UNITY_EDITOR
		return (Screen.width >= Screen.height ? ScreenOrientation.LandscapeLeft : ScreenOrientation.Portrait);
#else
		return Screen.orientation;
#endif
	}

	private static DeviceOrientation GetDeviceOrientation()
	{
#if UNITY_EDITOR
		return (Screen.width >= Screen.height ? DeviceOrientation.LandscapeLeft : DeviceOrientation.Portrait);
#else
		return Input.deviceOrientation;
#endif
	}

	private static bool GetOrientationType(ScreenOrientation orientation, out bool portrait)
	{
		switch (orientation)
		{
			case ScreenOrientation.LandscapeLeft:
			case ScreenOrientation.LandscapeRight:
				portrait = false;
				break;

			case ScreenOrientation.Portrait:
			case ScreenOrientation.PortraitUpsideDown:
				portrait = true;
				break;

			default:
				portrait = default(bool);
				return false;
		}

		return true;
	}

	private static ScreenOrientation ConvertDeviceOrientation(DeviceOrientation orientation)
	{
		switch (orientation)
		{
			case DeviceOrientation.LandscapeLeft: return ScreenOrientation.LandscapeLeft;
			case DeviceOrientation.LandscapeRight: return ScreenOrientation.LandscapeRight;
			case DeviceOrientation.Portrait: return ScreenOrientation.Portrait;
			case DeviceOrientation.PortraitUpsideDown: return ScreenOrientation.PortraitUpsideDown;
		}

		return ScreenOrientation.Unknown;
	}

	private static bool OrientationIsChanged(ScreenOrientation o1, ScreenOrientation o2, out bool upsidedown)
	{
		bool p1, p2;
		if (o1 != o2 && GetOrientationType(o1, out p1) && GetOrientationType(o2, out p2))
		{
			upsidedown = (p1 == p2);

			return true;
		}

		upsidedown = default(bool);

		return false;
	}

	#endregion
}

