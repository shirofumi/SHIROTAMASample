using UnityEngine;

public class BallFaceBillboard : FieldBillboard
{
	#region Fields

	private Quaternion rotation;

	#endregion

	#region Messages

	protected new void Start()
	{
		this.rotation = transform.localRotation;

		base.Start();
	}

	#endregion

	#region Methods

	protected override void Process()
	{
		GamePhase beforeRunning = GamePhase.Beginning | GamePhase.Ready;
		if (GamePhaseManager.PhaseStackCount == 0 || GamePhaseManager.Find(beforeRunning) != -1)
		{
			base.Process();

			transform.localRotation *= rotation;
		}
	}

	#endregion
}