using System.Xml.Linq;

namespace Editor.GameData
{
	public class CellData
	{
		#region Fields

		public readonly LayerData Layer;

		public readonly int Index;

		public readonly GroundData Ground;

		public readonly PanelData Panel;

		public readonly WallData Wall;

		public readonly ItemData Item;

		#endregion

		#region Properties

		public int X
		{
			get { return (Index % Layer.Width); }
		}

		public int Y
		{
			get { return (Index / Layer.Width); }
		}

		#endregion

		#region Constructors

		public CellData(XElement element, LayerData layer, int index)
		{
			this.Layer = layer;
			this.Index = index;
			this.Ground = new GroundData((int)element.Attribute("ground"));
			this.Panel = new PanelData((int)element.Attribute("panel"));
			this.Wall = new WallData((int)element.Attribute("wall"));
			this.Item = new ItemData((int)element.Attribute("item"));
		}

		#endregion
	}
}
