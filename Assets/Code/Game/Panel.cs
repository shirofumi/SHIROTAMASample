using System;

[Serializable]
public struct Panel
{
	#region Fields

	public int Code;

	#endregion

	#region Properties

	public PanelType Type
	{
		get { return (PanelType)((Code >> 16) & 0xff); }
		set { Code = (((int)value & 0xff) << 16) | (Code & 0xff); }
	}

	public int Option
	{
		get { return (int)(Code & 0xff); }
		set { Code = (Code & ~0xff) | ((int)value & 0xff); }
	}

	#endregion

	#region Constructors

	public Panel(int code)
	{
		this.Code = code;
	}

	public Panel(PanelType type, int option)
	{
		this.Code = (((int)type & 0xff) << 16) | (option & 0xff);
	}

	#endregion
}