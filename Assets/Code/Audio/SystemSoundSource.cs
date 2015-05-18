using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SystemSoundSource : SingletonMonoBehaviour<SystemSoundSource>
{
	#region Fields

	public SystemSounds Sounds;

	private new AudioSource audio;

	#endregion

	#region Messages

	private new void Awake()
	{
		base.Awake();

		this.audio = GetComponent<AudioSource>();
	}

	#endregion

	#region Methods

	public static void Start()
	{
		Play(Instance.Sounds.Start);
	}

	public static void Start(float volumeScale)
	{
		Play(Instance.Sounds.Start, volumeScale);
	}

	public static void Select()
	{
		Play(Instance.Sounds.Select);
	}

	public static void Select(float volumeScale)
	{
		Play(Instance.Sounds.Select, volumeScale);
	}

	public static void Unable()
	{
		Play(Instance.Sounds.Unable);
	}

	public static void Unable(float volumeScale)
	{
		Play(Instance.Sounds.Unable, volumeScale);
	}

	public static void Pause()
	{
		Play(Instance.Sounds.Pause);
	}

	public static void Pause(float volumeScale)
	{
		Play(Instance.Sounds.Pause, volumeScale);
	}

	public static void Back()
	{
		Play(Instance.Sounds.Back);
	}

	public static void Back(float volumeScale)
	{
		Play(Instance.Sounds.Back, volumeScale);
	}

	private static void Play(AudioClip clip)
	{
		Instance.audio.PlayOneShot(clip);
	}

	private static void Play(AudioClip clip, float volumeScale)
	{
		Instance.audio.PlayOneShot(clip, volumeScale);
	}

	#endregion
}