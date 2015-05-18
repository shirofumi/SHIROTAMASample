using UnityEngine;

public class InfoBillboard : Billboard
{
	#region Fields

	private static readonly Quaternion rotation = Quaternion.AngleAxis(90.0f, Vector3.forward);

	#endregion

	#region Methods

	protected override void Process()
	{
		if (ScreenMonitor.Landscape)
		{
			transform.localRotation = Quaternion.identity;
		}
		else
		{
			transform.localRotation = rotation;
		}
	}

	#endregion
}
