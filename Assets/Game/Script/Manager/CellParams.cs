using System;
using UnityEngine;

public class CellParams : ScriptableObject
{
	#region Fields

	public float RoughGroundFriction;

	public float RoughGroundHealingPoint;

	public float RoughGroundInterval;

	public float ActionPanelDelayTime;

	public float AccelerationPanelAcceleration;

	public float RotationPanelAngularVelocity;

	public float ExcitingAreaPanelExcitingPoint;

	public float HealingAreaPanelHealingPoint;

	public float PitPanelMaxCentripetalForce;

	public float CrackPanelMargin;

	public float CrackPanelMaxAttractiveForce;

	#endregion
}