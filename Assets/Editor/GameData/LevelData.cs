using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace Editor.GameData
{
	public class LevelData
	{
		#region Fields

		public readonly int Index;

		public readonly ReadOnlyCollection<MapData> Maps;

		#endregion

		#region Constructors

		public LevelData(XElement element)
		{
			this.Maps = element.Elements("map").Select(x => new MapData(x)).ToList().AsReadOnly();

			this.Index = Maps.Select(x => x.Level).Aggregate((x, y) => x == y ? x : -1);
		}

		#endregion
	}
}
