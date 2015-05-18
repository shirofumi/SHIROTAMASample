using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
	#region Fields

	private readonly List<GestureDeviceBase> devices = new List<GestureDeviceBase>();

	private int[] offsets;

	#endregion

	#region Properties

	public static int GestureCount
	{
		get
		{
			var @this = Instance;
			return @this.offsets[@this.offsets.Length - 1];
		}
	}

	#endregion

	#region Messages

	private void OnEnable()
	{
		ScreenMonitor.ScreenStateChanged += OnScreenStateChanged;
	}

	private void OnDisable()
	{
		if (ScreenMonitor.IsAlive) ScreenMonitor.ScreenStateChanged -= OnScreenStateChanged;
	}

	private new void Awake()
	{
		base.Awake();

		Input.simulateMouseWithTouches = false;

		devices.Add(new MouseGesture());
#if UNITY_EDITOR
		// for Unity Remote
		devices.Add(new TouchGesture());
#else
		if (Input.touchSupported) devices.Add(new TouchGesture());
#endif

		offsets = new int[devices.Count + 1];
	}

	private void Update()
	{
		for (int i = 0; i < devices.Count; i++)
		{
			GestureDeviceBase device = devices[i];

			device.Update();
			offsets[i + 1] = offsets[i] + device.GestureCount;
		}
	}

	#endregion

	#region Methods

	public static void GetGesture(int index, out Gesture gesture)
	{
		Instance._GetGesture(index, out gesture);
	}

	public static void SetHandled(int index, bool handled)
	{
		Instance._SetHandled(index, handled);
	}

	public static void Reset()
	{
		Instance._Reset();
	}

	private void _GetGesture(int index, out Gesture gesture)
	{
		int i = GetDeviceIndex(index);
		if (i < 0 || i >= offsets.Length) throw new ArgumentOutOfRangeException("index");

		GestureDeviceBase device = devices[i];
		int n = offsets[i] - index;
		device.GetGesture(n, out gesture);
	}

	private void _SetHandled(int index, bool handled)
	{
		int i = GetDeviceIndex(index);
		if (i < 0 || i >= offsets.Length) throw new ArgumentOutOfRangeException("index");

		GestureDeviceBase device = devices[i];
		int n = offsets[i] - index;
		device.SetHandled(n, handled);
	}

	private void _Reset()
	{
		for (int i = 0; i < devices.Count; i++)
		{
			devices[i].Reset();
		}
	}

	private int GetDeviceIndex(int gestureIndex)
	{
		int index = Array.BinarySearch<int>(offsets, gestureIndex);
		if (index < 0) index = ~index - 1;
		return index;
	}

	private void OnScreenStateChanged(ScreenStateChangedEventData data)
	{
		_Reset();
	}

	#endregion
}