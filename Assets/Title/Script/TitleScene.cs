using UnityEngine;

namespace Title
{
	public class TitleScene : SingletonMonoBehaviour<TitleScene>
	{
		#region Messages

		private void Start()
		{
			RecordManager.Load();

			ScreenFadeManager.FadeIn();
		}

		#endregion

		#region Methods

		public void StartGame()
		{
			SystemSoundSource.Start();

			ScreenFadeManager.FadeOut(() =>
			{
				if (SuspensionManager.Load())
				{
					Application.LoadLevel(Scenes.Main);
				}
				else
				{
					Application.LoadLevel(Scenes.LevelSelection);
				}
			});
		}

		public void Back()
		{
			SystemSoundSource.Back();

			ScreenFadeManager.FadeOut(() =>
			{
				Application.Quit();
			});
		}

		#endregion
	}
}
