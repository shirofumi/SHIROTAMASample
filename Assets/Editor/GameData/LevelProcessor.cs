using System.Collections.Generic;
using UnityEngine;

using Fsp.Unity.Editor.GameData;

namespace Editor.GameData
{
	[GameDataProcessor(typeof(LevelData))]
	public class LevelProcessor : GameDataProcessor<LevelData>
	{
		#region Methods

		protected override object Process(GDContext context, LevelData input)
		{
			Level level = ScriptableObject.CreateInstance<Level>();

			level.Index = input.Index;

			List<MapSummary> maps = new List<MapSummary>();
			foreach (MapData mdata in input.Maps)
			{
				MapSummary summary;
				summary.Missions = null;
				summary.LayerCount = mdata.Layers.Count;

				maps.Add(summary);
			}
			level.Maps = maps.ToArray();

			return level;
		}

		#endregion
	}
}
