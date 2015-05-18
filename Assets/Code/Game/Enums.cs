
public enum Theme
{
	None = 0x00,
	Wood = 0x01,
	Grass = 0x02,
}

public enum GroundType
{
	None = 0x00,
	Normal = 0x01,
	Rough = 0x02,
	NormalToRough = 0x03,
}

public enum PanelType
{
	None = 0x00,
	Acceleration = 0x01,
	DirectionChange = 0x02,
	RotationCW = 0x03,
	RotationCCW = 0x04,
	ExcitingArea = 0x05,
	HealingArea = 0x06,
	Pit = 0x10,
	Booster = 0x20,
	Stopper = 0x21,
	Crack = 0x40,
}

public enum WallType
{
	None = 0x00,
	Hard = 0x01,
	Breakable = 0x10,
	Edge = 0x40,
}

public enum ItemType
{
	None = 0x00,
	Small = 0x01,
	Medium = 0x02,
	Large = 0x03,
}

public enum BarrierType
{
	None = 0x00,
	Stick = 0x01,
	Circle = 0x02,
}

public enum BarrierScale
{
	None = 0x00,
	Small = 0x01,
	Medium = 0x02,
	Large = 0x03,
}

public enum MissionType
{
	None = 0x00,
	FastCompletion = 0x01,
	LessBarrier = 0x02,
	LessHitting = 0x03,
	MoreHitting = 0x04,
	LessSlugging = 0x05,
	MoreSlugging = 0x06,
	MoreAcceleration = 0x10,
	MoreDicretionChange = 0x11,
	MoreRotation = 0x12,
	InitialBarrier = 0x40,
	ExciteBall = 0x41,
	DontExciteBall = 0x42,
	StopBall = 0x43,
	DontStopBall = 0x44,
	BreakAll = 0x45,
}