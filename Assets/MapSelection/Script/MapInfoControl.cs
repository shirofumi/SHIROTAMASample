using UnityEngine;

namespace MapSelection
{
	public class MapInfoControl : MonoBehaviour
	{
		#region Fields

		private GameObject information;

		private NumberTextSetter primary, secondary;

		private NumberTextSetter layers;

		private NumberTextSetter highscore;

		#endregion

		#region Messages

		private void Awake()
		{
			this.information = transform.Find("Information").gameObject;

			NumberTextSetter[] numbers = GetComponentsInChildren<NumberTextSetter>();
			foreach (NumberTextSetter number in numbers)
			{
				switch (number.gameObject.name)
				{
					case "Primary":
						primary = number;
						break;
					case "Secondary":
						secondary = number;
						break;
					case "Layers":
						layers = number;
						break;
					case "HighScore":
						highscore = number;
						break;
				}
			}
		}

		private void Update()
		{
			MapID map = MapSelectionScene.Current;

			if (map != null)
			{
				MapSummary summary = MapSelectionScene.Level.Maps[map.Secondary - 1];
				MapRecord record = RecordManager.GetMap(map);

				primary.Number = map.Primary;
				secondary.Number = map.Secondary;
				layers.Number = summary.LayerCount;
				highscore.Number = record.HighScore;

				information.SetActive(true);
			}
			else
			{
				information.SetActive(false);
			}
		}

		#endregion

		#region Methods

		public void StartGame()
		{
			MapSelectionScene.Instance.StartGame();

			SystemSoundSource.Start();
		}

		#endregion
	}
}
