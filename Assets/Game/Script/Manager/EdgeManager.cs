using UnityEngine;

public class EdgeManager : SingletonMonoBehaviour<EdgeManager>
{
	#region Fields

	public GameObject Edge;

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
		Map map = GameScene.Map;
		Layer layer = GameScene.Layer;
		for (int i = 0; i < layer.Edges.Length; i++)
		{
			Block edge = layer.Edges[i];

			float x = (edge.X * 2 + edge.Width - map.Width) * +0.5f;
			float y = (edge.Y * 2 + edge.Height - map.Height) * -0.5f;
			float w = edge.Width;
			float h = edge.Height;

			GameObject gameObject = (GameObject)Instantiate(Edge, new Vector3(x, y, 0.0f), Quaternion.identity);
			gameObject.transform.SetParent(transform, false);

			BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
			collider.size = new Vector2(w, h);
		}
	}

	#endregion
}