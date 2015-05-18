using UnityEngine;
using UnityEngine.Events;

public class MapMonitor : SingletonMonoBehaviour<MapMonitor>
{
	#region Fields

	[SerializeField]
	private UnityEvent mapSizeChangedEvent;

	private int width;

	private int height;

	#endregion

	#region Events

	public static event UnityAction MapSizeChanged
	{
		add { Instance._MapSizeChanged += value; }
		remove { Instance._MapSizeChanged -= value; }
	}

	private event UnityAction _MapSizeChanged
	{
		add
		{
			if (mapSizeChangedEvent == null) mapSizeChangedEvent = new UnityEvent();

			mapSizeChangedEvent.AddListener(value);
		}
		remove
		{
			if (mapSizeChangedEvent == null) return;

			mapSizeChangedEvent.RemoveListener(value);
		}
	}

	#endregion

	#region Messages

	private void Start()
	{
		CheckMapSize();
	}

	private void Update()
	{
		CheckMapSize();
	}

	#endregion

	#region Methods

	private void CheckMapSize()
	{
		Map map = GameScene.Map;

		if (this.width != map.Width || this.height != map.Height)
		{
			this.width = map.Width;
			this.height = map.Height;

			if (mapSizeChangedEvent != null) mapSizeChangedEvent.Invoke();
		}
	}

	#endregion
}

