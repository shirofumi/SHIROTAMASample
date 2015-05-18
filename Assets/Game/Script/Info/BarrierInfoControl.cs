using UnityEngine;
using UnityEngine.UI;

public class BarrierInfoControl : MonoBehaviour
{
	#region Constants

	private const float BlockSize = 128.0f * 3.0f;

	private const float ActiveScale = 1.0f;

	private const float InactiveScale = 0.5f;

	#endregion
	
	#region Fields

	public int Index;

	public Sprite DisabledBackground;

	private Barrier current;

	private Image image;

	private Image background;

	#endregion

	#region Messages

	private void Awake()
	{
		this.image = transform.Find("Visual").GetComponent<Image>();
		this.background = transform.Find("Background").GetComponent<Image>();
	}

	private void Start()
	{
		if (Index >= GameScene.Layer.MaxBarrierCount)
		{
			image.enabled = false;
			background.sprite = DisabledBackground;
			SetBackgroundScale(InactiveScale);

			this.enabled = false;
		}
	}

	private void Update()
	{
		Transform stock = BarrierManager.Stock;

		if (Index < stock.childCount)
		{
			Transform child = stock.GetChild(Index);
			BarrierControl script = child.GetComponent<BarrierControl>();
			Barrier barrier = script.Barrier;
			if (current != barrier)
			{
				Sprite sprite = SpriteSelector.GetBarrier(barrier.Type, barrier.Scale);
				image.sprite = sprite;

				Rect spriteRect = sprite.rect;
				Vector3 scale = child.localScale;
				float size = Mathf.Max(spriteRect.width * scale.x, spriteRect.height * scale.y);
				float offset = (BlockSize - size) / BlockSize * 0.5f;
				
				RectTransform rect = image.gameObject.GetComponent<RectTransform>();
				float min = offset, max = 1.0f - offset;
				rect.anchorMin = new Vector2(min, min);
				rect.anchorMax = new Vector2(max, max);
				rect.localRotation = Quaternion.AngleAxis(barrier.InitialAngle, Vector3.forward);

				current = barrier;
			}

			image.enabled = true;
			SetBackgroundScale(ActiveScale);
		}
		else
		{
			image.enabled = false;
			SetBackgroundScale(InactiveScale);
		}
	}

	#endregion

	#region Methods

	private void SetBackgroundScale(float scale)
	{
		RectTransform rect = background.rectTransform;
		Vector3 s = rect.localScale;
		s.x = scale;
		s.y = scale;
		rect.localScale = s;
	}

	#endregion
}
