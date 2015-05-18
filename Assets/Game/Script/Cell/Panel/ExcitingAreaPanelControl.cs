using UnityEngine;

public class ExcitingAreaPanelControl : EffectPanelBehaviour
{
	#region Fields

	private static bool locked;

	#endregion

	#region Constructors

	static ExcitingAreaPanelControl()
	{
		CellManager.Reset += () => locked = false;
	}

	#endregion

	#region Methods

	protected override void BeginEffect(BallControl ball)
	{
		ExcitingPanelEffect.Play();

		GameSEGlobalSource.Play(CellManager.SEs.Excite);
	}

	protected override void EndEffect(BallControl ball)
	{
		ExcitingPanelEffect.Stop();
	}

	protected override void ApplyEffect(BallControl ball)
	{
		if (locked) return;

		locked = true;

		if (ball.IsOutOfControl) return;

		float point = CellManager.Params.ExcitingAreaPanelExcitingPoint * Time.deltaTime;
		ball.AddExcitingPoint(point);
	}

	#endregion
}
