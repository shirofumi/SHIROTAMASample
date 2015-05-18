using UnityEngine;

public class FieldEffectManager : EffectManager<FieldEffectManager>
{
	#region Methods

	public static GameObject Add(GameObject original)
	{
		return Instance._Add(original);
	}

	private GameObject _Add(GameObject original)
	{
		GameObject instance = Instantiate(original);
		instance.transform.SetParent(this.transform, false);
		return instance;
	}

	#endregion
}
