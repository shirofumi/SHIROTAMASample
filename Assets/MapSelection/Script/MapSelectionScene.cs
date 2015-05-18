using System;
using UnityEngine;

namespace MapSelection
{
	public class MapSelectionScene : SingletonMonoBehaviour<MapSelectionScene>
	{
		#region Fields

		private Level level;

		private MapID current;

		#endregion

		#region Properties

		public static Level Level
		{
			get { return Instance.level; }
		}

		public static MapID Current
		{
			get { return Instance.current; }
			set { Instance.current = value; }
		}

		#endregion

		#region Messages

		private new void Awake()
		{
			base.Awake();

			int index = (int)Context.Data[Keys.Level];
			this.level = Resources.Load<Level>(Level.GetResourceName(index));

			object value;
			if (Context.Data.TryGetValue(Keys.MapID, out value))
			{
				current = (MapID)value;
			}
		}

		private void Start()
		{
			ScreenFadeManager.FadeIn();
		}

		#endregion

		#region Methods

		public void StartGame()
		{
			if (Current == null) return;

			ScreenFadeManager.FadeOut(() =>
			{
				Context.Data[Keys.MapID] = MapSelectionScene.Current;
				Context.Data[Keys.Depth] = 0;

				Application.LoadLevel(Scenes.Main);
			});
		}

		public void Back()
		{
			SystemSoundSource.Back();

			ScreenFadeManager.FadeOut(() =>
			{
				Context.Data.Remove(Keys.MapID);
				Context.Data.Remove(Keys.Depth);

				Application.LoadLevel(Scenes.LevelSelection);
			});
		}

		#endregion
	}
}
