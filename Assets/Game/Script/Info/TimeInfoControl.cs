using UnityEngine;

public class TimeInfoControl : MonoBehaviour
{
	#region Fields

	private NumberTextSetter minuteSetter;

	private NumberTextSetter secondSetter;

	#endregion

	#region Messages

	private void Awake()
	{
		this.minuteSetter = transform.Find("Minute").GetComponentInChildren<NumberTextSetter>();
		this.secondSetter = transform.Find("Second").GetComponentInChildren<NumberTextSetter>();
	}

	private void Update()
	{
		minuteSetter.Number = TimeManager.Minute;
		secondSetter.Number = TimeManager.Second;
	}

	#endregion
}
