using System;

[Flags]
public enum GamePhase
{
	None = 0x0000,
	Beginning = 0x0001,
	Ending = 0x0002,
	Ready = 0x0010,
	Running = 0x0020,
	Completed = 0x0040,
	Missed = 0x0080,
	Result = 0x0100,
	Failed = 0x0200,
	Pause = 0x1000,

	All = Beginning | Ending | Ready | Running | Completed | Missed | Result | Failed | Pause,
}