using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResultScreenControl : MonoBehaviour, IPointerClickHandler
{
	#region Fields

	private NumberTextSetter points;

	private NumberTextSetter baseScore;

	private NumberTextSetter timeBonus;

	private NumberTextSetter totalScore;

	#endregion

	#region Messages

	private void Awake()
	{
		NumberTextSetter[] setters = GetComponentsInChildren<NumberTextSetter>();
		for (int i = 0; i < setters.Length; i++)
		{
			NumberTextSetter setter = setters[i];
			switch (setter.gameObject.name)
			{
				case "Points":
					points = setter;
					break;
				case "BaseScore":
					baseScore = setter;
					break;
				case "TimeBonus":
					timeBonus = setter;
					break;
				case "TotalScore":
					totalScore = setter;
					break;
			}
		}
	}

	private void OnEnable()
	{
		points.Number = GameScene.Map.Points;
		baseScore.Number = ScoreManager.Score;
		timeBonus.Number = ScoreManager.TimeBonus;
		totalScore.Number = ScoreManager.TotalScore;
	}

	#endregion

	#region Methods

	public void OnPointerClick(PointerEventData eventData)
	{
		GamePhaseManager.Next(GamePhase.Ending);
	}

	
	#endregion
}
