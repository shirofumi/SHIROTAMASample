using System;

[Serializable]
public struct Item
{
	#region Fields

	public int Code;

	#endregion

	#region Properties

	public ItemType Type
	{
		get { return (ItemType)((Code >> 16) & 0xff); }
		set { Code = (((int)value & 0xff) << 16) | (Code & 0xff); }
	}

	public int Option
	{
		get { return (int)(Code & 0xff); }
		set { Code = (Code & ~0xff) | ((int)value & 0xff); }
	}

	public bool TopLeft
	{
		get { return ((Code & (1 << 0)) != 0); }
	}

	public bool TopRight
	{
		get { return ((Code & (1 << 1)) != 0); }
	}

	public bool BottomLeft
	{
		get { return ((Code & (1 << 2)) != 0); }
	}

	public bool BottomRight
	{
		get { return ((Code & (1 << 3)) != 0); }
	}

	#endregion

	#region Constructors

	public Item(int code)
	{
		this.Code = code;
	}

	public Item(ItemType type, int option)
	{
		this.Code = (((int)type & 0xff) << 16) | (option & 0xff);
	}

	#endregion
}