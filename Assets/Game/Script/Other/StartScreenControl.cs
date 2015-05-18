using UnityEngine;

public class StartScreenControl : MonoBehaviour
{
	#region Messages

	private void Start()
	{
		MapID mapID = GameScene.Map.ID;
		int depth = GameScene.Depth + 1;

		NumberTextSetter[] setters = GetComponentsInChildren<NumberTextSetter>();
		for (int i = 0; i < setters.Length; i++)
		{
			NumberTextSetter setter = setters[i];
			switch (setter.gameObject.name)
			{
				case "Primary":
					setter.Number = mapID.Primary;
					break;
				case "Secondary":
					setter.Number = mapID.Secondary;
					break;
				case "Layer":
					setter.Number = depth;
					break;
			}
		}
	}

	private void GetReady()
	{
		GamePhaseManager.Next(GamePhase.Ready);
	}

	#endregion
}
