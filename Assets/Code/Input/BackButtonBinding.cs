using UnityEngine;
using UnityEngine.Events;

public class BackButtonBinding : MonoBehaviour
{
	#region Fields

	[SerializeField]
	private UnityEvent action;

	#endregion

	#region Events

	public event UnityAction Action
	{
		add { action.AddListener(value); }
		remove { action.RemoveListener(value); }
	}

	#endregion

	#region Messages

	private void Awake()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			Destroy(this);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			action.Invoke();
		}
	}

	#endregion
}
