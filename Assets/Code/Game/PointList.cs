using System;

[Serializable]
public struct PointList
{
	#region Fields

	public int[] Points;

	#endregion

	#region Properties

	public int this[int index]
	{
		get { return this.Points[index]; }
		set { this.Points[index] = value; }
	}

	#endregion

	#region Methods

	public static implicit operator int[](PointList value)
	{
		return value.Points;
	}

	public static implicit operator PointList(int[] value)
	{
		PointList result;
		result.Points = value;
		return result;
	}

	#endregion
}