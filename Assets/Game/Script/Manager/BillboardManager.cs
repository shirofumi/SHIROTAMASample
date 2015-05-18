
public class BillboardManager : SingletonMonoBehaviour<BillboardManager>
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

	#endregion

	#region Methods

	private void OnScreenStateChanged(ScreenStateChangedEventData data)
	{
		if (data.OrientationChanged)
		{
			Billboard.Invoke();
		}
	}

	#endregion
}