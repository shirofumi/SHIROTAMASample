using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public static class SinCurveGenerator
{
	#region Constants

	private const string PropertyName = "m_AnchoredPosition";

	private const int TangentModeAuto = 10;

	#endregion
	
	#region Fields

	public static readonly float AmplitudeX = 10.0f;

	public static readonly float AmplitudeY = 2.0f;

	public static readonly float FrequencyX = 5.0f;

	public static readonly float FrequencyY = 3.0f;

	public static readonly int FrameRate;

	public static readonly int Length;

	public static readonly int IntervalX;

	public static readonly int IntervalY;

	#endregion

	#region Constructors

	static SinCurveGenerator()
	{
		int scale = 1000;

		int length = scale;
		int x = (int)(FrequencyX * scale);
		int y = (int)(FrequencyY * scale);
		int gcd = GetGcd(length, GetGcd(x, y));

		length /= gcd;
		x /= gcd;
		y /= gcd;

		int lcm = GetLcm(x, y) * 4;

		FrameRate = lcm / GetGcd(lcm, length);
		Length = length;
		IntervalX = (FrameRate * length) / (x * 4);
		IntervalY = (FrameRate * length) / (y * 4);

		if (FrameRate > 120)
		{
			Debug.LogWarning("Too much frame rate:" + FrameRate);
		}
	}

	#endregion

	#region Methods

	[MenuItem("Assets/Edit/SinCurve")]
	public static void Generate()
	{
		AnimationClip clip = Selection.activeObject as AnimationClip;
		if (clip != null)
		{
			AnimationCurve curveX, curveY;
			curveX = GetCurve(AmplitudeX, IntervalX);
			curveY = GetCurve(AmplitudeY, IntervalY);

			clip.ClearCurves();
			clip.SetCurve(String.Empty, typeof(RectTransform), PropertyName + ".x", curveX);
			clip.SetCurve(String.Empty, typeof(RectTransform), PropertyName + ".y", curveY);
	
			AssetDatabase.SaveAssets();

			Debug.Log(FrameRate);
		}
		else
		{
			Debug.LogError("Select an 'Animation Clip' object.");
		}
	}

	private static AnimationCurve GetCurve(float amplitude, int interval)
	{
		int keyCount = (FrameRate * Length * 3) / (interval * 4) + 1;
		List<Keyframe> keys = new List<Keyframe>(keyCount);
		for (int i = 0; i < keyCount; i++)
		{
			int loop = i / 3;
			int offset = ((i % 3) >> 1) | (i % 3);
			int index = (loop * 4 + offset) * interval;

			float time = (float)index / (float)FrameRate;
			float value = Mathf.Sin((float)offset * Mathf.PI / 2.0f) * amplitude;

			Keyframe key = new Keyframe(time, value) { tangentMode = TangentModeAuto };

			keys.Add(key);
		}

		AnimationCurve curve = new AnimationCurve(keys.ToArray());

		return curve;
	}

	private static int GetGcd(int m, int n)
	{
		if (m < n)
		{
			int tmp = m;
			m = n;
			n = tmp;
		}

		while (n != 0)
		{
			int x = m % n;
			m = n;
			n = x;
		}

		return m;
	}

	private static int GetLcm(int m, int n)
	{
		return (m * n / GetGcd(m, n));
	}

	#endregion
}
