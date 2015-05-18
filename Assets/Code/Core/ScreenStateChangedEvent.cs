using System;
using UnityEngine.Events;

[Serializable]
public class ScreenStateChangedEvent : UnityEvent<ScreenStateChangedEventData> { }

[Serializable]
public struct ScreenStateChangedEventData
{
	#region Fields

	private readonly ScreenStateChange flags;

	#endregion

	#region Properties

	public ScreenStateChange Flags
	{
		get { return flags; }
	}

	public bool WidthChanged
	{
		get { return ((flags & ScreenStateChange.Width) != 0); }
	}

	public bool HeightChanged
	{
		get { return ((flags & ScreenStateChange.Height) != 0); }
	}

	public bool ResolutionChanged
	{
		get { return ((flags & (ScreenStateChange.Width | ScreenStateChange.Height)) != 0); }
	}

	public bool OrientationChanged
	{
		get { return ((flags & ScreenStateChange.Orientation) != 0); }
	}

	public bool OrientationTypeChanged
	{
		get { return ((flags & ScreenStateChange.OrientationType) != 0); }
	}

	#endregion

	#region Constructors

	public ScreenStateChangedEventData(ScreenStateChange flags)
	{
		this.flags = flags;
	}

	#endregion
}

[Flags]
public enum ScreenStateChange
{
	None = 0x00,
	Width = 0x01,
	Height = 0x02,
	Orientation = 0x04,
	OrientationType = 0x08,

	All = Width | Height | Orientation | OrientationType,
}