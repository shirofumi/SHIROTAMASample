
public class ScoreInfoControl : SingletonMonoBehaviour<ScoreInfoControl>
{
	#region Fields

	private NumberTextSetter setter;

	private int version;

	private int point;

	#endregion

	#region Properties

	public static int Score
	{
		get { return Instance.setter.Number; }
		set { Instance.setter.Number = value; }
	}

	#endregion

	#region Messages

	private new void Awake()
	{
		base.Awake();

		this.setter = GetComponentInChildren<NumberTextSetter>();
	}

	private void Start()
	{
		this.version = -1;
		this.point = -1;
	}

	private void Update()
	{
		if (version != ActionCounter.Version || point != PointInfoControl.Point)
		{
			version = ActionCounter.Version;
			point = PointInfoControl.Point;

			Score = ScoreManager.CalculateScore(point);
		}
	}

	#endregion
}
