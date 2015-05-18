using UnityEngine;

public class LayerInfoControl : MonoBehaviour
{
	#region Fields

	private NumberTextSetter setter;

	#endregion

	#region Messages

	private void Awake()
	{
		this.setter = GetComponentInChildren<NumberTextSetter>();
	}

	private void Update()
	{
		this.setter.Number = GameScene.Depth + 1;
	}

	#endregion
}
