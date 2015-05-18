using UnityEngine;
using System;

[RequireComponent(typeof(ParticleSystem))]
public abstract class SingletonParticleEffect<T> : SingletonMonoBehaviour<T> where T : MonoBehaviour
{
	#region Fields

	public bool UseTimer;

	public float Duration;

	private float startTime;

	private new ParticleSystem particleSystem;

	#endregion

	#region Messages

	private new void Awake()
	{
		this.particleSystem = GetComponent<ParticleSystem>();
	}

	private void Start()
	{
		Renderer renderer = GetComponent<Renderer>();
		renderer.sortingLayerName = SortingLayers.FieldEffect;
	}

	private void Update()
	{
		if (UseTimer && Time.time - startTime > Duration)
		{
			Stop();
		}
	}

	#endregion

	#region Methods

	public void Play()
	{
		if (particleSystem.isStopped || particleSystem.isPaused)
		{
			startTime = Time.time;

			particleSystem.Play(false);
		}
	}

	public void Stop()
	{
		if (particleSystem.isPlaying)
		{
			startTime = Single.PositiveInfinity;

			particleSystem.Stop(false);
		}
	}

	#endregion
}
