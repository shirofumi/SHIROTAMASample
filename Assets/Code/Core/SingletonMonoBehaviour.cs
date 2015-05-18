using System;
using UnityEngine;

using UnityObject = UnityEngine.Object;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	#region Fields

	private static T instance;

	#endregion

	#region Properties

	public static bool IsAlive
	{
		get { return (instance != null); }
	}

	public static T Instance
	{
		get { return instance ?? FindInstance(); }
	}

	#endregion

	#region Messages

	protected void Awake()
	{
		if (instance == null)
		{
			instance = this as T;

			if (instance == null)
			{
				Debug.LogWarning("Unexpected type parameter.", gameObject);

				FindInstance();
			}
		}
		else if(!ReferenceEquals(instance, this))
		{
#if UNITY_EDITOR
			if (!this.CompareTag("EditorOnly"))
			{
				Debug.LogWarning("Duplication of '" + typeof(T) + "' instances.", gameObject);
			}
#endif

			Destroy(this);
		}
	}

	protected void OnDestroy()
	{
		if (ReferenceEquals(instance, this))
		{
			instance = null;
		}
	}

	private static T FindInstance()
	{
		instance = UnityObject.FindObjectOfType<T>();
		if (instance == null)
		{
			throw new InvalidOperationException("No '" + typeof(T) + "' instance.");
		}

		return instance;
	}

	#endregion
}