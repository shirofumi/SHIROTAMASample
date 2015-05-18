using UnityEngine;

public class HealingAreaPanelControl : EffectPanelBehaviour
{
	#region Fields

	private static bool locked;

	#endregion

	#region Constructors

	static HealingAreaPanelControl()
	{
		CellManager.Reset += () => locked = false;
	}

	#endregion

	#region Methods

	protected override void BeginEffect(BallControl ball)
	{
		HealingPanelEffect.Play();

		GameSEGlobalSource.Play(CellManager.SEs.Heal);
	}

	protected override void EndEffect(BallControl ball)
	{
		HealingPanelEffect.Stop();
	}

	protected override void ApplyEffect(BallControl ball)
	{
		if (locked) return;

		locked = true;

		if (ball.IsOutOfControl) return;

		float point = CellManager.Params.HealingAreaPanelHealingPoint * Time.deltaTime;
		ball.AddExcitingPoint(-point);
	}

	#endregion
}
