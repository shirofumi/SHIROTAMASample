
public class GameController : SingletonMonoBehaviour<GameController>
{
	#region Messages

	private new void Awake()
	{
		base.Awake();

		DontDestroyOnLoad(gameObject);
	}

	private new void OnDestroy()
	{
		base.OnDestroy();

		Destroy(gameObject);
	}

	#endregion
}
