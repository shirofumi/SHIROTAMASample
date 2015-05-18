using System;
using UnityEngine;

public class ScreenSwitcher : MonoBehaviour
{
	#region Screen

	private enum Screen
	{
		Unknown,
		Start,
		Pause,
		Failed,
		Result,
	}

	#endregion

	#region Fields

	private Transform[] screens;

	#endregion

	#region Messages

	private void OnEnable()
	{
		GamePhaseManager.GamePhaseChanged += OnGamePhaseChanged;
	}

	private void OnDisable()
	{
		if (GamePhaseManager.IsAlive) GamePhaseManager.GamePhaseChanged -= OnGamePhaseChanged;
	}

	private void Awake()
	{
		int count = Enum.GetValues(typeof(Screen)).Length;
		
		screens = new Transform[count - 1];
		SetScreen(Screen.Start, transform.Find("StartScreen"));
		SetScreen(Screen.Pause, transform.Find("PauseScreen"));
		SetScreen(Screen.Failed, transform.Find("FailedScreen"));
		SetScreen(Screen.Result, transform.Find("ResultScreen"));

		for (int i = 0; i < screens.Length; i++)
		{
			screens[i].gameObject.SetActive(false);
		}
	}

	#endregion

	#region Methods

	private Transform GetScreen(Screen screen)
	{
		return this.screens[(int)screen - 1];
	}

	private void SetScreen(Screen screen, Transform value)
	{
		this.screens[(int)screen - 1] = value;
	}

	private void OnGamePhaseChanged(GamePhaseChangedEventData data)
	{
		Screen prev = PhaseToScreen(data.Previous);
		if (prev != Screen.Unknown)
		{
			GetScreen(prev).gameObject.SetActive(false);
		}

		Screen next = PhaseToScreen(data.Next);
		if (next != Screen.Unknown)
		{
			GetScreen(next).gameObject.SetActive(true);
		}
	}

	private static Screen PhaseToScreen(GamePhase phase)
	{
		switch (phase)
		{
			case GamePhase.Beginning: return Screen.Start;
			case GamePhase.Result: return Screen.Result;
			case GamePhase.Failed: return Screen.Failed;
			case GamePhase.Pause: return Screen.Pause;
		}

		return Screen.Unknown;
	}

	#endregion
}
