using UnityEngine;
using System;
using System.Collections.Generic;

using UnityObject = UnityEngine.Object;
using UnityRandom = UnityEngine.Random;

public class BarrierManager : SingletonMonoBehaviour<BarrierManager>
{
	#region Tuple

	private struct Tuple : IComparable<Tuple>
	{
		public Transform Barrier;

		public float Value;

		public int CompareTo(Tuple other)
		{
			return Comparer<float>.Default.Compare(this.Value, other.Value);
		}
	}

	#endregion

	#region Constants

	private const int BufferSize = 5;

	#endregion

	#region Fields

	[SerializeField]
	private BarrierParams parameters;

	[SerializeField]
	private BarrierSEs ses;

	private Transform active;

	private Transform stock;

	private Collider2D area;

	private List<Tuple> buffer = new List<Tuple>(BufferSize);

	#endregion

	#region Properties

	public static BarrierParams Params
	{
		get { return Instance.parameters; }
	}

	public static BarrierSEs SEs
	{
		get { return Instance.ses; }
	}

	public static Transform Active
	{
		get { return Instance.active; }
	}

	public static Transform Stock
	{
		get { return Instance.stock; }
	}

	public static Collider2D Area
	{
		get { return Instance.area; }
	}

	#endregion

	#region Messages

	private new void Awake()
	{
		base.Awake();

		this.active = transform.Find("Active");
		this.stock = transform.Find("Stock");
		this.area = transform.Find("Area").GetComponent<Collider2D>();
	}

	private void Update()
	{
		if (GamePhaseManager.ReadyOrRunning)
		{
			HandleInput();
		}
	}

	private void LateUpdate()
	{
		CheckBarrierStore();
	}

	#endregion

	#region Methods

	private void CheckBarrierStore()
	{
		int activeCount = active.childCount;
		int storedCount = stock.childCount;
		int sum = activeCount + storedCount;
		int max = GameScene.Layer.MaxBarrierCount;
		if (sum < max)
		{
			for (int i = 0, diff = max - sum; i < diff; i++)
			{
				GameObject barrier = CreateBarrier();
				barrier.SetActive(false);
				barrier.transform.SetParent(stock, false);
			}
		}
	}

	private GameObject CreateBarrier()
	{
		Barrier barrier = new Barrier();
		float r = UnityRandom.value;

		BarrierEntry[] entries = GameScene.Layer.BarrierEntries;
		for (int i = 0; i < entries.Length; i++)
		{
			if (r < entries[i].Threshold)
			{
				barrier.Type = entries[i].Type;
				barrier.Scale = entries[i].Scale;
				barrier.InitialAngle = UnityRandom.Range(0, 16) * (360.0f / 16.0f);
				break;
			}
		}

		GameObject instance = InstantiateBarrier(SelectTemplate(barrier), barrier);
		instance.transform.localRotation = Quaternion.AngleAxis(barrier.InitialAngle, Vector3.forward);

		return instance;
	}

	private GameObject SelectTemplate(Barrier barrier)
	{
		BarrierStore store = BarrierStore.Instance;

		switch (barrier.Type)
		{
			case BarrierType.Stick:
				switch (barrier.Scale)
				{
					case BarrierScale.Small: return store.SmallStick;
					case BarrierScale.Medium: return store.MediumStick;
					case BarrierScale.Large: return store.LargeStick;
				}
				break;

			case BarrierType.Circle:
				switch (barrier.Scale)
				{
					case BarrierScale.Small: return store.SmallCircle;
					case BarrierScale.Medium: return store.MediumCircle;
					case BarrierScale.Large: return store.LargeCircle;
				}
				break;
		}

		return null;
	}

	private GameObject InstantiateBarrier(GameObject original, Barrier barrier)
	{
		GameObject result = UnityObject.Instantiate(original);
		result.transform.SetParent(transform, false);

		BarrierControl script = result.GetComponent<BarrierControl>();
		script.Barrier = barrier;

		return result;
	}

	private Transform SetBarrier(Vector2 position)
	{
		if (area.OverlapPoint(position))
		{
			Transform barrier = stock.GetChild(0);

			Vector3 pos = barrier.localPosition;
			pos.x = position.x;
			pos.y = position.y;
			barrier.localPosition = pos;

			barrier.gameObject.SetActive(true);
			barrier.SetParent(active, false);

			GameSEGlobalSource.Play(SEs.Set);

			ActionCounter.SetBarrier();

			return barrier;
		}

		return null;
	}

	private void HandleInput()
	{
		for (int i = 0; i < InputManager.GestureCount; i++)
		{
			Gesture gesture;
			InputManager.GetGesture(i, out gesture);

			if (gesture.Type == GestureType.Tap)
			{
				HandleTap(ref gesture);
			}
			else if (gesture.Type == GestureType.Flick)
			{
				HandleFlick(ref gesture);
			}
		}
	}

	private void HandleTap(ref Gesture gesture)
	{
		if (stock.childCount == 0) return;

		Camera camera = Camera.main;
		Vector2 position = camera.ScreenToWorldPoint(gesture.Position);

		SetBarrier(position);
	}

	private void HandleFlick(ref Gesture gesture)
	{
		Camera camera = Camera.main;
		Vector2 position = camera.ScreenToWorldPoint(gesture.Position);
		Vector2 velocity = (Vector2)camera.ScreenToWorldPoint(gesture.Position + gesture.Velocity) - position;

		for (int i = 0; i < active.childCount; i++)
		{
			Tuple tuple;
			tuple.Barrier = active.GetChild(i);
			tuple.Value = (position - (Vector2)tuple.Barrier.position).sqrMagnitude;

			buffer.Add(tuple);
		}

		buffer.Sort();

		bool handled = false;
		for (int i = 0; i < buffer.Count; i++)
		{
			Tuple tuple = buffer[i];
			BarrierControl script = tuple.Barrier.GetComponent<BarrierControl>();

			if (script.Push(position, velocity))
			{
				handled = true;
				break;
			}
		}

		buffer.Clear();

		if (!handled && stock.childCount != 0)
		{
			Transform barrier = SetBarrier(position);
			BarrierControl script = barrier.GetComponent<BarrierControl>();
			script.Push(position, velocity);
		}
	}

	#endregion
}
