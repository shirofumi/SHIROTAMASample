using UnityEngine;

namespace MapSelection
{
	public class MapSelectionAssets : SingletonMonoBehaviour<MapSelectionAssets>
	{
		#region Fields

		[SerializeField]
		private Sprite _MissionSuccess;

		[SerializeField]
		private Sprite _MissionFailure;

		[SerializeField]
		private Sprite _CompletedMark;

		[SerializeField]
		private Sprite _AllCompletedMark;

		#endregion

		#region Properties

		public static Sprite MissionSuccess
		{
			get { return Instance._MissionSuccess; }
		}

		public static Sprite MissionFailure
		{
			get { return Instance._MissionFailure; }
		}

		public static Sprite CompletedMark
		{
			get { return Instance._CompletedMark; }
		}

		public static Sprite AllCompletedMark
		{
			get { return Instance._AllCompletedMark; }
		}

		#endregion
	}
}
