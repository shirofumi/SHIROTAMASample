using System;

[Serializable]
public struct Ground
{
	#region Fields

	public int Code;

	#endregion

	#region Properties

	public GroundType Type
	{
		get { return (GroundType)((Code >> 16) & 0xff); }
		set { Code = (((int)value & 0xff) << 16) | (Code & 0xff); }
	}

	public int Option
	{
		get { return (int)(Code & 0xff); }
		set { Code = (Code & ~0xff) | (value & 0xff); }
	}

	#endregion

	#region Constructors

	public Ground(int code)
	{
		this.Code = code;
	}

	public Ground(GroundType type, int option)
	{
		this.Code = (((int)type & 0xff) << 16) | (option & 0xff);
	}

	#endregion
}