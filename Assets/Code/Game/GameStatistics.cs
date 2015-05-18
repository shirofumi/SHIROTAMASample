using System;

[Serializable]
public class GameStatistics : ICloneable
{
	#region Fields

	public int BarrierCountInReady;

	public int BarrierCountInRunning;

	public int PushingBarrierCountInReady;

	public int PushingBarrierCountInRunning;

	public int HittingNormalBallCount;

	public int HittingExcitedBallCount;

	public int SluggingNormalBallCount;

	public int SluggingExcitedBallCount;

	public int AccelerationCount;

	public int DirectionChangeCount;

	public int RotationCount;

	public int BreakingWallCount;

	public int OutOfControlCount;

	public int StopCount;

	#endregion

	#region Properties

	public int TotalBarrierCount { get { return (BarrierCountInReady + BarrierCountInRunning); } }

	public int TotalPushingBarrierCount { get { return (PushingBarrierCountInReady + PushingBarrierCountInRunning); } }

	public int HittingCount { get { return (HittingNormalBallCount + HittingExcitedBallCount); } }

	public int SluggingCount { get { return (SluggingNormalBallCount + SluggingExcitedBallCount); } }

	public int TotalHittingCount { get { return (HittingCount + SluggingCount); } }

	#endregion

	#region Methods

	public GameStatistics Clone()
	{
		return (GameStatistics)this.MemberwiseClone();
	}

	object ICloneable.Clone()
	{
		return Clone();
	}

	#endregion
}
