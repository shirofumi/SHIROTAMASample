using UnityEngine;

[RequireComponent(typeof(DirectionChangePanelEffectAnimation), typeof(SpriteRenderer))]
public class DirectionChangePanelEffect : MonoBehaviour
{
	#region Fields

	private new DirectionChangePanelEffectAnimation animation;

	#endregion

	#region Messages

	private void Awake()
	{
		this.animation = GetComponent<DirectionChangePanelEffectAnimation>();
	}

	private void Start()
	{
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.sprite = SpriteSelector.GetPanel(PanelType.DirectionChange, 0);
	}

	#endregion

	#region Methods

	public void Play()
	{
		animation.ChangeDirection = true;
	}

	#endregion
}