using System;
using System.Runtime.Serialization;

[Serializable]
public class LevelRecord : ISerializable
{
	#region Constants

	private const string MapsName = "maps";

	#endregion

	#region Fields

	private MapRecord[] maps;

	#endregion

	#region Properties

	public MapRecord[] Maps
	{
		get { return maps; }
	}

	#endregion

	#region Constructors

	public LevelRecord(int mapSize)
	{
		this.maps = new MapRecord[mapSize];
	}

	protected LevelRecord(SerializationInfo info, StreamingContext context)
	{
		this.maps = (MapRecord[])info.GetValue(MapsName, typeof(MapRecord[]));
	}

	#endregion

	#region Methods

	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(MapsName, this.maps, typeof(MapRecord[]));
	}

	#endregion
}
