
public class AccelerationPanelEffect : SingletonParticleEffect<AccelerationPanelEffect>
{
	#region Methods

	public new static void Play()
	{
		((SingletonParticleEffect<AccelerationPanelEffect>)Instance).Play();
	}

	public new static void Stop()
	{
		((SingletonParticleEffect<AccelerationPanelEffect>)Instance).Stop();
	}

	#endregion
}