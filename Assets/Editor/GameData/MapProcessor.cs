using Fsp.Unity.Editor.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Editor.GameData
{
	[GameDataProcessor(typeof(MapData))]
	public class MapProcessor : GameDataProcessor<MapData>
	{
		#region Methods

		protected override object Process(GDContext context, MapData input)
		{
			Map map;
			try
			{
				map = ProcessMap(input);
			}
			catch (Exception e)
			{
				string header = String.Format("MAP {0}-{1}  ", input.Level, input.Index);

				throw new InvalidOperationException(header + e.Message);
			}

			LinkMapData(context);

			return map;
		}

		private Map ProcessMap(MapData input)
		{
			MapID mapID = new MapID(input.Level, input.Index);

			Map map = ScriptableObject.CreateInstance<Map>();
			map.name = mapID.ToString();
			map.ID = mapID;
			map.Width = input.Width;
			map.Height = input.Height;
			map.LimitTime = input.LimitTime;

			List<Mission> missions = new List<Mission>(input.Missions.Count);
			foreach (MissionData mdata in input.Missions)
			{
				Mission mission;
				ProcessMission(mdata, input, out mission);
				missions.Add(mission);
			}
			map.Missions = missions.ToArray();

			List<Layer> layers = new List<Layer>(input.Layers.Count);
			for (int i = 0; i < input.Layers.Count; i++)
			{
				LayerData ldata = input.Layers[i];
				try
				{
					Layer layer = ProcessLayer(ldata);
					layers.Add(layer);
				}
				catch (Exception e)
				{
					string header = String.Format("LAYER {0}  ", i + 1);

					throw new InvalidOperationException(header + e.Message);
				}
			}
			map.Layers = layers.ToArray();

			return map;
		}

		private Layer ProcessLayer(LayerData ldata)
		{
			Layer layer = ScriptableObject.CreateInstance<Layer>();
			layer.name = "Layer" + ldata.Depth;
			layer.Theme = (global::Theme)ldata.Theme;
			layer.StartPosition = new Vector2(ldata.StartPositionX, ldata.StartPositionY);
			layer.Points = ldata.Cells.Sum(x => GetItemPoint(x.Item));
			layer.MaxBarrierCount = ldata.Barrier.MaxCount;

			List<BarrierEntry> entries = new List<BarrierEntry>(ldata.Barrier.Entries.Count);
			float weightSum = ldata.Barrier.Entries.Select(x => x.Weight).Sum();
			foreach (BarrierEntryData edata in ldata.Barrier.Entries)
			{
				BarrierEntry entry;
				ProcessBarrierEntry(edata, weightSum, out entry);
				entries.Add(entry);
			}
			BarrierEntry[] entryArray = entries.OrderByDescending(x => x.Threshold).ToArray();
			if (entryArray.Length == 0) throw new InvalidOperationException("No barrier entries.");
			entryArray[entryArray.Length - 1].Threshold = Single.PositiveInfinity;
			layer.BarrierEntries = entryArray;

			layer.Edges = FindBlocks(ldata, cdata => cdata.Wall.Type == WallType.Edge);
			layer.Cracks = FindBlocks(ldata, cdata => cdata.Panel.Type == PanelType.Crack);

			List<Cell> cells = new List<Cell>(ldata.Cells.Count);
			foreach (CellData cdata in ldata.Cells)
			{
				Cell cell;
				ProcessGround(cdata, out cell.Ground);
				ProcessPanel(cdata, out cell.Panel);
				ProcessWall(cdata, out cell.Wall);
				ProcessItem(cdata, out cell.Item);

				cells.Add(cell);
			}
			layer.Cells = cells.ToArray();

			return layer;
		}

		private void LinkMapData(GDContext context)
		{
			if (context.Parent != null)
			{
				context.Parent.Link.Set(context.Link, (levelAsset, mapAsset) =>
				{
					Level level = levelAsset as Level;
					Map map = mapAsset as Map;
					if (level != null && map != null)
					{
						int index = map.ID.Secondary - 1;
						if (index < level.Maps.Length)
						{
							level.Maps[index].Missions = (Mission[])map.Missions.Clone();
						}
					}
				});
			}
		}

		private void ProcessMission(MissionData mdata, MapData map, out Mission mission)
		{
			string name = Enum.GetName(typeof(MissionType), mdata.Type);
			var type = (global::MissionType)Enum.Parse(typeof(global::MissionType), name);
			switch (mdata.Type)
			{
				case MissionType.FastCompletion:
					if (mdata.Option > 0 && (mdata.Option % 5 == 0 || mdata.Option < 10))
					{
						mission = new Mission()
						{
							Type = type,
							Option = mdata.Option,
						};
					}
					else throw new InvalidOperationException("Time is not multiple of 5.");
					break;
				case MissionType.LessBarrier:
				case MissionType.LessHitting:
				case MissionType.MoreHitting:
				case MissionType.LessSlugging:
				case MissionType.MoreSlugging:
				case MissionType.MoreAcceleration:
				case MissionType.MoreDicretionChange:
				case MissionType.MoreRotation:
					if (mdata.Option > 0)
					{
						mission = new Mission()
						{
							Type = type,
							Option = mdata.Option,
						};
					}
					else throw new InvalidOperationException("Negative or Zero count.");
					break;
				case MissionType.InitialBarrier:
				case MissionType.ExciteBall:
				case MissionType.DontExciteBall:
				case MissionType.StopBall:
				case MissionType.DontStopBall:
					mission = new Mission()
					{
						Type = type,
						Option = 0,
					};
					break;
				case MissionType.BreakAll:
					mission = new Mission()
					{
						Type = type,
						Option = CountBreakableObjects(map),
					};
					break;
				default:
					throw new InvalidOperationException("None or unknown MissionType");
			}
		}

		private void ProcessBarrierEntry(BarrierEntryData edata, float weightSum, out BarrierEntry entry)
		{
			string tname = Enum.GetName(typeof(BarrierType), edata.Type);
			var type = (global::BarrierType)Enum.Parse(typeof(global::BarrierType), tname);
			switch (edata.Type)
			{
				case BarrierType.Stick:
				case BarrierType.Circle:
					entry.Type = type;
					break;
				default:
					throw new InvalidOperationException("None or unknown BarrierType");
			}

			string sname = Enum.GetName(typeof(BarrierScale), edata.Scale);
			var scale = (global::BarrierScale)Enum.Parse(typeof(global::BarrierScale), sname);
			switch (edata.Scale)
			{
				case BarrierScale.Small:
				case BarrierScale.Medium:
				case BarrierScale.Large:
					entry.Scale = scale;
					break;
				default:
					throw new InvalidOperationException("None or unknown BarrierScale");
			}

			entry.Threshold = edata.Weight / weightSum;
		}

		private void ProcessGround(CellData cdata, out Ground ground)
		{
			switch (cdata.Ground.Type)
			{
				case GroundType.Normal:
					int option;
					if (IsBoundary(cdata, out option))
					{
						ground = new Ground()
						{
							Type = global::GroundType.NormalToRough,
							Option = option,
						};
					}
					else
					{
						ground = new Ground()
						{
							Type = global::GroundType.Normal,
							Option = cdata.Ground.Option,
						};
					}
					break;

				case GroundType.Rough:
					ground = new Ground()
					{
						Type = global::GroundType.Rough,
						Option = cdata.Ground.Option,
					};
					break;

				default:
					throw new InvalidOperationException("None or unknown GroundType");
			}
		}

		private void ProcessPanel(CellData cdata, out Panel panel)
		{
			string name = Enum.GetName(typeof(PanelType), cdata.Panel.Type);
			var type = (global::PanelType)Enum.Parse(typeof(global::PanelType), name);

			switch (cdata.Panel.Type)
			{
				case PanelType.None:
					panel = default(Panel);
					break;

				case PanelType.Acceleration:
				case PanelType.DirectionChange:
					panel = new Panel()
					{
						Type = type,
						Option = cdata.Panel.Option % 8,
					};
					break;

				case PanelType.RotationCW:
				case PanelType.RotationCCW:
				case PanelType.ExcitingArea:
				case PanelType.HealingArea:
				case PanelType.Pit:
				case PanelType.Booster:
				case PanelType.Stopper:
					panel = new Panel()
					{
						Type = type,
						Option = 0,
					};
					break;

				case PanelType.Crack:
					int option;
					GetCrackOption(cdata, out option);
					panel = new Panel()
					{
						Type = global::PanelType.Crack,
						Option = option,
					};
					break;

				default:
					throw new InvalidOperationException("Unknown PanelType");
			}
		}

		private void ProcessWall(CellData cdata, out Wall wall)
		{
			switch (cdata.Wall.Type)
			{
				case WallType.None:
					wall = default(Wall);
					break;

				case WallType.Hard:
				case WallType.Edge:
					string name = Enum.GetName(typeof(WallType), cdata.Wall.Type);
					wall = new Wall()
					{
						Type = (global::WallType)Enum.Parse(typeof(global::WallType), name),
						Option = 0,
					};
					break;

				case WallType.Breakable:
					wall = new Wall()
					{
						Type = global::WallType.Breakable,
						Option = 1,
					};
					break;

				default:
					throw new InvalidOperationException("Unknown WallType");
			}
		}

		private void ProcessItem(CellData cdata, out Item item)
		{
			string name = Enum.GetName(typeof(ItemType), cdata.Item.Type);

			switch (cdata.Item.Type)
			{
				case ItemType.None:
					item = default(Item);
					break;

				case ItemType.Small:
					if (cdata.Item.Option == 0) goto case ItemType.None;
					item = new Item()
					{
						Type = (global::ItemType)Enum.Parse(typeof(global::ItemType), name),
						Option = cdata.Item.Option,
					};
					break;

				case ItemType.Medium:
				case ItemType.Large:
					item = new Item()
					{
						Type = (global::ItemType)Enum.Parse(typeof(global::ItemType), name),
						Option = 0,
					};
					break;

				default:
					throw new InvalidOperationException("Unknown ItemType");
			}
		}

		private int CountBreakableObjects(MapData mdata)
		{
			return mdata.Layers.SelectMany(x => x.Cells).Where(x => x.Wall.Type == WallType.Breakable).Count();
		}

		private Block[] FindBlocks(LayerData ldata, Predicate<CellData> match)
		{
			HashSet<Block> blockSet = new HashSet<Block>();

			IList<CellData> cells = ldata.Cells;
			int width = ldata.Width;
			int height = ldata.Height;
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					if (match(cells[x + y * width])) DetectBlocks(ldata, match, blockSet, x, y);
				}
			}

			return blockSet.OrderBy(e => (e.Y << 24) + (e.X << 16) + (e.Height << 8) + (e.Width << 0)).ToArray();
		}

		private void DetectBlocks(LayerData ldata, Predicate<CellData> match, HashSet<Block> blocks, int x, int y)
		{
			IList<CellData> cells = ldata.Cells;
			int width = ldata.Width;
			int height = ldata.Height;

			int blockFlags = 0;
			if (x - 1 >= 0 && match(cells[(x - 1) + y * width])) blockFlags |= 0x01;
			if (x + 1 < width && match(cells[(x + 1) + y * width])) blockFlags |= 0x02;
			if (y - 1 >= 0 && match(cells[x + (y - 1) * width])) blockFlags |= 0x04;
			if (y + 1 < height && match(cells[x + (y + 1) * width])) blockFlags |= 0x08;

			Block block = new Block(x, y, 1, 1);
			int count = CountBits(blockFlags);
			switch (count)
			{
				case 0:
					blocks.Add(block);
					break;
				case 1:
					if ((blockFlags & 0x03) != 0) blocks.Add(InflateBlock(ldata, match, block, false));
					if ((blockFlags & 0x0c) != 0) blocks.Add(InflateBlock(ldata, match, block, true));
					break;
				case 2:
					if ((blockFlags & 0x03) != 0 && (blockFlags & 0x0c) != 0)
					{
						blocks.Add(InflateBlock(ldata, match, InflateBlock(ldata, match, block, false), true));
						blocks.Add(InflateBlock(ldata, match, InflateBlock(ldata, match, block, true), false));
					}
					break;
				case 3:
					if ((blockFlags & 0x03) == 0x03)
					{
						blocks.Add(InflateBlock(ldata, match, InflateBlock(ldata, match, block, true), false));
					}
					else if ((blockFlags & 0x0c) == 0x0c)
					{
						blocks.Add(InflateBlock(ldata, match, InflateBlock(ldata, match, block, false), true));
					}
					break;
				case 4:
					break;
			}

		}

		private Block InflateBlock(LayerData ldata, Predicate<CellData> match, Block block, bool vertical)
		{
			IList<CellData> cells = ldata.Cells;
			int width = ldata.Width;
			int height = ldata.Height;

			if (vertical)
			{
				while (block.Y - 1 >= 0)
				{
					for (int i = 0; i < block.Width; i++)
					{
						if (!match(cells[(block.X + i) + (block.Y - 1) * width])) goto PosY;
					}
					block.Y--;
				}
			PosY:
				while (block.Y + block.Height < height)
				{
					for (int i = 0; i < block.Width; i++)
					{
						if (!match(cells[(block.X + i) + (block.Y + block.Height) * width])) goto End;
					}
					block.Height++;
				}
			}
			else
			{
				while (block.X - 1 >= 0)
				{
					for (int i = 0; i < block.Height; i++)
					{
						if (!match(cells[(block.X - 1) + (block.Y + i) * width])) goto PosX;
					}
					block.X--;
				}
			PosX:
				while (block.X + block.Width < width)
				{
					for (int i = 0; i < block.Height; i++)
					{
						if (!match(cells[(block.X + block.Width) + (block.Y + i) * width])) goto End;
					}
					block.Width++;
				}
			}

		End:
			return block;
		}

		private bool IsBoundary(CellData cdata, out int option)
		{
			int rough = 0;
			for (int dy = 0; dy < 3; dy++)
			{
				for (int dx = 0; dx < 3; dx++)
				{
					int i = dy * 3 + dx;
					int x = cdata.X + dx - 1;
					int y = cdata.Y + dy - 1;

					if (x >= 0 && x < cdata.Layer.Width && y >= 0 && y < cdata.Layer.Height
						&& cdata.Layer.Cells[x + y * cdata.Layer.Width].Ground.Type == GroundType.Rough)
					{
						rough |= (1 << i);
					}
				}
			}

			if (rough != 0)
			{
				int map = GetEnvironmentMap(rough);
				int count = CountBits(rough & 0xaaaa);
				if (count == 0)
				{
					if ((map & 0x10) != 0) option = 1;
					else if ((map & 0x40) != 0) option = 3;
					else if ((map & 0x04) != 0) option = 6;
					else if ((map & 0x01) != 0) option = 8;
					else goto Error;
				}
				else if (count == 1)
				{
					if ((map & 0x25) == 0x20) option = 2;
					else if ((map & 0xc1) == 0x80) option = 4;
					else if ((map & 0x1c) == 0x08) option = 5;
					else if ((map & 0x52) == 0x02) option = 7;
					else goto Error;
				}
				else if (count == 2)
				{
					if ((map & 0xa1) == 0xa0) option = 9;
					else if ((map & 0x2c) == 0x28) option = 10;
					else if ((map & 0xc2) == 0x82) option = 11;
					else if ((map & 0x1a) == 0x0a) option = 12;
					else goto Error;
				}
				else goto Error;

				return true;

			Error:
				throw new InvalidOperationException("Invalid Arrange");
			}

			option = -1;
			return false;
		}

		private void GetCrackOption(CellData cdata, out int option)
		{
			int crack = 0;
			for (int dy = 0; dy < 3; dy++)
			{
				for (int dx = 0; dx < 3; dx++)
				{
					int i = dy * 3 + dx;
					int x = cdata.X + dx - 1;
					int y = cdata.Y + dy - 1;

					if (x < 0 || x >= cdata.Layer.Width || y < 0 || y >= cdata.Layer.Height
						|| cdata.Layer.Cells[x + y * cdata.Layer.Width].Panel.Type == PanelType.Crack)
					{
						crack |= (1 << i);
					}
				}
			}

			int map = GetEnvironmentMap(crack);
			int crscount = CountBits(crack & 0xaaaa);
			if (crscount == 1)
			{
				if ((map & 0x02) != 0) option = 1;
				else if ((map & 0x08) != 0) option = 2;
				else if ((map & 0x80) != 0) option = 3;
				else if ((map & 0x20) != 0) option = 4;
				else goto Error;
			}
			else if (crscount == 2)
			{
				if ((map & 0x0b) == 0x0b) option = 5;
				else if ((map & 0x86) == 0x86) option = 6;
				else if ((map & 0x22) == 0x22) option = 7;
				else if ((map & 0x88) == 0x88) option = 8;
				else if ((map & 0x68) == 0x68) option = 9;
				else if ((map & 0xb0) == 0xb0) option = 10;
				else goto Error;
			}
			else if (crscount == 3)
			{
				if ((map & 0x8f) == 0x8f) option = 11;
				else if ((map & 0x6b) == 0x6b) option = 12;
				else if ((map & 0xb6) == 0xb6) option = 13;
				else if ((map & 0xf8) == 0xf8) option = 14;
				else goto Error;
			}
			else if (crscount == 4)
			{
				int diagcount = CountBits(crack & 0x5555) - 1;
				if (diagcount == 2)
				{
					if ((map & 0xaf) == 0xaf) option = 19;
					else if ((map & 0xeb) == 0xeb) option = 20;
					else if ((map & 0xbe) == 0xbe) option = 21;
					else if ((map & 0xfa) == 0xfa) option = 22;
					else goto Error;
				}
				else if (diagcount == 3)
				{
					if ((map & 0xef) == 0xef) option = 15;
					else if ((map & 0xbf) == 0xbf) option = 16;
					else if ((map & 0xfb) == 0xfb) option = 17;
					else if ((map & 0xfe) == 0xfe) option = 18;
					else goto Error;
				}
				else if (diagcount == 4)
				{
					option = 23;
				}
				else goto Error;
			}
			else goto Error;

			return;

		Error:
			throw new InvalidOperationException("Invalid Arrange");
		}

		private int GetItemPoint(ItemData idata)
		{
			switch (idata.Type)
			{
				case ItemType.None: return 0;
				case ItemType.Small: return GameConstants.SmallItemPoint * CountBits(idata.Option);
				case ItemType.Medium: return GameConstants.MediumItemPoint;
				case ItemType.Large: return GameConstants.LargeItemPoint;
				default:
					throw new InvalidOperationException("Unknown ItemType");
			}
		}

		private int GetEnvironmentMap(int bits)
		{
			// 001 002 004    L1 L2 L4
			// 008 010 020 => L8 -- H8
			// 040 080 100    H4 H2 H1

			int x = ((bits & 0x01e0) >> 5);
			x = ((x & 0x03) << 2) | ((x & 0x0c) >> 2);
			x = ((x & 0x05) << 1) | ((x & 0x0a) >> 1);

			int low = bits & 0x0f;
			int high = x;

			return (low + (high << 4));
		}

		private int CountBits(int value)
		{
			unchecked
			{
				uint bits = (uint)value;
				bits = (bits & 0x55555555) + (bits >> 1 & 0x55555555);
				bits = (bits & 0x33333333) + (bits >> 2 & 0x33333333);
				bits = (bits & 0x0f0f0f0f) + (bits >> 4 & 0x0f0f0f0f);
				bits = (bits & 0x00ff00ff) + (bits >> 8 & 0x00ff00ff);
				bits = (bits & 0x0000ffff) + (bits >> 16 & 0x0000ffff);
				return (int)bits;
			}
		}

		#endregion
	}
}
