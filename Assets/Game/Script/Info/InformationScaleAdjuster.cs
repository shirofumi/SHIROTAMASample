using UnityEngine;
using UnityEngine.UI;

public class InformationScaleAdjuster : MonoBehaviour
{
	#region Fields

	private Vector2 defaultResolution;

	#endregion

	#region Messages

	private void OnEnable()
	{
		LayoutManager.LayoutChanged += OnLayoutChanged;
	}

	private void OnDisable()
	{
		if (LayoutManager.IsAlive) LayoutManager.LayoutChanged -= OnLayoutChanged;
	}

	private void Awake()
	{
		CanvasScaler scaler = GetComponent<CanvasScaler>();
		this.defaultResolution = scaler.referenceResolution;
	}

	#endregion

	#region Methods

	private void UpdateReferenceResolution()
	{
		CanvasScaler scaler = GetComponent<CanvasScaler>();

		Vector2 resolution = defaultResolution;
		Vector2 size = LayoutManager.InfoArea.size;
		if (ScreenMonitor.Landscape)
		{
			resolution.y *= Screen.height / size.y;
		}
		else
		{
			resolution.y *= Screen.width / size.x;
		}

		scaler.referenceResolution = resolution;
	}

	private void OnLayoutChanged()
	{
		UpdateReferenceResolution();
	}

	#endregion
}
