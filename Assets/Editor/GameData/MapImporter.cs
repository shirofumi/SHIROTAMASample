using Fsp.Unity.Editor.GameData;
using System.IO;
using System.Xml.Linq;

namespace Editor.GameData
{
	[GameDataImporter(typeof(MapData), ".mapproj")]
	public class MapImporter : GameDataImporter<MapData>
	{
		#region Methods

		protected override MapData Import(GDContext context, Stream stream)
		{
			XDocument xdoc = XDocument.Load(stream);

			string dir = Path.GetDirectoryName(context.AssetPath);
			dir = Path.Combine(dir, "Resources");
			dir = Path.Combine(dir, "Map");

			GDProcess process = new GDProcess(null, context.Process.Processor, SimpleGameDataAssetCreator.Instance);
			foreach (XElement xmap in xdoc.Element("project").Elements("map"))
			{
				MapData map = new MapData(xmap);

				string path = Path.Combine(dir, map.Name + ".asset");
				path = path.Replace(Path.DirectorySeparatorChar, '/');

				context.Fork(process, path, map);
			}

			context.Cancel();

			return null;
		}

		#endregion
	}
}
