using System;
using UnityEngine;

[RequireComponent(typeof(FlyingBirdAnimation))]
public class FlyingBirdEffect : SingletonMonoBehaviour<FlyingBirdEffect>, IScreenEffect
{
	#region Fields

	public Vector2 Position;

	public float RotationAmount;

	public Vector3 AdditiveRotation;

	public Vector2 Scale;

	public FlyingBirdSEs SEs;

	private int direction;

	private Vector2 startPosition;

	private Quaternion startRotation;

	private Quaternion targetRotation;

	private Vector3 defaultScale;

	private Transform ball;

	private Transform bird;

	private new FlyingBirdAnimation animation;

	private new AudioSource audio;

	#endregion

	#region Properties

	public bool Success
	{
		get { return animation.Success; }
		set { animation.Success = value; }
	}

	public Transform Ball
	{
		get { return ball; }
	}

	public Transform Bird
	{
		get { return bird; }
	}

	#endregion

	#region Events

	public event Action End;

	#endregion

	#region Messages

	private new void Awake()
	{
		base.Awake();

		this.ball = transform.Find("Ball");
		this.bird = transform.Find("Bird");
		this.animation = GetComponent<FlyingBirdAnimation>();
		this.audio = GetComponent<AudioSource>();

		this.targetRotation = ball.localRotation;
		this.defaultScale = transform.localScale;
	}

	private void Update()
	{
		if (direction == default(int)) UpdateDirection();

		if (ball.gameObject.activeSelf)
		{
			Vector2 position = new Vector2(Position.x * direction, Position.y);
			Vector2 origin = ScreenEffectManager.ToGameSpacePosition(Vector2.zero);
			Vector2 delta = ScreenEffectManager.ToGameSpacePosition(position) - origin;

			Vector3 pos = transform.localPosition;
			pos.x = startPosition.x + delta.x;
			pos.y = startPosition.y + delta.y;
			transform.localPosition = pos;

			Vector3 scale = defaultScale;
			if (ScreenMonitor.Landscape)
			{
				scale.x *= Scale.x;
				scale.y *= Scale.y;
			}
			else
			{
				scale.x *= Scale.y;
				scale.y *= Scale.x;
			}
			transform.localScale = scale;

			Quaternion rotation = Quaternion.Slerp(startRotation, targetRotation, RotationAmount);
			Quaternion additive = Quaternion.Euler(AdditiveRotation);
			Quaternion screen = ScreenRotation.Get(ScreenOrientation.LandscapeLeft, ScreenMonitor.Orientation);
			ball.localRotation = screen * additive * rotation;
		}
		else if (bird.gameObject.activeSelf)
		{
			Vector2 position = new Vector2(Position.x * direction, Position.y);
			Vector2 basePosition = ScreenEffectManager.GetGameObjectPosition(startPosition);

			Vector3 pos = transform.localPosition;
			pos.x = basePosition.x + position.x;
			pos.y = basePosition.y + position.y;
			transform.localPosition = pos;

			transform.localScale = defaultScale;

			Vector3 scale = bird.localScale;
			scale.x = direction;
			bird.localScale = scale;
		}
	}

	private void PlaySE(int number)
	{
		AudioClip clip = null;
		switch (number)
		{
			case 0: clip = SEs.Jump; break;
			case 1: clip = SEs.Transform; break;
			case 2: clip = SEs.Appear; break;
			case 3: clip = SEs.Stumble; break;
			case 4: clip = SEs.Land; break;
			case 5: clip = SEs.Fly; break;
		}

		if (clip != null)
		{
			audio.clip = clip;
			audio.Play();
		}
	}

	private void Terminate()
	{
		if (End != null) End();

		Destroy(gameObject);
	}

	#endregion

	#region Methods

	public void Init(Vector2 position, Quaternion rotation)
	{
		Quaternion screen = ScreenRotation.Get(ScreenOrientation.LandscapeLeft, ScreenMonitor.Orientation);

		this.startPosition = position;
		this.startRotation = Quaternion.Inverse(screen) * rotation;
	}

	public static void StartAnimation()
	{
		Instance.animation.Start = true;
	}

	void IScreenEffect.OnLayoutChanged()
	{
		this.direction = default(int);
	}

	private void UpdateDirection()
	{
		Quaternion screen = ScreenRotation.Get(ScreenOrientation.LandscapeLeft, ScreenMonitor.Orientation);
		Vector2 pos = Quaternion.Inverse(screen) * startPosition;

		this.direction = (pos.x >= 0.0f ? -1 : 1);
	}

	#endregion
}