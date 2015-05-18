using UnityEngine;

public abstract class ItemBehaviour : MonoBehaviour
{
	#region Constants

	private const float AnimationInterval = 1.0f;

	#endregion

	#region Fields

	public Item Item;

	private new SpriteRenderer renderer;

	private Sprite sprite1, sprite2;

	private float prevTime;

	#endregion

	#region Properties

	public abstract int Point { get; }

	#endregion

	#region Messages

	protected void Awake()
	{
		this.renderer = GetComponent<SpriteRenderer>();
	}

	protected void Start()
	{
		CellControl cell = GetComponentInParent<CellControl>();
		if (cell != null)
		{
			cell.WallStateChanged += OnWallStateChanged;
		}

		this.sprite1 = SpriteSelector.GetItem(Item.Type, 1);
		this.sprite2 = SpriteSelector.GetItem(Item.Type, 2);

		renderer.sprite = sprite1;
		prevTime = Time.time;
	}

	protected void Update()
	{
		if (Time.time - prevTime > AnimationInterval)
		{
			Sprite current = renderer.sprite;
			if (current == sprite1)
			{
				renderer.sprite = sprite2;
			}
			else if (current == sprite2)
			{
				renderer.sprite = sprite1;
			}

			prevTime = Time.time;
		}
	}

	protected void OnDestroy()
	{
		CellControl cell = GetComponentInParent<CellControl>();
		if (cell != null)
		{
			cell.WallStateChanged -= OnWallStateChanged;
		}
	}

	protected void OnTriggerEnter2D(Collider2D other)
	{
		GameObject gameObject = other.gameObject;
		if (gameObject.layer == Layers.Ball)
		{
			OnGotItem();

			Destroy(this.gameObject);
		}
	}

	protected void OnWallStateChanged(bool exist)
	{
		Collider2D collider = GetComponent<Collider2D>();
		collider.enabled = !exist;
	}

	#endregion

	#region Methods

	private void OnGotItem()
	{
		int point = Point;

		float volumeScale = 1.0f;
		for (int i = 0; i < point; i++)
		{
			GameObject instance = ScreenEffectManager.Add(EffectStore.Instance.ItemEffect);
			ItemEffect effect = instance.GetComponent<ItemEffect>();
			effect.Init(gameObject, ItemIconControl.Target);
			effect.End += OnItemEffectEnd;

			GameSEGlobalSource.Play(CellManager.SEs.GetItem, volumeScale);
			volumeScale *= 0.5f;
		}

		ActionCounter.GetItem(point);

		if (ActionCounter.Point == GameScene.Layer.Points)
		{
			GameScene.Ball.FlyAway();
		}
	}

	private void OnItemEffectEnd()
	{
		PointInfoControl.GetPoint(Item.Type);

		if (GamePhaseManager.Phase == GamePhase.Completed)
		{
			if (PointInfoControl.Point == GameScene.Layer.Points)
			{
				FlyingBirdEffect.StartAnimation();
			}
		}
	}

	#endregion
}
