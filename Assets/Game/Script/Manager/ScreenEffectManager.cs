using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScreenEffectManager : EffectManager<ScreenEffectManager>
{
	#region Fields

	public Camera GameCamera;

	public Camera UICamera;

	private new Camera camera;

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

	private new void Awake()
	{
		base.Awake();

		if (GameCamera == null) throw new InvalidOperationException("'GameCamera' is null.");
		if (UICamera == null) throw new InvalidOperationException("'UICamera' is null.");

		this.camera = base.GetComponent<Camera>();
	}

	private void Start()
	{
		_UpdateCamera();
	}

	#endregion

	#region Methods

	public static void UpdateCamera()
	{
		Instance._UpdateCamera();
	}

	public static GameObject Add(GameObject original)
	{
		return Instance._Add(original);
	}

	public static Vector2 GetGameObjectPosition(GameObject gameObject)
	{
		return Instance._GetGameObjectPosition(gameObject);
	}

	public static Vector2 GetGameObjectPosition(Vector2 position)
	{
		return Instance._GetGameObjectPosition(position);
	}

	public static Vector2 GetUIObjectPosition(GameObject gameObject)
	{
		return Instance._GetUIObjectPosition(gameObject);
	}

	public static Vector2 GetUIObjectPosition(Vector2 position)
	{
		return Instance._GetUIObjectPosition(position);
	}

	public static Vector2 ToGameSpacePosition(Vector2 position)
	{
		return Instance._ToGameSpacePosition(position);
	}

	public static Vector2 ToUISpacePosition(Vector2 position)
	{
		return Instance._ToUISpacePosition(position);
	}

	private void _UpdateCamera()
	{
		camera.orthographicSize = GameCamera.orthographicSize;
	}

	private GameObject _Add(GameObject original)
	{
		GameObject instance = Instantiate(original);
		instance.transform.SetParent(this.transform, false);
		return instance;
	}

	private Vector2 _GetGameObjectPosition(GameObject gameObject)
	{
		Vector3 pos;
		pos = GameCamera.WorldToScreenPoint(gameObject.transform.position);
		pos = camera.ScreenToWorldPoint(pos);

		return pos;
	}

	private Vector2 _GetGameObjectPosition(Vector2 position)
	{
		Vector3 pos;
		pos = GameCamera.WorldToScreenPoint(position);
		pos = camera.ScreenToWorldPoint(pos);

		return pos;
	}

	private Vector2 _GetUIObjectPosition(GameObject gameObject)
	{
		Vector3 pos;
		pos = UICamera.WorldToScreenPoint(gameObject.transform.position);
		pos = camera.ScreenToWorldPoint(pos);

		return pos;
	}

	private Vector2 _GetUIObjectPosition(Vector2 position)
	{
		Vector3 pos;
		pos = UICamera.WorldToScreenPoint(position);
		pos = camera.ScreenToWorldPoint(pos);

		return pos;
	}

	private Vector2 _ToGameSpacePosition(Vector2 position)
	{
		Vector3 pos;
		pos = camera.WorldToScreenPoint(position);
		pos = GameCamera.ScreenToWorldPoint(pos);

		return pos;
	}

	private Vector2 _ToUISpacePosition(Vector2 position)
	{
		Vector3 pos;
		pos = camera.WorldToScreenPoint(position);
		pos = UICamera.ScreenToWorldPoint(pos);

		return pos;
	}

	private void OnLayoutChanged()
	{
		for (int i = 0, count = transform.childCount; i < count; i++)
		{
			Transform child = transform.GetChild(i);
			IScreenEffect effect = child.GetComponent<IScreenEffect>();
			if (effect != null)
			{
				effect.OnLayoutChanged();
			}
		}
	}

	#endregion
}