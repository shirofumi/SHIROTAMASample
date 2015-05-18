using UnityEngine;

public class FrameControl : MonoBehaviour
{
	#region Messages

	private void OnEnable()
	{
		MapMonitor.MapSizeChanged += OnMapSizeChanged;
	}

	private void OnDisable()
	{
		if (MapMonitor.IsAlive) MapMonitor.MapSizeChanged -= OnMapSizeChanged;
	}

	#endregion

	#region Methods

	private void OnMapSizeChanged()
	{
		var left = transform.Find("Left");
		var right = transform.Find("Right");
		var top = transform.Find("Top");
		var bottom = transform.Find("Bottom");

		Vector3 pos;
		Map map = GameScene.Map;
		float hw = map.Width * 0.5f - 0.25f;
		float hh = map.Height * 0.5f - 0.25f;

		pos = left.localPosition;
		left.localPosition = new Vector3(-hw, pos.y, pos.z);

		pos = right.localPosition;
		right.localPosition = new Vector3(hw, pos.y, pos.z);

		pos = top.localPosition;
		top.localPosition = new Vector3(pos.x, hh, pos.z);

		pos = bottom.localPosition;
		bottom.localPosition = new Vector3(pos.x, -hh, pos.z);
	}

	#endregion
}
