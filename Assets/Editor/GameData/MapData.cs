using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Editor.GameData
{
	public class MapData
	{
		#region Fields

		public readonly string Name;

		public readonly int Level;

		public readonly int Index;

		public readonly int Width;

		public readonly int Height;

		public readonly int LimitTime;

		public readonly ReadOnlyCollection<MissionData> Missions;

		public readonly ReadOnlyCollection<LayerData> Layers;

		private static readonly Regex NameRegex = new Regex(@"^(\d+)-(\d+)$", RegexOptions.Compiled);

		#endregion

		#region Constructors

		public MapData(XElement element)
		{
			this.Name = (string)element.Attribute("name");
			this.Width = (int)element.Attribute("width");
			this.Height = (int)element.Attribute("height");
			this.LimitTime = (int)element.Attribute("limit_time");
			this.Missions = element.Element("mission_list").Elements("mission").Select(x => new MissionData((int)x)).ToList().AsReadOnly();
			this.Layers = element.Element("layer_list").Elements("layer").Select((x, n) => new LayerData(x, this, n)).ToList().AsReadOnly();

			Match match = NameRegex.Match(Name);
			this.Level = Int32.Parse(match.Groups[1].Value);
			this.Index = Int32.Parse(match.Groups[2].Value);
		}

		#endregion
	}
}
