
public class ActionCounter : SingletonMonoBehaviour<ActionCounter>
{
	#region Fields

	private int version;

	private int accumulatedPoint;

	private int point;

	private GameStatistics statistics;

	#endregion

	#region Properties

	public static int Version { get { return Instance.version; } }

	public static int AccumulatedPoint { get { return Instance.accumulatedPoint; } }

	public static int Point { get { return Instance.point; } }

	public static GameStatistics Statistics { get { return Instance.statistics; } }

	#endregion

	#region Messages

	private new void Awake()
	{
		GameContext context = GameScene.GameContext;
		if (context != null)
		{
			this.statistics = context.Statistics;
		}
		else
		{
			this.statistics = new GameStatistics();
		}

		Layer[] layers = GameScene.Map.Layers;
		int depth = GameScene.Depth;

		this.accumulatedPoint = 0;
		for (int i = 0; i < depth; i++)
		{
			this.accumulatedPoint += layers[i].Points;
		}

		version++;
	}

	#endregion

	#region Methods

	public static void SetBarrier()
	{
		var @this = Instance;
		if (GamePhaseManager.Phase == GamePhase.Ready)
		{
			@this.statistics.BarrierCountInReady++;
		}
		else if (GamePhaseManager.Phase == GamePhase.Running)
		{
			@this.statistics.BarrierCountInRunning++;
		}

		@this.version++;
	}

	public static void PushBarrier()
	{
		var @this = Instance;
		if (GamePhaseManager.Phase == GamePhase.Ready)
		{
			@this.statistics.PushingBarrierCountInReady++;
		}
		else if (GamePhaseManager.Phase == GamePhase.Running)
		{
			@this.statistics.PushingBarrierCountInRunning++;
		}

		@this.version++;
	}

	public static void HitBall(bool frozen, bool excited)
	{
		var @this = Instance;
		if (frozen)
		{
			if (excited)
			{
				@this.statistics.SluggingExcitedBallCount++;
			}
			else
			{
				@this.statistics.SluggingNormalBallCount++;
			}
		}
		else
		{
			if (excited)
			{
				@this.statistics.HittingExcitedBallCount++;
			}
			else
			{
				@this.statistics.HittingNormalBallCount++;
			}
		}

		@this.version++;
	}

	public static void EnterAccelerationPanel()
	{
		var @this = Instance;
		@this.statistics.AccelerationCount++;

		@this.version++;
	}

	public static void EnterDirectionChangePanel()
	{
		var @this = Instance;
		@this.statistics.DirectionChangeCount++;

		@this.version++;
	}

	public static void EnterRotationPanel()
	{
		var @this = Instance;
		@this.statistics.RotationCount++;

		@this.version++;
	}

	public static void BreakWall()
	{
		var @this = Instance;
		@this.statistics.BreakingWallCount++;

		@this.version++;
	}

	public static void GetOutOfControl()
	{
		var @this = Instance;
		@this.statistics.OutOfControlCount++;

		@this.version++;
	}

	public static void Stop()
	{
		var @this = Instance;
		@this.statistics.StopCount++;

		@this.version++;
	}

	public static void GetItem(int count)
	{
		var @this = Instance;
		@this.point += count;

		@this.version++;
	}

	#endregion
}