using System;
using UnityEngine;

public static class ScreenRotation
{
	#region Fields

	private static Quaternion[] rotations = new Quaternion[4];

	#endregion

	#region Constructors

	static ScreenRotation()
	{
		rotations[0] = Quaternion.identity;
		rotations[1] = Quaternion.AngleAxis(90.0f, Vector3.forward);
		rotations[2] = Quaternion.AngleAxis(180.0f, Vector3.forward);
		rotations[3] = Quaternion.AngleAxis(270.0f, Vector3.forward);
	}

	#endregion

	#region Methods

	public static Quaternion Get(ScreenOrientation from, ScreenOrientation to)
	{
		int o1 = GetOffset(from);
		int o2 = GetOffset(to);
		int index = (o2 - o1 + 4) % 4;
		return rotations[index];
	}

	public static void Get(ScreenOrientation from, ScreenOrientation to, out Quaternion rotation)
	{
		int o1 = GetOffset(from);
		int o2 = GetOffset(to);
		int index = (o2 - o1 + 4) % 4;
		rotation = rotations[index];
	}

	private static int GetOffset(ScreenOrientation orientation)
	{
		switch (orientation)
		{
			case ScreenOrientation.LandscapeLeft: return 3;
			case ScreenOrientation.LandscapeRight: return 1;
			case ScreenOrientation.Portrait: return 0;
			case ScreenOrientation.PortraitUpsideDown: return 2;
			default:
				throw new ArgumentOutOfRangeException("orientation");
		}
	}

	#endregion
}