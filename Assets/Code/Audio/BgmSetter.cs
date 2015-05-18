using UnityEngine;
using UnityEngine.Audio;

public class BgmSetter : MonoBehaviour
{
	#region Fields

	public AudioMixerGroup Output;

	public AudioClip Clip;

	public bool Loop;

	#endregion

	#region Messages

	private void Start()
	{
		AudioSource audio = BgmManager.Audio;

		audio.outputAudioMixerGroup = Output;
		audio.clip = Clip;
		audio.loop = Loop;

		if(!audio.isPlaying) audio.Play();
	}

	#endregion
}