using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
	#region Params

	private class Params
	{
		public const string MasterVolume = "MasterVolume";

		public const string BgmVolume = "BgmVolume";

		public const string SEVolume = "SEVolume";

		public const string SystemVolume = "SystemVolume";
	}

	#endregion

	#region Constants

	public const string MuteKey = "Mute";

	public const string MasterVolumeKey = "MasterVolume";

	public const string BgmVolumeKey = "BgmVolume";

	public const string SEVolumeKey = "SEVolume";

	public const string SystemVolumeKey = "SystemVolume";

	#endregion

	#region Fields

	public static readonly float MinVolume = -80.0f;

	public static readonly float MaxVolume = 20.0f;

	[SerializeField]
	private AudioMixer m_AudioMixer;

	private bool mute;

	private float masterVolume;

	private float bgmVolume;

	private float seVolume;

	private float systemVolume;

	#endregion

	#region Properties

	public static AudioMixer AudioMixer
	{
		get { return Instance.m_AudioMixer; }
	}

	public static bool Mute
	{
		get { return Instance.mute; }
		set { Instance.SetMute(ref Instance.mute, value); }
	}

	public static float MasterVolume
	{
		get { return Instance.masterVolume; }
		set { Instance.SetVolume(ref Instance.masterVolume, value, Params.MasterVolume, MasterVolumeKey); }
	}

	public static float BgmVolume
	{
		get { return Instance.bgmVolume; }
		set { Instance.SetVolume(ref Instance.bgmVolume, value, Params.BgmVolume, BgmVolumeKey); }
	}

	public static float SEVolume
	{
		get { return Instance.seVolume; }
		set { Instance.SetVolume(ref Instance.seVolume, value, Params.SEVolume, SEVolumeKey); }
	}

	public static float SystemVolume
	{
		get { return Instance.systemVolume; }
		set { Instance.SetVolume(ref Instance.systemVolume, value, Params.SystemVolume, SystemVolumeKey); }
	}

	#endregion

	#region Messages

	private new void Awake()
	{
		base.Awake();

		if (m_AudioMixer == null)
		{
			Debug.Log("AudioMixer cannot be null.", gameObject);

			Destroy(gameObject);
		}
	}

	private new void OnDestroy()
	{
		base.OnDestroy();

		Destroy(gameObject);
	}

	private void Start()
	{
		Init();
	}

	#endregion

	#region Methods

	private void Init()
	{
		this.mute = (PlayerPrefs.GetInt(MuteKey, 0) != 0);
		this.masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 0.0f);
		this.bgmVolume = PlayerPrefs.GetFloat(BgmVolumeKey, 0.0f);
		this.seVolume = PlayerPrefs.GetFloat(SEVolumeKey, 0.0f);
		this.systemVolume = PlayerPrefs.GetFloat(SystemVolumeKey, 0.0f);

		m_AudioMixer.SetFloat(Params.MasterVolume, mute ? MinVolume : masterVolume);
		m_AudioMixer.SetFloat(Params.BgmVolume, bgmVolume);
		m_AudioMixer.SetFloat(Params.SEVolume, seVolume);
		m_AudioMixer.SetFloat(Params.SystemVolume, systemVolume);
	}

	private void SetMute(ref bool field, bool value)
	{
		if (field != value)
		{
			field = value;

			float volume = value ? MinVolume : masterVolume;
			m_AudioMixer.SetFloat(Params.MasterVolume, volume);

			PlayerPrefs.SetInt(MuteKey, value ? 1 : 0);
		}
	}

	private void SetVolume(ref float field, float value, string param, string key)
	{
		value = Mathf.Clamp(value, MinVolume, MaxVolume);
		if (field != value)
		{
			field = value;

			if(!mute)m_AudioMixer.SetFloat(param, value);

			PlayerPrefs.SetFloat(key, value);
		}
	}

	#endregion
}
