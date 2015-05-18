using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public struct MapRecord : ISerializable
{
	#region Constants

	private const string AccomplishmentName = "accomplishment";

	private const string HighScoreName = "high_score";

	#endregion

	#region Fields

	public int Accomplishment;

	public int HighScore;

	#endregion

	#region Properties

	public bool Completed
	{
		get { return ((Accomplishment & (1 << -1)) != 0); }
		set
		{
			if (value)
			{
				Accomplishment |= (1 << -1);
			}
			else
			{
				Accomplishment &= ~(1 << -1);
			}
		}
	}

	public bool Mission1
	{
		get { return this[0]; }
		set { this[0] = value; }
	}

	public bool Mission2
	{
		get { return this[1]; }
		set { this[1] = value; }
	}

	public bool Mission3
	{
		get { return this[2]; }
		set { this[2] = value; }
	}

	public bool this[int index]
	{
		get { return ((Accomplishment & (1 << index)) != 0); }
		set
		{
			if (value)
			{
				Accomplishment |= (1 << index);
			}
			else
			{
				Accomplishment &= ~(1 << index);
			}
		}
	}

	#endregion

	#region Constructors

	private MapRecord(SerializationInfo info, StreamingContext context)
	{
		this.Accomplishment = info.GetInt32(AccomplishmentName);
		this.HighScore = info.GetInt32(HighScoreName);
	}

	#endregion

	#region Methods

	public MapRecord Merge(MapRecord other)
	{
		MapRecord result;
		result.Accomplishment = this.Accomplishment | other.Accomplishment;
		result.HighScore = Mathf.Max(this.HighScore, other.HighScore);
		return result;
	}

	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(AccomplishmentName, this.Accomplishment);
		info.AddValue(HighScoreName, this.HighScore);
	}

	#endregion
}
