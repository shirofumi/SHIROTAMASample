using System;
using System.Runtime.Serialization;

[Serializable]
public class GameRecord : ISerializable
{
	#region Constants

	private const string LevelsName = "levels";

	#endregion

	#region Fields

	private LevelRecord[] levels;

	#endregion

	#region Properties

	public LevelRecord[] Levels
	{
		get { return levels; }
	}

	#endregion

	#region Constructors

	public GameRecord(int levelSize, int mapSize)
	{
		levels = new LevelRecord[levelSize];
		for (int i = 0; i < levels.Length; i++)
		{
			levels[i] = new LevelRecord(mapSize);
		}
	}

	protected GameRecord(SerializationInfo info, StreamingContext context)
	{
		this.levels = (LevelRecord[])info.GetValue(LevelsName, typeof(LevelRecord[]));
	}

	#endregion

	#region Methods

	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(LevelsName, this.levels, typeof(LevelRecord[]));
	}

	#endregion
}
