using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;

namespace MapSelection
{
	public class MapControl : MonoBehaviour, ISelectHandler
	{
		#region Fields

		private MapID id;

		private Image completed;

		private Text header;

		private Image[] checks;

		#endregion

		#region Properties

		public MapID ID
		{
			get { return id; }
			set { id = value; }
		}

		#endregion

		#region Messages

		private void Awake()
		{
			this.completed = transform.Find("CompletionMark").GetComponent<Image>();
			this.header = transform.Find("Header").GetComponent<Text>();

			Transform list = transform.Find("Checks");
			Image[] checks = new Image[list.childCount];
			for (int i = 0; i < list.childCount; i++)
			{
				checks[i] = list.GetChild(i).GetComponent<Image>();
			}
			this.checks = checks;
		}

		private void Start()
		{
			MapRecord record = RecordManager.GetMap(id);

			header.text = GetLabelText();

			Sprite mark = GetCheckMark(record);
			completed.enabled = (mark != null);
			completed.sprite = mark;

			for (int i = 0; i < checks.Length; i++)
			{
				checks[i].sprite = record[i] ? MapSelectionAssets.MissionSuccess : MapSelectionAssets.MissionFailure;
			}
		}

		#endregion

		#region Methods

		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			MapSelectionScene.Current = id;

			SystemSoundSource.Select();
		}

		private string GetLabelText()
		{
			StringBuilder sb = new StringBuilder(16);
			sb.Append(id.Primary);
			sb.Append(" - ");
			sb.Append(id.Secondary);
			return sb.ToString();
		}

		private Sprite GetCheckMark(MapRecord record)
		{
			Sprite sprite;
			if (record.Completed)
			{
				const int mask = (1 << 3) - 1;
				if ((record.Accomplishment & mask) == mask)
				{
					sprite = MapSelectionAssets.AllCompletedMark;
				}
				else
				{
					sprite = MapSelectionAssets.CompletedMark;
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
