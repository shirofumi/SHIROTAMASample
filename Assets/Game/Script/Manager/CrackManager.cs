using UnityEngine;

public class CrackManager : SingletonMonoBehaviour<CrackManager>
{
	#region Fields

	public GameObject Crack;

	#endregion

	#region Messages

	private void Start()
	{
		Init();
	}

	#endregion

	#region Methods

	private void Init()
	{
		float margin = CellManager.Params.CrackPanelMargin;

		Map map = GameScene.Map;
		Layer layer = GameScene.Layer;
		for (int i = 0; i < layer.Cracks.Length; i++)
		{
			Block crack = layer.Cracks[i];

			float x = (crack.X * 2 + crack.Width - map.Width) * +0.5f;
			float y = (crack.Y * 2 + crack.Height - map.Height) * -0.5f;
			float w = crack.Width;
			float h = crack.Height;

			if (crack.X != 0)
			{
				x += margin * 0.5f;
				w -= margin;
			}
			if (crack.X + crack.Width != map.Width)
			{
				x -= margin * 0.5f;
				w -= margin;
			}

			if (crack.Y != 0)
			{
				y -= margin * 0.5f;
				h -= margin;
			}
			if (crack.Y + crack.Height != map.Height)
			{
				y += margin * 0.5f;
				h -= margin;
			}

			GameObject gameObject = (GameObject)Instantiate(Crack, new Vector3(x, y, 0.0f), Quaternion.identity);
			gameObject.transform.SetParent(transform, false);

			BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
			collider.size = new Vector2(w, h);
		}
	}

	#endregion
}