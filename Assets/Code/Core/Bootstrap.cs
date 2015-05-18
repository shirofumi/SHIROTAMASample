using UnityEngine;

public class Bootstrap : MonoBehaviour
{
	#region Fields

	public int Scene;

	#endregion

	#region Messages

	private void Awake()
	{
		Application.LoadLevel(Scene);
	}

	#endregion
}
