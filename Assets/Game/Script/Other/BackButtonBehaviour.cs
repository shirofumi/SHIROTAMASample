using UnityEngine;
using UnityEngine.Events;

public class BackButtonBehaviour : MonoBehaviour
{
	#region Fields

	public UnityEvent Pause;

	public UnityEvent Back;

	#endregion

	#region Methods

	public void Action()
	{
		if (GamePhaseManager.Phase != GamePhase.Pause)
		{
			if (Pause != null) Pause.Invoke();
		}
		else
		{
			if (Back != null) Back.Invoke();
		}
	}

	#endregion
}