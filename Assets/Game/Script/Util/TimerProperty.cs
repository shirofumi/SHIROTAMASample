using System;
using UnityEngine;

[Serializable]
public struct TimerProperty
{
	#region Fields

	private float startTime;

	private bool state;

	#endregion

	#region Properties

	public float StartTime
	{
		get { return startTime; }
	}

	public float ElapsedTime
	{
		get { return (Time.time - startTime); }
	}

	public bool State
	{
		get { return state; }
	}

	#endregion

	#region Methods

	public void Start()
	{
		Start(false);
	}

	public void Start(bool force)
	{
		if (!state || force)
		{
			startTime = Time.time;
			state = true;
		}
	}

	public bool Update(float duration)
	{
		if (state && Time.time - startTime > duration)
		{
			state = false;

			return true;
		}

		return false;
	}

	#endregion
}