using UnityEngine;

public class MapInfoControl : MonoBehaviour
{
	#region Fields

	private NumberTextSetter primarySetter;

	private NumberTextSetter secondarySetter;

	#endregion

	#region Messages

	private void Awake()
	{
		this.primarySetter = transform.Find("Primary").GetComponentInChildren<NumberTextSetter>();
		this.secondarySetter = transform.Find("Secondary").GetComponentInChildren<NumberTextSetter>();
	}

	private void Update()
	{
		MapID id = GameScene.Map.ID;
		this.primarySetter.Number = id.Primary;
		this.secondarySetter.Number = id.Secondary;
	}

	#endregion
}
