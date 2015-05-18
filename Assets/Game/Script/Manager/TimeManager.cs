using UnityEngine;

public class TimeManager : SingletonMonoBehaviour<TimeManager>
{
	#region Constants

	private const float MaxTime = 1.0e6f;

	private const int MaxSecond = 100 * 60 - 1;

	private const int ActiveFlag = 0x01;

	private const int RunningFlag = 0x02;

	private const int AllFlags = ActiveFlag | RunningFlag;

	#endregion

	#region Fields

	private float accumulated;

	private float start;

	private int minute;

	private int second;

	private int timeStamp;

	private int flags = AllFlags;

	#endregion

	#region Properties

	public static int Minute
	{
		get
		{
			var @this = Instance;
			if (@this.timeStamp != Time.frameCount) @this.UpdateTime();

			return @this.minute;
		}
	}

	public static int Second
	{
		get
		{
			var @this = Instance;
			if (@this.timeStamp != Time.frameCount) @this.UpdateTime();

			return @this.second;
		}
	}

	public static int TotalSecond
	{
		get
		{
			var @this = Instance;
			if (@this.timeStamp != Time.frameCount) @this.UpdateTime();

			return (@this.minute * 60 + @this.second);
		}
	}

	#endregion

	#region Messages

	private void OnEnable()
	{
		GamePhaseManager.GamePhaseChanged += OnGamePhaseChanged;

		UpdateState(ActiveFlag, true);
	}

	private void OnDisable()
	{
		UpdateState(ActiveFlag, false);

		if (GamePhaseManager.IsAlive) GamePhaseManager.GamePhaseChanged -= OnGamePhaseChanged;
	}

	private new void Awake()
	{
		Reset();

		GameContext context = GameScene.GameContext;
		if (context != null)
		{
			this.accumulated = context.Time;
		}
	}

	#endregion

	#region Methods

	public void Reset()
	{
		accumulated = 0.0f;
		start = Time.time;
		timeStamp = -1;
	}

	private void UpdateTime()
	{
		float elapsed = (flags == 0 ? (Time.time - start) : 0.0f);
		int time = Mathf.Min((int)(accumulated + elapsed), MaxSecond);

		minute = time / 60;
		second = time % 60;

		timeStamp = Time.frameCount;
	}

	private void UpdateState(int flag, bool value)
	{
		bool state = (flags == 0);
		if (value)
		{
			flags &= ~flag;
		}
		else
		{
			flags |= flag;
		}
		state ^= (flags == 0);

		if (state)
		{
			if (value)
			{
				start = Time.time;
			}
			else
			{
				accumulated += Time.time - start;

				if (accumulated > MaxTime) accumulated = MaxTime;
			}
		}
	}

	private void OnGamePhaseChanged(GamePhaseChangedEventData data)
	{
		if (data.Next == GamePhase.Running)
		{
			UpdateState(RunningFlag, true);
		}
		else
		{
			UpdateState(RunningFlag, false);
		}
	}

	#endregion
}