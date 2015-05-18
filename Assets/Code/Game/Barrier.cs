using System;

[Serializable]
public struct Barrier : IEquatable<Barrier>
{
	#region Fields

	public BarrierType Type;

	public BarrierScale Scale;

	public float InitialAngle;

	#endregion

	#region Constructors

	public Barrier(BarrierType type, BarrierScale scale, float angle)
	{
		this.Type = type;
		this.Scale = scale;
		this.InitialAngle = angle;
	}

	#endregion

	#region Methods

	public bool Equals(Barrier other)
	{
		return (this.Type == other.Type && this.Scale == other.Scale && this.InitialAngle == other.InitialAngle);
	}

	public override bool Equals(object obj)
	{
		return (obj is Barrier && this.Equals((Barrier)obj));
	}

	public override int GetHashCode()
	{
		return unchecked((((int)Type << 16) | (int)Scale) ^ InitialAngle.GetHashCode());
	}

	public static bool operator ==(Barrier x, Barrier y)
	{
		return x.Equals(y);
	}

	public static bool operator !=(Barrier x, Barrier y)
	{
		return !x.Equals(y);
	}

	#endregion
}