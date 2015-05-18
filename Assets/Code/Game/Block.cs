using System;

[Serializable]
public struct Block : IEquatable<Block>
{
	#region Fields

	public byte X;

	public byte Y;

	public byte Width;

	public byte Height;

	#endregion

	#region Constructors

	public Block(byte x, byte y, byte width, byte height)
	{
		this.X = x;
		this.Y = y;
		this.Width = width;
		this.Height = height;
	}

	public Block(int x, int y, int width, int height)
	{
		this.X = (byte)x;
		this.Y = (byte)y;
		this.Width = (byte)width;
		this.Height = (byte)height;
	}

	#endregion

	#region Methods

	public bool Equals(Block other)
	{
		return (this.X == other.X && this.Y == other.Y && this.Width == other.Width && this.Height == other.Height);
	}

	public override bool Equals(object obj)
	{
		return (obj is Block && this.Equals((Block)obj));
	}

	public override int GetHashCode()
	{
		return ((this.X << 24) | (this.Y << 16) | (this.Width << 8) | (this.Height << 0));
	}

	#endregion	
}