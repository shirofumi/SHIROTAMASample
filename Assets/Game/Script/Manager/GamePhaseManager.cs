using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GamePhaseManager : SingletonMonoBehaviour<GamePhaseManager>
{
	#region Fields

	[SerializeField]
	private GamePhaseChangedEvent gamePhaseChangedEvent;

	private readonly List<GamePhase> phaseStack = new List<GamePhase>();

	private static readonly GamePhase initialPhase = GamePhase.Beginning;

	private static readonly Dictionary<GamePhase, GamePhase> transitions = new Dictionary<GamePhase, GamePhase>()
	{
		{ GamePhase.None, GamePhase.All },
		{ GamePhase.Beginning, GamePhase.Ready },
		{ GamePhase.Ending, GamePhase.None },
		{ GamePhase.Ready, GamePhase.Running | GamePhase.Pause },
		{ GamePhase.Running, GamePhase.Completed | GamePhase.Missed | GamePhase.Pause },
		{ GamePhase.Completed, GamePhase.Ending | GamePhase.Result | GamePhase.Pause },
		{ GamePhase.Missed, GamePhase.Failed | GamePhase.Pause},
		{ GamePhase.Result, GamePhase.Ending},
		{ GamePhase.Failed, GamePhase.Ending},
		{ GamePhase.Pause, GamePhase.Ending },
	};

	#endregion

	#region Properties

	public static GamePhase Phase
	{
		get { return Instance._GetPhase(0); }
	}

	public static int PhaseStackCount
	{
		get { return Instance.phaseStack.Count; }
	}

	public static bool Ready
	{
		get { return (Phase == GamePhase.Ready); }
	}

	public static bool Running
	{
		get { return (Phase == GamePhase.Running); }
	}

	public static bool ReadyOrRunning
	{
		get { return ((Phase & (GamePhase.Ready | GamePhase.Running)) != 0); }
	}

	#endregion

	#region Events

	public static event UnityAction<GamePhaseChangedEventData> GamePhaseChanged
	{
#if UNITY_EDITOR
		add { if (UnityEditor.EditorApplication.isPlaying) Instance._GamePhaseChanged += value; }
		remove { if (UnityEditor.EditorApplication.isPlaying) Instance._GamePhaseChanged -= value; }
#else
		add { Instance._GamePhaseChanged += value; }
		remove { Instance._GamePhaseChanged -= value; }
#endif
	}

	private event UnityAction<GamePhaseChangedEventData> _GamePhaseChanged
	{
		add
		{
			if (gamePhaseChangedEvent == null) gamePhaseChangedEvent = new GamePhaseChangedEvent();

			gamePhaseChangedEvent.AddListener(value);
		}
		remove
		{
			if (gamePhaseChangedEvent == null) return;

			gamePhaseChangedEvent.RemoveListener(value);
		}
	}

	#endregion

	#region Messages

	private void Start()
	{
		_Push(initialPhase);
	}

	#endregion

	#region Methods

	public static void Next(GamePhase phase) { Instance._Next(phase); }

	public static void Push(GamePhase phase) { Instance._Push(phase); }

	public static void Pop(GamePhase phase) { Instance._Pop(phase); }

	public static int Find(GamePhase phase) { return Instance._Find(phase); }

	public static GamePhase GetPhase(int index) { return Instance._GetPhase(index); }

	public static bool HasTransition(GamePhase from, GamePhase to)
	{
		return ((transitions[from] & to) == to);
	}

	private void _Next(GamePhase phase)
	{
		if(phaseStack.Count > 0 && CanMove(phase))
		{
			GamePhase prev = _GetPhase(0);

			phaseStack[phaseStack.Count - 1] = phase;

			RaisePhaseChangedEvent(prev, phase);
		}
	}

	private void _Push(GamePhase phase)
	{
		if (CanMove(phase))
		{
			GamePhase prev = _GetPhase(0);

			phaseStack.Add(phase);

			RaisePhaseChangedEvent(prev, phase);
		}
	}

	private void _Pop(GamePhase phase)
	{
		if (phaseStack.Count > 0)
		{
			GamePhase prev = _GetPhase(0);
			if (prev == phase)
			{
				phaseStack.RemoveAt(phaseStack.Count - 1);

				RaisePhaseChangedEvent(phase, _GetPhase(0));
			}
		}
	}

	private GamePhase _GetPhase(int index)
	{
		int i = phaseStack.Count - index - 1;
		return (i >= 0 ? phaseStack[i] : GamePhase.None);
	}

	private int _Find(GamePhase phase)
	{
		for (int i = phaseStack.Count - 1; i >= 0; i--)
		{
			if ((phaseStack[i] & phase) != 0) return i;
		}

		return -1;
	}

	private bool CanMove(GamePhase phase)
	{
		GamePhase current = _GetPhase(0);
		GamePhase mask = transitions[current];

#if UNITY_EDITOR
		if (phase != GamePhase.Pause && (phase & mask) == 0)
		{
			Debug.LogWarningFormat("Cannot move from {0} to {1}.", current, phase);
		}
#endif

		return ((phase & mask) != 0);
	}

	private void RaisePhaseChangedEvent(GamePhase prev, GamePhase next)
	{
		if (gamePhaseChangedEvent != null)
		{
			GamePhaseChangedEventData data = new GamePhaseChangedEventData(prev, next);

			gamePhaseChangedEvent.Invoke(data);
		}
	}

	#endregion
}