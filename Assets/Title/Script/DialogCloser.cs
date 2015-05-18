
using UnityEngine;
using UnityEngine.EventSystems;

namespace Title
{
	public class DialogCloser : MonoBehaviour, IPointerClickHandler
	{
		#region Methods

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			DialogManager.Hide();
		}

		#endregion
	}
}
