using System;
using UnityEngine.Events;

[Serializable]
public class GamePhaseChangedEvent : UnityEvent<GamePhaseChangedEventData> { }

[Serializable]
public struct GamePhaseChangedEventData
{
	#region Fields

	private readonly GamePhase previous;

	private readonly GamePhase next;

	#endregion

	#region Properties

	public GamePhase Previous { get { return previous; } }

	public GamePhase Next { get { return next; } }

	#endregion

	#region Constructors

	public GamePhaseChangedEventData(GamePhase previous, GamePhase next)
	{
		this.previous = previous;
		this.next = next;
	}

	#endregion
}
