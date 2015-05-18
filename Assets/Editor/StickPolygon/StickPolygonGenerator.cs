using UnityEngine;
using UnityEditor;

public static class StickPolygonGenerator
{
	#region Fields

	private static readonly float[] HalfLength = { 0.63f, 0.88f, 1.13f };

	private static readonly float[] HalfThickness = { 0.1f, 0.1f, 0.1f };

	private static readonly int[] Division = { 5, 5, 5 };

	#endregion

	#region Methods

	[MenuItem("CONTEXT/PolygonCollider2D/StickSmall")]
	public static void GenerateSmall()
	{
		Generate(0);
	}

	[MenuItem("CONTEXT/PolygonCollider2D/StickMedium")]
	public static void GenerateMedium()
	{
		Generate(1);
	}

	[MenuItem("CONTEXT/PolygonCollider2D/StickLarge")]
	public static void GenerateLarge()
	{
		Generate(2);
	}

	private static void Generate(int preset)
	{
		foreach (GameObject gameObject in Selection.gameObjects)
		{
			PolygonCollider2D collider = gameObject.GetComponent<PolygonCollider2D>();
			if (collider != null)
			{
				float hl = HalfLength[preset];
				float ht = HalfThickness[preset];
				int div = Division[preset];

				int count = (div + 1) * 2;
				Vector2[] points = new Vector2[count];

				Vector2 center = new Vector2(hl, 0.0f);
				float step = 180.0f / div;
				int half = count / 2;
				for (int i = 0; i <= div; i++)
				{
					float angle = step * i;
					Vector2 r = Quaternion.AngleAxis(angle, Vector3.back) * Vector2.up * ht;
					Vector2 pos = center + r;

					points[i] = pos;
					points[i + half] = -pos;
				}

				collider.pathCount = 1;
				collider.SetPath(0, points);
			}
		}
	}

	#endregion
}