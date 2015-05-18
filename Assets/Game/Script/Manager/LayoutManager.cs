using UnityEngine;
using UnityEngine.Events;

public class LayoutManager : SingletonMonoBehaviour<LayoutManager>
{
	#region Fields

	public float MinInfoAspect;

	public float MaxInfoAspect;

	public float MinPadding;

	[SerializeField]
	private UnityEvent layoutChangedEvent;

	private Rect main;

	private Rect info;

	private bool dirty;

	#endregion

	#region Properties

	public static Rect MainArea
	{
		get { return Instance.main; }
	}

	public static Rect InfoArea
	{
		get { return Instance.info; }
	}

	#endregion

	#region Events

	public static event UnityAction LayoutChanged
	{
		add { Instance._LayoutChanged += value; }
		remove { Instance._LayoutChanged -= value; }
	}

	private event UnityAction _LayoutChanged
	{
		add
		{
			if (layoutChangedEvent == null) layoutChangedEvent = new UnityEvent();

			layoutChangedEvent.AddListener(value);
		}
		remove
		{
			if (layoutChangedEvent == null) return;

			layoutChangedEvent.RemoveListener(value);
		}
	}

	#endregion

	#region Messages

	private void OnEnable()
	{
		ScreenMonitor.ScreenStateChanged += OnScreenStateChanged;
		MapMonitor.MapSizeChanged += OnMapStateChanged;
	}

	private void OnDisable()
	{
		if (ScreenMonitor.IsAlive) ScreenMonitor.ScreenStateChanged -= OnScreenStateChanged;
		if (MapMonitor.IsAlive) MapMonitor.MapSizeChanged -= OnMapStateChanged;
	}

	private void Update()
	{
		if (dirty) Allocate();
	}

	#endregion

	#region Methods

	public void Invalidate()
	{
		this.dirty = true;
	}

	private void Allocate()
	{
		bool landscape = ScreenMonitor.Landscape;

		int sw, sh;
		if (landscape)
		{
			sw = Screen.width;
			sh = Screen.height;
		}
		else
		{
			sw = Screen.height;
			sh = Screen.width;
		}

		Vector2 mainSize = CalculatePreferredMainSize(sh);
		Vector2 infoSize = new Vector2(sw - mainSize.x, sh);

		float minInfoWidth = sh * MinInfoAspect;
		float maxInfoWidth = sh * MaxInfoAspect;
		if (infoSize.x < minInfoWidth)
		{
			float w = sw - minInfoWidth - MinPadding;
			float ratio = w / mainSize.x;
			mainSize.x = w;
			mainSize.y = mainSize.y * ratio;

			infoSize.x = minInfoWidth;
			infoSize.y = mainSize.y;
		}
		else if (infoSize.x > maxInfoWidth)
		{
			infoSize.x = maxInfoWidth;
		}

		if (landscape)
		{
			if (ScreenMonitor.Orientation == ScreenOrientation.LandscapeLeft)
			{
				info.x = 0.0f;
				main.x = infoSize.x;
			}
			else
			{
				info.x = sw - infoSize.x;
				main.x = sw - (infoSize.x + mainSize.x);
			}

			info.y = (sh - infoSize.y) * 0.5f;
			info.width = infoSize.x;
			info.height = infoSize.y;

			main.y = (sh - infoSize.y) * 0.5f;
			main.width = mainSize.x;
			main.height = mainSize.y;
		}
		else
		{
			info.x = (sh - infoSize.y) * 0.5f;
			info.y = sw - infoSize.x;
			info.width = infoSize.y;
			info.height = infoSize.x;

			main.x = (sh - infoSize.y) * 0.5f;
			main.y = sw - (infoSize.x + mainSize.x);
			main.width = mainSize.y;
			main.height = mainSize.x;
		}

		this.dirty = false;

		if (layoutChangedEvent != null) layoutChangedEvent.Invoke();
	}

	private Vector2 CalculatePreferredMainSize(int sh)
	{
		Map map = GameScene.Map;
		float width = (float)((map.Width - 1) * sh) / (float)(map.Height - 1);
		return new Vector2(width, (float)sh);
	}

	private void OnScreenStateChanged(ScreenStateChangedEventData data)
	{
		Invalidate();
	}

	private void OnMapStateChanged()
	{
		Invalidate();
	}

	#endregion
}
