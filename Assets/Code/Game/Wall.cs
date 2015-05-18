using System;

[Serializable]
public struct Wall
{
	#region Fields

	public int Code;

	#endregion

	#region Properties

	public WallType Type
	{
		get { return (WallType)((Code >> 16) & 0xff); }
		set { Code = (((int)value & 0xff) << 16) | (Code & 0xff); }
	}

	public int Option
	{
		get { return (int)(Code & 0xff); }
		set { Code = (Code & ~0xff) | ((int)value & 0xff); }
	}

	#endregion

	#region Constructors

	public Wall(int code)
	{
		this.Code = code;
	}

	public Wall(WallType type, int option)
	{
		this.Code = (((int)type & 0xff) << 16) | (option & 0xff);
	}

	#endregion
}