using UnityEngine;

namespace LevelSelection
{
	public class LevelSelectionAssets : SingletonMonoBehaviour<LevelSelectionAssets>
	{
		#region Fields

		[SerializeField]
		private Sprite _SelectableBox;

		[SerializeField]
		private Sprite _UnselectableBox;
		
		[SerializeField]
		private Sprite _CompletedMark;

		[SerializeField]
		private Sprite _AllCompletedMark;

		#endregion

		#region Properties

		public static Sprite SelectableBox
		{
			get { return Instance._SelectableBox; }
		}

		public static Sprite UnselectableBox
		{
			get { return Instance._UnselectableBox; }
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
