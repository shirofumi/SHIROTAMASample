using UnityEngine;

public class GameSEGlobalSource : SingletonMonoBehaviour<GameSEGlobalSource>
{
	#region Fields

	private new AudioSource audio;

	#endregion

	#region Messages

	private new void Awake()
	{
		base.Awake();

		this.audio = GetComponentInChildren<AudioSource>();
	}

	#endregion

	#region Methods

	public static void Play(AudioClip clip)
	{
		Instance.audio.PlayOneShot(clip);
	}

	public static void Play(AudioClip clip, float volumeScale)
	{
		Instance.audio.PlayOneShot(clip, volumeScale);
	}

	#endregion
}