using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BallAnimationEffect : MonoBehaviour
{
	#region Fields

	public Color Color;

	private Material material;

	#endregion

	#region Messages

	private void Start()
	{
		Renderer renderer = GetComponent<Renderer>();
		this.material = renderer.material;
	}

	private void Update()
	{
		material.color = Color;
	}

	#endregion
}