using System;

public class MissionChecker : SingletonMonoBehaviour<MissionChecker>
{
	#region Dependency

	[Flags]
	private enum Dependency
	{
		None = 0x00,
		Time = 0x01,
		Statistics = 0x02,
	}

	#endregion

	#region MissionData

	private struct MissionData
	{
		public bool Check;

		public Dependency Dependency;

		public int Timestamp;

		public int Version;
	}

	#endregion

	#region Fields

	private MissionData[] dataSet;

	#endregion

	#region Properties

	public bool this[int index]
	{
		get
		{
			MissionData data = dataSet[index];
			bool invalid =
				((data.Dependency & Dependency.Time) != 0 && data.Timestamp != TimeManager.TotalSecond) ||
				((data.Dependency & Dependency.Statistics) != 0 && data.Version != ActionCounter.Version);
			if (invalid) UpdateState(index);

			return dataSet[index].Check;
		}
	}

	#endregion

	#region Messages

	private new void Awake()
	{
		base.Awake();

		Mission[] missions = GameScene.Map.Missions;

		this.dataSet = new MissionData[missions.Length];
		for (int i = 0; i < dataSet.Length; i++)
		{
			SetDependency(ref dataSet[i], missions[i]);
		}
	}

	#endregion

	#region Methods

	public static bool IsAccomplished(int index)
	{
		return Instance[index];
	}

	private void UpdateState(int index)
	{
		MissionData data = dataSet[index];

		bool dependsOnTime = ((data.Dependency & Dependency.Time) != 0);
		bool dependsOnStatistics = ((data.Dependency & Dependency.Statistics) != 0);

		Mission mission = GameScene.Map.Missions[index];
		int time = (dependsOnTime ? TimeManager.TotalSecond : -1);
		GameStatistics statistics = (dependsOnStatistics ? ActionCounter.Statistics : null);

		data.Check = Check(mission, time, statistics);

		if (dependsOnTime) data.Timestamp = TimeManager.TotalSecond;
		if (dependsOnStatistics) data.Version = ActionCounter.Version;

		dataSet[index] = data;
	}

	private static void SetDependency(ref MissionData data, Mission mission)
	{
		switch (mission.Type)
		{
			case MissionType.FastCompletion:
				data.Dependency = Dependency.Time;
				break;

			case MissionType.LessBarrier:
			case MissionType.LessHitting:
			case MissionType.MoreHitting:
			case MissionType.LessSlugging:
			case MissionType.MoreSlugging:
			case MissionType.MoreAcceleration:
			case MissionType.MoreDicretionChange:
			case MissionType.MoreRotation:
			case MissionType.InitialBarrier:
			case MissionType.ExciteBall:
			case MissionType.DontExciteBall:
			case MissionType.StopBall:
			case MissionType.DontStopBall:
			case MissionType.BreakAll:
				data.Dependency = Dependency.Statistics;
				break;
		}
	}

	private static bool Check(Mission mission, int time, GameStatistics statistics)
	{
		switch (mission.Type)
		{
			case MissionType.FastCompletion: return (time <= mission.Option);
			case MissionType.LessBarrier: return (statistics.TotalBarrierCount <= mission.Option);
			case MissionType.LessHitting: return (statistics.TotalHittingCount <= mission.Option);
			case MissionType.MoreHitting: return (statistics.TotalHittingCount >= mission.Option);
			case MissionType.LessSlugging: return (statistics.SluggingCount <= mission.Option);
			case MissionType.MoreSlugging: return (statistics.SluggingCount >= mission.Option);
			case MissionType.MoreAcceleration: return (statistics.AccelerationCount >= mission.Option);
			case MissionType.MoreDicretionChange: return (statistics.DirectionChangeCount >= mission.Option);
			case MissionType.MoreRotation: return (statistics.RotationCount >= mission.Option);
			case MissionType.InitialBarrier: return (statistics.BarrierCountInRunning == 0);
			case MissionType.ExciteBall: return (statistics.OutOfControlCount >= 1);
			case MissionType.DontExciteBall: return (statistics.OutOfControlCount == 0);
			case MissionType.StopBall: return (statistics.StopCount >= 1);
			case MissionType.DontStopBall: return (statistics.StopCount == 0);
			case MissionType.BreakAll: return (statistics.BreakingWallCount == mission.Option);
		}

		return false;
	}

	#endregion
}