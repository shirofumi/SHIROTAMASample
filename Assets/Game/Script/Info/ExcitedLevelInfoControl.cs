using UnityEngine;
using UnityEngine.UI;

public class ExcitedLevelInfoControl : MonoBehaviour
{
	#region Fields

	public float DeadAngle;

	private Image gauge;

	private float current;

	private Animator animator;

	private int id;

	#endregion

	#region Messages

	private void Awake()
	{
		this.gauge = transform.Find("Container").Find("Gauge").GetComponent<Image>();
		this.animator = GetComponent<Animator>();
		this.id = Animator.StringToHash("Quake");
	}

	private void Update()
	{
		BallControl ball = GameScene.Ball;

		float value = ball.ExcitedLevel;
		if (current != value)
		{
			if (ball.IsOutOfControl)
			{
				gauge.fillAmount = 1.0f;

				animator.SetBool(id, true);
			}
			else
			{
				float amount = Mathf.Clamp01(value / GameConstants.MaxExcitedLevel);

				gauge.fillAmount = amount;

				animator.SetBool(id, false);
			}

			current = value;
		}
	}

	#endregion

	#region Methods

	private float GetFillAmount(float amount)
	{
		float offset = DeadAngle * (0.5f / 360.0f);
		return
			amount < Mathf.Epsilon ? 0.0f :
			amount > 1.0f - Mathf.Epsilon ? 1.0f :
			amount + offset;
	}

	#endregion
}
