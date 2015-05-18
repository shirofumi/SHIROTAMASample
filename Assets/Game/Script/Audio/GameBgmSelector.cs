using UnityEngine;
using UnityEngine.Audio;

public class GameBgmSelector : MonoBehaviour
{
	#region Fields

	public AudioMixerGroup Output;

	public AudioClip MainBgm;

	public AudioClip ResultBgm;

	public AudioClip StartJingle;

	public float FadeTime;

	private AudioMixerSnapshot snapshot;

	private AudioMixerSnapshot ssNormal;

	private AudioMixerSnapshot ssMute;

	private AudioMixerSnapshot ssPause;

	#endregion

	#region Messages

	private void Awake()
	{
		AudioMixer mixer = Output.audioMixer;
		this.ssNormal = mixer.FindSnapshot("Normal");
		this.ssMute = mixer.FindSnapshot("Mute");
		this.ssPause = mixer.FindSnapshot("Pause");
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

	private void PlayBgm(AudioClip clip, bool loop)
	{
		AudioSource audio = BgmManager.Audio;

		ssNormal.TransitionTo(0.0f);
		audio.outputAudioMixerGroup = Output;
		audio.clip = clip;
		audio.loop = loop;
		audio.Play();

		snapshot = ssNormal;
	}

	private void FadeOut()
	{
		ssMute.TransitionTo(FadeTime);

		snapshot = ssMute;
	}

	private void TurnUpVolume()
	{
		if (snapshot != ssMute)
		{
			ssNormal.TransitionTo(0.0f);

			snapshot = ssNormal;
		}
	}

	private void TurnDownVolume()
	{
		if (snapshot != ssMute)
		{
			ssPause.TransitionTo(0.0f);

			snapshot = ssPause;
		}
	}

	private void OnGamePhaseChanged(GamePhaseChangedEventData data)
	{
		if (data.Previous == GamePhase.Pause)
		{
			TurnUpVolume();
		}
		else if (data.Next == GamePhase.Pause)
		{
			TurnDownVolume();
		}
		else
		{
			switch (data.Next)
			{
				case GamePhase.Beginning:
					PlayBgm(StartJingle, false);
					break;
				case GamePhase.Ready:
					PlayBgm(MainBgm, true);
					break;
				case GamePhase.Result:
					PlayBgm(ResultBgm, true);
					break;
				case GamePhase.Ending:
				case GamePhase.Completed:
					FadeOut();
					break;
				case GamePhase.Failed:
					TurnDownVolume();
					break;
			}
		}
	}

	#endregion
}
