using UnityEngine;
using UnityEngine.Audio;

public class GameSEVolumeController : MonoBehaviour
{
	#region Fields

	public AudioMixer Mixer;

	private AudioMixerSnapshot ssNormal;

	private AudioMixerSnapshot ssMute;

	#endregion

	#region Messages

	private void Awake()
	{
		this.ssNormal = Mixer.FindSnapshot("Normal");
		this.ssMute = Mixer.FindSnapshot("Mute");
	}

	private void OnEnable()
	{
		GamePhaseManager.GamePhaseChanged += OnGamePhaseChanged;
	}

	private void OnDisable()
	{
		if (GamePhaseManager.IsAlive) GamePhaseManager.GamePhaseChanged -= OnGamePhaseChanged;
	}
	
	#endregion

	#region Methods

	private void OnGamePhaseChanged(GamePhaseChangedEventData data)
	{
		if (data.Next == GamePhase.Pause)
		{
			ssMute.TransitionTo(0.0f);
		}
		else
		{
			ssNormal.TransitionTo(0.0f);
		}
	}

	#endregion
}
