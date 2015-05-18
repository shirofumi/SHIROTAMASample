using UnityEngine;

[RequireComponent(typeof(ExcitingPanelEffectAnimation))]
public class ExcitingPanelEffect : SingletonMonoBehaviour<ExcitingPanelEffect>
{
	#region Fields

	private new ExcitingPanelEffectAnimation animation;

	#endregion

	#region Messages

	private new void Awake()
	{
		base.Awake();

		this.animation = GetComponent<ExcitingPanelEffectAnimation>();
	}

	#endregion

	#region Methods

	public static void Play()
	{
		Instance.animation.Exciting = true;
	}

	public static void Stop()
	{
		Instance.animation.Exciting = false;
	}

	#endregion
}