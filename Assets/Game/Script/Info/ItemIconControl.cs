using UnityEngine;
using UnityEngine.UI;

public class ItemIconControl : SingletonMonoBehaviour<ItemIconControl>
{
	#region Fields

	bool startAnimation;

	private GrowingEffect effect;

	private GameObject target;

	#endregion

	#region Properties

	public static GameObject Target
	{
		get { return Instance.target; }
	}

	#endregion

	#region Messages

	private new void Awake()
	{
		base.Awake();

		this.effect = GetComponentInChildren<GrowingEffect>();
		this.target = transform.Find("Target").gameObject;
	}

	private void Start()
	{
		Image image = GetComponentInChildren<Image>();
		image.sprite = SpriteSelector.GetItem(ItemType.Small, 1);
	}

	private void Update()
	{
		if (startAnimation)
		{
			effect.Grow = true;

			startAnimation = false;
		}
	}

	#endregion

	#region Methods

	public static void StartAnimation()
	{
		Instance.startAnimation = true;
	}

	#endregion
}
