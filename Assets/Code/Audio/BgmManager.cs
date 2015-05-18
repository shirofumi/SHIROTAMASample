using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BgmManager : SingletonMonoBehaviour<BgmManager>
{
	#region Fields

	private new AudioSource audio;

	#endregion

	#region Properties

	public static AudioSource Audio
	{
		get { return Instance.audio; }
	}

	#endregion

	#region Messages

	private new void Awake()
	{
		base.Awake();

		this.audio = GetComponent<AudioSource>();
	}

	private new void OnDestroy()
	{
		base.OnDestroy();

		Destroy(gameObject);
	}

	private void Update()
	{
		audio.volume = ScreenFadeManager.Amount;
	}

	#endregion

}