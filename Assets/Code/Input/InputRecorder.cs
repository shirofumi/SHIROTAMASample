using System;
using UnityEngine;

public class InputRecorder
{
	#region Record

	private struct Record
	{
		public Vector2 Position;

		public float Time;
	}

	#endregion

	#region Constants

	private const int DefaultRecordSize = 64;

	#endregion

	#region Fields

	private float threshold;

	private Record start;

	private Record moving;

	private readonly RingList<Record> records;

	#endregion

	#region Properties

	public int Count { get { return records.Count; } }

	public Vector2 StartPosition { get { return start.Position; } }

	public float StartTime { get { return start.Time; } }

	public Vector2 StartMovingPosition { get { return moving.Position; } }

	public float StartMovingTime { get { return moving.Time; } }

	public Vector2 LastPosition { get { return records[0].Position; } }

	public float LastTime { get { return records[0].Time; } }

	public bool Moved { get { return (moving.Time != 0.0f); } }

	#endregion

	#region Constructors

	public InputRecorder(float moveThreshold) : this(moveThreshold, DefaultRecordSize) { }

	public InputRecorder(float moveThreshold, int capacity)
	{
		this.threshold = moveThreshold * moveThreshold;
		this.records = new RingList<Record>(capacity);
	}

	#endregion

	#region Methods

	public void Clear()
	{
		this.start = default(Record);
		this.moving = default(Record);
		this.records.Clear();
	}

	public void Add(Vector2 position, float time)
	{
		Record record;
		record.Position = position;
		record.Time = time;

		if (records.Count == 0)
		{
			start = record;
		}
		else if(!Moved && (record.Position - start.Position).sqrMagnitude > threshold)
		{
			moving = record;
		}

		records.PushFront(record);
	}

	public Vector2 GetPosition(float time)
	{
		if (records.Count == 0) throw new InvalidOperationException("No Records.");

		int index = SearchRecord(time);
		if (index == -1)
		{
			return records[0].Position;
		}
		else if (index == records.Count - 1)
		{
			return records[records.Count - 1].Position;
		}
		else
		{
			Record next = records[index];
			Record prev = records[index + 1];
			float t = (time - prev.Time) / (next.Time - prev.Time);

			return (next.Position * t + prev.Position * (1.0f - t));
		}
	}

	private int SearchRecord(float time)
	{
		int lower = 0, upper = records.Count - 1;
		while (lower <= upper)
		{
			int index = lower + (upper - lower) / 2;
			if (records[index].Time < time)
			{
				upper = index - 1;
			}
			else
			{
				lower = index + 1;
			}
		}

		return upper;
	}

	#endregion
}