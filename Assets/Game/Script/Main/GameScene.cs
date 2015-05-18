using UnityEngine;

public class GameScene : SingletonMonoBehaviour<GameScene>
{
	#region Fields

	private Map map;

	private int depth;

	private GameContext context;

	private BallControl ball;

	#endregion

	#region Properties

	public static Map Map
	{
		get { return Instance.map; }
	}

	public static Layer Layer
	{
		get
		{
			GameScene @this = Instance;
			return @this.map.Layers[@this.depth];
		}
	}

	public static int Depth
	{
		get { return Instance.depth; }
	}

	public static GameContext GameContext
	{
		get { return Instance.context; }
	}

	public static BallControl Ball
	{
		get { return Instance.ball; }
	}

	#endregion

	#region Messages

	private void OnEnable()
	{
		GamePhaseManager.GamePhaseChanged += OnGamePhaseChanged;
	}

	private void OnDisable()
	{
		if (GamePhaseManager.IsAlive) GamePhaseManager.GamePhaseChanged -= OnGamePhaseChanged;
	}

	private new void Awake()
	{
		base.Awake();

		InitFromContext();

		ThemeResourceManager.Load(Layer.Theme);

		this.ball = GameObject.FindGameObjectWithTag(Tags.Ball).GetComponent<BallControl>();
	}

	private void Start()
	{
		ScreenFadeManager.FadeIn();
	}

	private new void OnDestroy()
	{
		Time.timeScale = 1.0f;

		base.OnDestroy();
	}

	#endregion

	#region Methods

	public void Pause()
	{
		GamePhaseManager.Push(GamePhase.Pause);
	}

	public void Back()
	{
		if (GamePhaseManager.Phase == GamePhase.Pause)
		{
			GamePhaseManager.Pop(GamePhase.Pause);
		}
		else
		{
			GamePhaseManager.Push(GamePhase.Pause);
		}
	}

	public void Restart()
	{
		SuspensionManager.Delete();

		ScreenFadeManager.FadeOut(() =>
		{
			Context.Data[Keys.Depth] = 0;
			Context.Data.Remove(Keys.GameContext);

			Application.LoadLevel(Scenes.Main);
		});
	}

	public void ToHome()
	{
		SuspensionManager.Delete();

		ScreenFadeManager.FadeOut(() =>
		{
			Context.Data.Remove(Keys.Depth);
			Context.Data.Remove(Keys.GameContext);
			Context.Cache.Remove(Keys.MapCache);

			Application.LoadLevel(Scenes.MapSelection);
		});
	}

	private void InitFromContext()
	{
		MapID id = (MapID)Context.Data[Keys.MapID];
		this.map = LoadMap(id);

		this.depth = (int)Context.Data[Keys.Depth];

		object value;
		if (Context.Data.TryGetValue(Keys.GameContext, out value))
		{
			this.context = ((GameContext)value).Clone();
		}
	}

	private Map LoadMap(MapID id)
	{
		if (Context.Cache.ContainsKey(Keys.MapCache))
		{
			Map cache = (Map)Context.Cache[Keys.MapCache];

			if (cache.ID.Equals(id)) return cache;
		}
		
		Map map = Resources.Load<Map>(id.GetResourceName());

		Context.Cache.Add(Keys.MapCache, map);

		return map;
	}

	private void OnCompleted()
	{
		if (depth + 1 < map.Layers.Length)
		{
			Context.Data[Keys.Depth] = depth + 1;

			GameContext context = new GameContext();
			context.Time = TimeManager.TotalSecond;
			context.Statistics = ActionCounter.Statistics;
			Context.Data[Keys.GameContext] = context;

			SuspensionManager.Save();
		}
		else
		{
			MapRecord record = new MapRecord();
			record.Completed = true;
			record.HighScore = ScoreManager.TotalScore;
			record.Mission1 = MissionChecker.IsAccomplished(0);
			record.Mission2 = MissionChecker.IsAccomplished(1);
			record.Mission3 = MissionChecker.IsAccomplished(2);
			RecordManager.Update(map.ID, record);

			SuspensionManager.Delete();
		}
	}

	private void OnMissed()
	{
		SuspensionManager.Delete();
	}

	private void OnEnd()
	{
		if (depth + 1 < map.Layers.Length)
		{
			ScreenFadeManager.FadeOut(() =>
			{
				Application.LoadLevel(Scenes.Main);
			});
		}
		else
		{
			ToHome();
		}
	}

	private void OnGamePhaseChanged(GamePhaseChangedEventData data)
	{
		Time.timeScale = (data.Next == GamePhase.Pause ? 0.0f : 1.0f);

		switch (data.Next)
		{
			case GamePhase.Completed:
				OnCompleted();
				break;
			case GamePhase.Missed:
				OnMissed();
				break;
			case GamePhase.Ending:
				OnEnd();
				break;
		}
	}

	#endregion
}