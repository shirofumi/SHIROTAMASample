using UnityEngine;

public class FieldBillboard : Billboard
{
	#region Methods

	protected override void Process()
	{
		transform.localRotation = ScreenRotation.Get(ScreenOrientation.LandscapeLeft, ScreenMonitor.Orientation);
	}

	#endregion
}