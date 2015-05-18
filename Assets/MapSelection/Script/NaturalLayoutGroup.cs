using UnityEngine;
using UnityEngine.UI;

namespace MapSelection
{
	public class NaturalLayoutGroup : HorizontalOrVerticalLayoutGroup
	{
		#region Methods

		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();

			CalcAlongAxis(0, IsVertical());
		}

		public override void CalculateLayoutInputVertical()
		{
			CalcAlongAxis(1, IsVertical());
		}

		public override void SetLayoutHorizontal()
		{
			SetChildrenAlongAxis(0, IsVertical());
		}

		public override void SetLayoutVertical()
		{
			SetChildrenAlongAxis(1, IsVertical());
		}

		private bool IsVertical()
		{
#if UNITY_EDITOR
			return (Screen.width < Screen.height);
#else
			ScreenOrientation orientation = Screen.orientation;
			return (orientation == ScreenOrientation.Portrait || orientation == ScreenOrientation.PortraitUpsideDown);
#endif
		}

		#endregion
	}
}
