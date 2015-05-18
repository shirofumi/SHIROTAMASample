
public class PauseManager : SingletonMonoBehaviour<PauseManager>
{
	#region Messages

	private void OnEnable()
	{
		ScreenMonitor.ScreenStateChanged += OnScreenStateChanged;
	}

	private void OnDisable()
	{
		if (ScreenMonitor.IsAlive) ScreenMonitor.ScreenStateChanged -= OnScreenStateChanged;
	}

	private void OnApplicationPause(bool pauseState)
	{
		if (pauseState)
		{
			GamePhaseManager.Push(GamePhase.Pause);
		}
	}

	#endregion

	#region Methods

	private void OnScreenStateChanged(ScreenStateChangedEventData data)
	{
		if (data.OrientationChanged)
		{
			if (GamePhaseManager.PhaseStackCount != 0)
			{
				GamePhaseManager.Push(GamePhase.Pause);
			}
		}
	}

	#endregion
}