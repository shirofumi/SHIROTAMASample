using UnityEngine;

public class InfoCameraControl : MonoBehaviour
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
		Camera camera = GetComponent<Camera>();

		Rect rect = LayoutManager.InfoArea;
		float sw = (float)Screen.width;
		float sh = (float)Screen.height;

		float x = rect.x / sw;
		float y = rect.y / sh;
		float w = rect.width / sw;
		float h = rect.height / sh;

		camera.rect = new Rect(x, y, w, h);
	}

	#endregion
}
