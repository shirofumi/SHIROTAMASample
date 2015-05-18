using UnityEngine;

[RequireComponent(typeof(HealingPanelEffectAnimation))]
public class HealingPanelEffect : SingletonMonoBehaviour<HealingPanelEffect>
{
	#region Fields

	private new HealingPanelEffectAnimation animation;

	#endregion

	#region Messages

	private new void Awake()
	{
		base.Awake();

		this.animation = GetComponent<HealingPanelEffectAnimation>();
	}

	#endregion

	#region Methods

	public static void Play()
	{
		Instance.animation.Healing = true;
	}

	public static void Stop()
	{
		Instance.animation.Healing = false;
	}

	#endregion
}