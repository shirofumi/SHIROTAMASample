using System;

[Serializable]
public class GameContext : ICloneable
{
	#region Fields

	public int Time;

	public GameStatistics Statistics;

	#endregion

	#region Methods

	public GameContext Clone()
	{
		GameContext instance = new GameContext();
		instance.Time = this.Time;
		instance.Statistics = this.Statistics.Clone();

		return instance;
	}

	object ICloneable.Clone()
	{
		return Clone();
	}

	#endregion
}