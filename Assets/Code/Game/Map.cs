using System;
using UnityEngine;

public class Map : ScriptableObject
{
	#region Fields

	public MapID ID;

	public int Width;

	public int Height;

	public int LimitTime;

	public Mission[] Missions;

	public Layer[] Layers;

	[NonSerialized]
	public int Points;

	#endregion

	#region Messages

	private void OnEnable()
	{
		AggregateLayerData();
	}

	#endregion

	#region Methods

	private void AggregateLayerData()
	{
		int points = 0;

		if (Layers != null)
		{
			foreach (Layer layer in Layers)
			{
				points += layer.Points;
			}
		}

		this.Points = points;
	}


	#endregion
}