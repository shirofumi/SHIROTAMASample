using UnityEngine;
using System;

using UnityRandom = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemEffect : MonoBehaviour, IScreenEffect
{
	#region Constants

	private const float DeadAngle = 90.0f;

	#endregion
	
	#region Fields

	public Vector3 Motion;

	private Vector2 start;

	private GameObject target;

	private float angle;

	private Vector2 offset;

	private Vector2 forward;

	private Vector2 side;

	private Vector2 scattering;

	private bool cached;

	#endregion

	#region Events

	public event Action End;

	#endregion

	#region Messages

	private void Start()
	{
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.sprite = SpriteSelector.GetItem(ItemType.Small, 1);
	}

	private void Update()
	{
		if (!cached) UpdateDirections();

		transform.localPosition = CalculatePosition();
	}

	private void Terminate()
	{
		if (End != null) End();

		Destroy(gameObject);
	}

	#endregion

	#region Methods

	public void Init(GameObject start, GameObject target)
	{
		this.start = start.transform.position;
		this.target = target;
		this.angle = UnityRandom.value * (360.0f - DeadAngle) + (DeadAngle / 2.0f);
	}

	void IScreenEffect.OnLayoutChanged()
	{
		cached = false;
	}

	private void UpdateDirections()
	{
		Vector2 s = ScreenEffectManager.GetGameObjectPosition(start);
		Vector2 t = ScreenEffectManager.GetUIObjectPosition(target);
		Vector2 d = t - s;
		Vector2 n = d.normalized;
		Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

		this.offset = s;
		this.forward = d;
		this.side = new Vector2(n.y, -n.x);
		this.scattering = rot * n;

		cached = true;
	}

	private Vector2 CalculatePosition()
	{
		return (offset + forward * Motion.x + side * Motion.y + scattering * Motion.z);
	}

	#endregion
}
