using UnityEngine;

namespace LevelSelection
{
	public class LevelSelectionScene : SingletonMonoBehaviour<LevelSelectionScene>
	{
		#region Messages

		public void Start()
		{
			ScreenFadeManager.FadeIn();
		}

		#endregion

		#region Methods

		public void SelectLevel(int level)
		{
			ScreenFadeManager.FadeOut(() =>
			{
				Context.Data[Keys.Level] = level;

				Application.LoadLevel(Scenes.MapSelection);
			});
		}

		public void Back()
		{
			SystemSoundSource.Back();

			ScreenFadeManager.FadeOut(() =>
			{
				Application.LoadLevel(Scenes.Title);
			});
		}

		#endregion
	}
}
