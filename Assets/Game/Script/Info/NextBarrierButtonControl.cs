using UnityEngine;
using UnityEngine.UI;

public class NextBarrierButtonControl : MonoBehaviour
{
	#region Fields

	public AudioClip Sound;

	private Button button;

	#endregion

	#region Messages

	private void OnEnable()
	{
		GamePhaseManager.GamePhaseChanged += OnGamePhaseChanged;
	}

	private void OnDisable()
	{
		if (GamePhaseManager.IsAlive) GamePhaseManager.GamePhaseChanged -= OnGamePhaseChanged;
	}

	private void Awake()
	{
		this.button = GetComponent<Button>();
	}

	#endregion

	#region Methods

	public void OnClick()
	{
		Transform stock = BarrierManager.Stock;

		if (stock.childCount != 0)
		{
			GameObject head = stock.GetChild(0).gameObject;

			GameSEGlobalSource.Play(Sound);

			Destroy(head);
		}
	}

	private void OnGamePhaseChanged(GamePhaseChangedEventData data)
	{
		button.interactable = ((data.Next & (GamePhase.Ready | GamePhase.Running)) != 0);
	}

	#endregion
}