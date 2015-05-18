using UnityEngine;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
	#region Constants

	private const int MaxScore = 10 * 1000 * 1000 - 1;

	#endregion

	#region Fields

	public ScoreTable ScoreTable;

	private int score;

	private int timeBonus;

	private int version = -1;

	private int timeStamp = -1;

	#endregion

	#region Properties

	public static int Score
	{
		get
		{
			var @this = Instance;
			if (@this.version != ActionCounter.Version) @this.UpdateScore();

			return @this.score;
		}
	}

	public static int TimeBonus
	{
		get
		{
			var @this = Instance;
			if (@this.timeStamp != TimeManager.TotalSecond) @this.UpdateTimeBonus();

			return @this.timeBonus;
		}
	}

	public static int TotalScore
	{
		get { return Mathf.Min(MaxScore, Score + TimeBonus); }
	}

	#endregion

	#region Methods

	public static int CalculateScore(int point)
	{
		int score = 0;
		ScoreTable table = Instance.ScoreTable;
		GameStatistics statistics = ActionCounter.Statistics;

		score += table.HitBall * statistics.HittingNormalBallCount;
		score += table.HitBallWithFrozen * statistics.SluggingNormalBallCount;
		score += table.HitExcitedBall * statistics.HittingExcitedBallCount;
		score += table.HitExcitedBallWithFrozen * statistics.SluggingExcitedBallCount;

		score += table.BreakWall * statistics.BreakingWallCount;

		score += table.GetItem * (ActionCounter.AccumulatedPoint + point);

		return Mathf.Min(MaxScore, score);
	}

	public static int CalculateTimeBonus(int time)
	{
		int score = 0;
		ScoreTable table = Instance.ScoreTable;

		score += table.TimeMultiplier * Mathf.Max(0, GameScene.Map.LimitTime - time);

		return Mathf.Min(MaxScore, score);
	}

	private void UpdateScore()
	{
		score = CalculateScore(ActionCounter.Point);
		version = ActionCounter.Version;
	}

	private void UpdateTimeBonus()
	{
		int time = TimeManager.TotalSecond;

		timeBonus = CalculateTimeBonus(time);
		timeStamp = time;
	}

	#endregion
}