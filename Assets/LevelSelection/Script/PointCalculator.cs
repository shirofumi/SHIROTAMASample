
namespace LevelSelection
{
	public class PointCalculator : SingletonMonoBehaviour<PointCalculator>
	{
		#region Fields

		public PointTable Table;

		private int points;

		#endregion

		#region Properties

		public static int TotalPoints
		{
			get { return Instance.points; }
		}

		#endregion

		#region Messages

		private new void Awake()
		{
			base.Awake();

			Calculate();
		}

		#endregion

		#region Methods

		private void Calculate()
		{
			int points = 0;

			GameRecord record = RecordManager.Record;
			LevelRecord[] levels = record.Levels;
			for (int levelIndex = 0; levelIndex < levels.Length; levelIndex++)
			{
				MapRecord[] maps = levels[levelIndex].Maps;
				for (int mapIndex = 0; mapIndex < maps.Length; mapIndex++)
				{
					if (maps[mapIndex].Completed)
					{
						points += Table[levelIndex][mapIndex];
					}
				}
			}

			this.points = points;
		}

		#endregion
	}
}
