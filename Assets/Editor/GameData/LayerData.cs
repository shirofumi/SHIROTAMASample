using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace Editor.GameData
{
	public class LayerData
	{
		#region Fields

		public readonly MapData Map;

		public readonly int Depth;

		public readonly Theme Theme;

		public readonly float StartPositionX;

		public readonly float StartPositionY;

		public readonly BarrierData Barrier;

		public readonly ReadOnlyCollection<CellData> Cells;

		#endregion

		#region Properties

		public int Width
		{
			get { return Map.Width; }
		}

		public int Height
		{
			get { return Map.Height; }
		}

		#endregion

		#region Constructors

		public LayerData(XElement element, MapData map, int index)
		{
			this.Map = map;
			this.Depth = index;
			this.Theme = (Theme)Enum.Parse(typeof(Theme), (string)element.Attribute("theme"), true);
			this.StartPositionX = (float)element.Attribute("start_position_x");
			this.StartPositionY = (float)element.Attribute("start_position_y");
			this.Barrier = new BarrierData(element.Element("barrier_info"));
			this.Cells = element.Element("cell_list").Elements("cell").Select((x, n) => new CellData(x, this, n)).ToList().AsReadOnly();
		}

		#endregion
	}
}
