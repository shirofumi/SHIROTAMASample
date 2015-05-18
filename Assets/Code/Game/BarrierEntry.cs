using System;

[Serializable]
public struct BarrierEntry
{
	#region Fields

	public BarrierType Type;

	public BarrierScale Scale;

	public float Threshold;

	#endregion
}