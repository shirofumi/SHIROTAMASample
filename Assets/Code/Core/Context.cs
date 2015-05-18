using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class Context : SingletonMonoBehaviour<Context>
{
	#region Fields

	[SerializeField]
	private readonly StringDictionary data = new StringDictionary();

	[NonSerialized]
	private readonly StringDictionary cache = new StringDictionary();

	#endregion

	#region Properties

	public static GameObject GameObject
	{
		get { return Instance.gameObject; }
	}

	public static Transform Transform
	{
		get { return Instance.transform; }
	}

	public static Dictionary<string, object> Data
	{
		get { return Instance.data; }
	}

	public static Dictionary<string, object> Cache
	{
		get { return Instance.cache; }
	}

	#endregion

	#region Messages

	private new void Awake()
	{
		base.Awake();

		DontDestroyOnLoad(gameObject);

		IContextInitializer initializer = GetComponent<IContextInitializer>();
		if (initializer != null)
		{
			initializer.InitializeContext();
		}
	}

	private new void OnDestroy()
	{
		base.OnDestroy();

		Destroy(gameObject);
	}

	#endregion

}