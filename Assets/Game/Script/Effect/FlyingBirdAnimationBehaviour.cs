using UnityEngine;

[SharedBetweenAnimators]
public class FlyingBirdAnimationBehaviour : StateMachineBehaviour
{
	#region Constants

	private const string BaseLayer = "Base Layer";

	private const string BallStateMachine = "Ball";

	private const string BirdStateMachine = "Bird";

	private const string FlyState = "Fly";

	private const string FlyingParameter = "Flying";

	#endregion

	#region Fields

	private static readonly int ball = Animator.StringToHash(BaseLayer + "." + BallStateMachine);

	private static readonly int bird = Animator.StringToHash(BaseLayer + "." + BirdStateMachine);

	private static readonly int fly = Animator.StringToHash(BaseLayer + "." + BirdStateMachine + "." + FlyState);

	private static readonly int flying = Animator.StringToHash(FlyingParameter);

	#endregion

	#region Methods

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (stateInfo.fullPathHash == fly)
		{
			animator.SetBool(flying, true);
		}
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (stateInfo.fullPathHash == fly)
		{
			animator.SetBool(flying, false);
		}
	}

	public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
	{
		FlyingBirdEffect effect = animator.GetComponent<FlyingBirdEffect>();
		if (effect != null)
		{
			if (stateMachinePathHash == ball)
			{
				effect.Ball.gameObject.SetActive(true);
			}
			else if (stateMachinePathHash == bird)
			{
				effect.Bird.gameObject.SetActive(true);
			}
		}
	}

	public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
	{
		FlyingBirdEffect effect = animator.GetComponent<FlyingBirdEffect>();
		if (effect != null)
		{
			if (stateMachinePathHash == ball)
			{
				effect.Ball.gameObject.SetActive(false);
			}
			else if (stateMachinePathHash == bird)
			{
				effect.Bird.gameObject.SetActive(false);
			}
		}
	}

	#endregion
}
