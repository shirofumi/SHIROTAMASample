using UnityEngine;
using UnityEngine.UI;

public class PauseButtonControl : MonoBehaviour
{
	#region Fields

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

	public void Pause()
	{
		if (button.interactable)
		{
			SystemSoundSource.Pause();

			GameScene.Instance.Pause();
		}
	}

	private void OnGamePhaseChanged(GamePhaseChangedEventData data)
	{
		button.interactable = GamePhaseManager.HasTransition(data.Next, GamePhase.Pause);
	}

	#endregion
}
