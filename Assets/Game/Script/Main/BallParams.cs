using UnityEngine;

public class BallParams : ScriptableObject
{
	#region Fields

	public float BaseMaxSpeed;

	public float SpeedWithOutOfControl;

	public float MassWithOutOfControl;

	public float HitPower;

	public float HitPowerWithBarrierFrozen;

	public float ImpactAdjustment;

	public float ImpactAbsorption;

	public float ImpactFriction;

	public float AutoHealingInterval;

	public float AutoHealingPoint;

	public float RecoveryTime;

	public float ExcitedLevelAfterRecovery;

	public float ExcitedLevelThreshold;

	#endregion
}
