using System;

[Serializable]
public struct Mission : IEquatable<Mission>
{
	#region Fields

	public int Code;

	#endregion

	#region Properties

	public MissionType Type
	{
		get { return (MissionType)((Code >> 16) & 0xff); }
		set { Code = (((int)value & 0xff) << 16) | (Code & 0xff); }
	}

	public int Option
	{
		get { return (int)(Code & 0xff); }
		set { Code = (Code & ~0xff) | (value & 0xff); }
	}

	#endregion

	#region Constructors

	public Mission(int code)
	{
		this.Code = code;
	}

	public Mission(MissionType type, int option)
	{
		this.Code = (((int)type & 0xff) << 16) | (option & 0xff);
	}

	#endregion

	#region Methods

	public bool Equals(Mission other)
	{
		return (this.Code == other.Code);
	}

	public override bool Equals(object obj)
	{
		return (obj != null && obj.GetType() == typeof(Mission) && this.Equals((Mission)obj));
	}

	public override int GetHashCode()
	{
		return Code;
	}

	#endregion

	
}