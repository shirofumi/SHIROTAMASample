using UnityEngine;

namespace MapSelection
{
	public class MapListControl : MonoBehaviour
	{
		#region Fields

		public GameObject Map;

		#endregion

		#region Messages

		private void Start()
		{
			Level level = MapSelectionScene.Level;
			int size = GameConstants.Maps + (SecretMapIsVisible() ? GameConstants.ExtraMaps : 0);
			for (int i = 0; i < size; i++)
			{
				GameObject instance = Instantiate(Map);
				instance.transform.SetParent(transform, false);

				MapControl script = instance.GetComponent<MapControl>();
				script.ID = new MapID(level.Index, i + 1);
			}
		}

		#endregion

		#region Methods

		private bool SecretMapIsVisible()
		{
			LevelRecord record = RecordManager.GetLevel(MapSelectionScene.Level.Index);

			const int mask = ((1 << 3) - 1);
			for (int i = 0; i < GameConstants.Maps; i++)
			{
				if ((record.Maps[i].Accomplishment & mask) != mask) return false;
			}

			return true;
		}

		#endregion
	}
}
