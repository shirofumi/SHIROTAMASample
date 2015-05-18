using UnityEngine;

public class FailedScreenControl : MonoBehaviour
{
	#region Methods

	public void Retry()
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
