using UnityEngine;

public class BirdFaceBehaviour : StateMachineBehaviour
{
	#region Params

	private static class Params
	{
		public static readonly int ElapsedTime = Animator.StringToHash("ElapsedTime");

		public static readonly int Switcher = Animator.StringToHash("Switcher");

		public static readonly int Sleepy = Animator.StringToHash("Sleepy");
	}

	#endregion

	#region States

	private static class States
	{
		public static readonly int Idle = Animator.StringToHash(BaseLayer + "." + "Idle");

		public static readonly int Wink = Animator.StringToHash(BaseLayer + "." + "Wink");
	}

	#endregion

	#region StateMachines

	private static class StateMachines
	{
		public static readonly int Sleepy = Animator.StringToHash(BaseLayer + "." + "Sleepy");

		public static readonly int Sleep = Animator.StringToHash(BaseLayer + "." + "Sleep");
	}

	#endregion

	#region Constants

	private const int WinkCount = 3;

	private const string BaseLayer = "Base Layer";

	#endregion

	#region Fields

	private float startTime;

	private int count;

	#endregion

	#region Methods

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (layerIndex == 0)
		{
			startTime = Time.time;

			if (stateInfo.fullPathHash == States.Wink)
			{
				if (++count >= WinkCount)
				{
					animator.SetBool(Params.Sleepy, true);

					count = 0;
				}
			}
			else if(stateInfo.fullPathHash != States.Idle)
			{
				int switcher = Random.Range(0, 3) - 1;
				bool sleepy = (Random.Range(0, 2) == 0);

				animator.SetInteger(Params.Switcher, switcher);
				animator.SetBool(Params.Sleepy, sleepy);
			}
		}
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (layerIndex == 0)
		{
			float elapsed = Time.time - startTime;

			animator.SetFloat(Params.ElapsedTime, elapsed);
		}
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (layerIndex == 0)
		{
			animator.SetFloat(Params.ElapsedTime, 0.0f);
		}
	}

	#endregion
}
