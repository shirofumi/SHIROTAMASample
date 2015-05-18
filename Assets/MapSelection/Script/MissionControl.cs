using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MapSelection
{
	public class MissionControl : MonoBehaviour
	{
		#region Fields

		public int Index;

		private Image check;

		private Text description;

		private readonly Dictionary<Mission, string> texts = new Dictionary<Mission,string>();

		#endregion

		#region Messages

		private void Awake()
		{
			this.check = transform.Find("Check").GetComponent<Image>();
			this.description = transform.Find("Description").GetComponent<Text>();
		}

		private void OnEnable()
		{
			UpdateInfo();
		}

		private void Update()
		{
			UpdateInfo();
		}

		#endregion

		#region Methods

		private void UpdateInfo()
		{
			MapID map = MapSelectionScene.Current;
			if (map != null)
			{
				MapSummary summary = MapSelectionScene.Level.Maps[map.Secondary - 1];
				MapRecord record = RecordManager.GetMap(map);

				check.sprite = record[Index] ? MapSelectionAssets.MissionSuccess : MapSelectionAssets.MissionFailure;

				if (Index >= 0 && Index < summary.Missions.Length)
				{
					Mission mission = summary.Missions[Index];

					string text;
					if (!texts.TryGetValue(mission, out text))
					{
						text = MissionText.GetDefault(mission);

						texts.Add(mission, text);
					}

					description.text = text;
				}
			}
		}

		#endregion
	}
}
