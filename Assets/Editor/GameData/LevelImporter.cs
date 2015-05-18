using Fsp.Unity.Editor.GameData;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Editor.GameData
{
	[GameDataImporter(typeof(LevelData), ".mapproj", Priority=1)]
	public class LevelImporter : GameDataImporter<LevelData>
	{
		#region Methods

		protected override LevelData Import(GDContext context, Stream stream)
		{
			LevelData level = LoadLevelData(stream);

			ForkMapProcess(context, level.Maps);

			InitLevelProcess(context, level);

			return level;
		}

		private LevelData LoadLevelData(Stream stream)
		{
			XDocument xdoc = XDocument.Load(stream);

			LevelData level = new LevelData(xdoc.Element("project"));

			return level;
		}

		private void ForkMapProcess(GDContext context, IEnumerable<MapData> maps)
		{
			IGameDataProcessor processor;
			if (GameDataProcessManager.TryGetProcessor(typeof(MapData), out processor))
			{
				string dir = Path.GetDirectoryName(context.AssetPath);
				dir = Path.Combine(dir, "Resources");
				dir = Path.Combine(dir, "Map");

				GDProcess process = new GDProcess(null, processor, SimpleGameDataAssetCreator.Instance);
				foreach (MapData map in maps)
				{
					string path = Path.Combine(dir, map.Name + ".asset");
					path = path.Replace(Path.DirectorySeparatorChar, '/');

					context.Fork(process, path, map);
				}
			}
		}

		private void InitLevelProcess(GDContext context, LevelData level)
		{
			string dir = Path.GetDirectoryName(context.AssetPath);
			dir = Path.Combine(dir, "Resources");
			dir = Path.Combine(dir, "Level");

			string path = Path.Combine(dir, "Level" + level.Index + ".asset");
			path = path.Replace(Path.DirectorySeparatorChar, '/');

			context.AssetPath = path;
		}

		#endregion
	}
}
