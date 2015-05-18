using UnityEngine;

public class PauseScreenControl : MonoBehaviour
{
	#region Methods

	public void Back()
	{
		if (enabled)
		{
			SystemSoundSource.Back();

			GameScene.Instance.Back();
		}
	}

	public void Restart()
	{
		if (enabled)
		{
			SystemSoundSource.Start();

			GameScene.Instance.Restart();
		}
	}

	public void ToHome()
	{
		if (enabled)
		{
			SystemSoundSource.Start();

			GameScene.Instance.ToHome();
		}
	}

	#endregion
}
