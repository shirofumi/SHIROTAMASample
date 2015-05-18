using System;
using UnityEngine;
using UnityEngine.Events;

using UnityObject = UnityEngine.Object;

[Serializable]
public class WallStateChangedEvent : UnityEvent<bool> { }

public class CellControl : MonoBehaviour
{
	#region Fields

	public int Index = -1;

	[SerializeField]
	private WallStateChangedEvent wallStateChanged;

	#endregion

	#region Properties

	public WallStateChangedEvent WallStateChangedEvent
	{
		get { return wallStateChanged ?? (wallStateChanged = new WallStateChangedEvent()); }
	}

	#endregion

	#region Events

	public event UnityAction<bool> WallStateChanged
	{
		add { WallStateChangedEvent.AddListener(value); }
		remove { WallStateChangedEvent.RemoveListener(value); }
	}

	#endregion

	#region Messages

	private void Start()
	{
		Cell cell = GameScene.Layer.Cells[Index];

		CreateGround(cell);
		CreatePanel(cell);
		CreateWall(cell);
		CreateItem(cell);
	}

	#endregion

	#region Methods

	private void CreateGround(Cell cell)
	{
		CellStore store = CellStore.Instance;

		Ground ground = cell.Ground;
		switch (ground.Type)
		{
			case GroundType.Normal:
			case GroundType.NormalToRough:
				InstantiateGround<NormalGroundControl>(store.NormalGround, ground);
				break;

			case GroundType.Rough:
				InstantiateGround<RoughGroundControl>(store.RoughGround, ground);
				break;
		}
	}

	private void CreatePanel(Cell cell)
	{
		CellStore store = CellStore.Instance;

		Panel panel = cell.Panel;
		switch (panel.Type)
		{
			case PanelType.Acceleration:
				InstantiatePanel<AccelerationPanelControl>(store.AccelerationPanel, panel);
				break;

			case PanelType.DirectionChange:
				InstantiatePanel<DirectionChangePanelControl>(store.DirectionChangePanel, panel);
				break;

			case PanelType.RotationCW:
				InstantiatePanel<RotationPanelControl>(store.RotationCWPanel, panel);
				break;

			case PanelType.RotationCCW:
				InstantiatePanel<RotationPanelControl>(store.RotationCCWPanel, panel);
				break;

			case PanelType.ExcitingArea:
				InstantiatePanel<ExcitingAreaPanelControl>(store.ExcitingAreaPanel, panel);
				break;

			case PanelType.HealingArea:
				InstantiatePanel<HealingAreaPanelControl>(store.HealingAreaPanel, panel);
				break;

			case PanelType.Pit:
				InstantiatePanel<PitPanelControl>(store.PitPanel, panel);
				break;

			case PanelType.Booster:
				InstantiatePanel<BoosterPanelControl>(store.BoosterPanel, panel);
				break;

			case PanelType.Stopper:
				InstantiatePanel<StopperPanelControl>(store.StopperPanel, panel);
				break;

			case PanelType.Crack:
				InstantiatePanel<CrackPanelControl>(store.CrackPanel, panel);
				break;
		}
	}

	private void CreateWall(Cell cell)
	{
		CellStore store = CellStore.Instance;

		Wall wall = cell.Wall;
		switch (wall.Type)
		{
			case WallType.Hard:
				InstantiateWall<HardWallControl>(store.HardWall, wall);
				break;

			case WallType.Breakable:
				InstantiateWall<BreakableWallControl>(store.BreakableWall, wall);
				break;

			case WallType.Edge:
				InstantiateWall<EdgeWallControl>(store.EdgeWall, wall);
				break;
		}
	}

	private void CreateItem(Cell cell)
	{
		CellStore store = CellStore.Instance;

		Item item = cell.Item;
		switch (item.Type)
		{
			case ItemType.Small:
				if (item.TopLeft) InstantiateSmallItem(store.SmallItem, item, -0.25f, 0.25f);
				if (item.TopRight) InstantiateSmallItem(store.SmallItem, item, 0.25f, 0.25f);
				if (item.BottomLeft) InstantiateSmallItem(store.SmallItem, item, -0.25f, -0.25f);
				if (item.BottomRight) InstantiateSmallItem(store.SmallItem, item, 0.25f, -0.25f);
				break;

			case ItemType.Medium:
				InstantiateItem<MediumItemControl>(store.MediumItem, item);
				break;

			case ItemType.Large:
				InstantiateItem<LargeItemControl>(store.LargeItem, item);
				break;
		}
	}

	private GameObject InstantiateGround<T>(GameObject original, Ground ground) where T : GroundBehaviour
	{
		GameObject result = UnityObject.Instantiate(original);
		result.transform.SetParent(transform, false);

		T script = result.GetComponent<T>();
		script.Ground = ground;

		return result;
	}

	private GameObject InstantiatePanel<T>(GameObject original, Panel panel) where T : PanelBehaviour
	{
		GameObject result = UnityObject.Instantiate(original);
		result.transform.SetParent(transform, false);

		T script = result.GetComponent<T>();
		script.Panel = panel;

		return result;
	}

	private GameObject InstantiateWall<T>(GameObject original, Wall wall) where T : WallBehaviour
	{
		GameObject result = UnityObject.Instantiate(original);
		result.transform.SetParent(transform, false);

		T script = result.GetComponent<T>();
		script.Wall = wall;

		return result;
	}

	private GameObject InstantiateItem<T>(GameObject original, Item item) where T : ItemBehaviour
	{
		GameObject result = UnityObject.Instantiate(original);
		result.transform.SetParent(transform, false);

		T script = result.GetComponent<T>();
		script.Item = item;

		return result;
	}

	private GameObject InstantiateSmallItem(GameObject original, Item item, float x, float y)
	{
		GameObject result = InstantiateItem<SmallItemControl>(original, item);

		Vector3 position = result.transform.localPosition;
		position.x = x;
		position.y = y;
		result.transform.localPosition = position;

		return result;
	}

	#endregion
}