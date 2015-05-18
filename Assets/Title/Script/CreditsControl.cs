using UnityEngine;

namespace Title
{
	public class CreditsControl : MonoBehaviour
	{
		#region Fields

		public GameObject DialogContent;

		#endregion

		#region Methods

		public void Show()
		{
			DialogManager.Show(DialogContent);
		}

		#endregion
	}
}
