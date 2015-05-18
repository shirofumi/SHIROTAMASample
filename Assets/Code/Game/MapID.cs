using System;
using System.Text;

[Serializable]
public class MapID : IEquatable<MapID>
{
	#region Fields

	public int Primary;

	public int Secondary;

	#endregion

	#region Constructors

	public MapID(int primary, int secondary)
	{
		this.Primary = primary;
		this.Secondary = secondary;
	}

	#endregion

	#region Methods

	public bool Equals(MapID other)
	{
		return (other != null && this.Primary == other.Primary && this.Secondary == other.Secondary);
	}

	public override bool Equals(object obj)
	{
		MapID other = obj as MapID;

		return (other != null && this.Equals(other));
	}

	public override int GetHashCode()
	{
		return (Primary ^ Secondary);
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder(8);
		ToString(sb);
		return sb.ToString();
	}

	public string GetResourceName()
	{
		StringBuilder sb = new StringBuilder(16);
		sb.Append(ResourceConstants.MapPath);
		ToString(sb);
		return sb.ToString();
	}

	private void ToString(StringBuilder sb)
	{
		sb.Append(Primary);
		sb.Append('-');
		sb.Append(Secondary);
	}

	#endregion

	
}