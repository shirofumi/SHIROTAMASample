using UnityEngine;

public class BarrierParams : ScriptableObject
{
	#region Fields

	public float LifeTime;

	public float MaxSpeed;

	public float MaxAngularSpeed;

	public float HittingExcitingPoint;

	public float HittingWithFrozenExcitingPoint;

	public float FreezeTime;

	public float CooldownTime;

	public float KickBackPower;

	public float KickBackPowerWithOutOfControl;

	public float KickBackPowerMultiplierWithFrozen;

	public float MovementAdjustment;

	public float BlinkingTime;

	#endregion
}
