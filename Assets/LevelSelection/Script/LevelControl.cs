using UnityEngine;
using UnityEngine.UI;

namespace LevelSelection
{
	public class LevelControl : MonoBehaviour
	{
		#region Fields

		public int Level;

		public Requirements Requirements;

		#endregion

		#region Properties

		private int Requirement
		{
			get { return Requirements.Points[Level - 1]; }
		}

		private bool Selectable
		{
			get { return (PointCalculator.TotalPoints >= Requirement); }
		}

		#endregion

		#region Messages

		private void Start()
		{
			Image background = GetComponent<Image>();
			Image check = transform.Find("Check").GetComponentInChildren<Image>();
			NumberTextSetter header = transform.Find("Header").GetComponentInChildren<NumberTextSetter>();
			NumberTextSetter requirement = transform.Find("Requirement").GetComponentInChildren<NumberTextSetter>();

			background.sprite = Selectable ? LevelSelectionAssets.SelectableBox : LevelSelectionAssets.UnselectableBox;

			Sprite mark = GetCheckMark();
			check.enabled = (mark != null);
			check.sprite = mark;

			header.Number = Level;
			requirement.Number = Requirement;
		}

		#endregion

		#region Methods

		public void Select()
		{
			if (Selectable)
			{
				LevelSelectionScene.Instance.SelectLevel(Level);

				SystemSoundSource.Start();
			}
			else
			{
				SystemSoundSource.Unable();
			}
		}

		private Sprite GetCheckMark()
		{
			LevelRecord record = RecordManager.GetLevel(Level);

			int accomplishment = -1;
			for (int i = 0; i < GameConstants.Maps; i++)
			{
				accomplishment &= record.Maps[i].Accomplishment;
			}

			int accomplishmentEx = accomplishment;
			for (int i = GameConstants.Maps; i < GameConstants.AllMaps; i++)
			{
				accomplishmentEx &= record.Maps[i].Accomplishment;
			}

			Sprite sprite;
			if ((accomplishment & (1 << -1)) != 0)
			{
				const int mask = (1 << 3) - 1;
				if ((accomplishmentEx & mask) == mask)
				{
					sprite = LevelSelectionAssets.AllCompletedMark;
				}
				else
				{
					sprite = LevelSelectionAssets.CompletedMark;
				}
			}
			else
			{
				sprite = null;
			}

			return sprite;
		}

		#endregion
	}
}