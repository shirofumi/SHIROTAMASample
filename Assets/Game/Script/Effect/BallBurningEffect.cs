
public class BallBurningEffect : SingletonParticleEffect<BallBurningEffect>
{
	#region Methods

	public new static void Play()
	{
		((SingletonParticleEffect<BallBurningEffect>)Instance).Play();
	}

	public new static void Stop()
	{
		((SingletonParticleEffect<BallBurningEffect>)Instance).Stop();
	}

	#endregion
}