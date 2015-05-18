using UnityEngine;

public class MainCameraControl : MonoBehaviour
{
	#region Messages

	private void OnEnable()
	{
		LayoutManager.LayoutChanged += OnLayoutChanged;
	}

	private void OnDisable()
	{
		if (LayoutManager.IsAlive) LayoutManager.LayoutChanged -= OnLayoutChanged;
	}

	#endregion

	#region Methods

	private void OnLayoutChanged()
	{
		Camera camera = Camera.main;
		Map map = GameScene.Map;

		transform.localRotation = ScreenRotation.Get(ScreenOrientation.LandscapeLeft, ScreenMonitor.Orientation);
		camera.orthographicSize = ((ScreenMonitor.Landscape ? map.Height : map.Width) - 1) * 0.5f;

		Rect rect = LayoutManager.MainArea;
		float sw = (float)Screen.width;
		float sh = (float)Screen.height;

		float x = rect.x / sw;
		float y = rect.y / sh;
		float w = rect.width / sw;
		float h = rect.height / sh;

		camera.rect = new Rect(x, y, w, h);

		ScreenEffectManager.UpdateCamera();
	}

	#endregion
}
