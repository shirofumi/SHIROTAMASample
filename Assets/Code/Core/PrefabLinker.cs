using UnityEngine;

using UnityObject = UnityEngine.Object;

public class PrefabLinker : MonoBehaviour
{
	#region Fields

	[SerializeField]
	private GameObject prefab;

	#endregion

	#region Messages

	private void Awake()
	{
		GameObject instance = UnityObject.Instantiate(prefab);
		instance.name = prefab.name;
		instance.transform.SetParent(this.transform, false);
	}

	private void Reset()
	{
		this.prefab = null;
	}

	#endregion
}