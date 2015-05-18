using UnityEngine;
using System;

public class BallControl : MonoBehaviour
{
	#region Fields

	public BallParams Params;

	public BallSEs SEs;

	private float minSpeed;

	private float maxSpeed;

	private float excitedLevel;

	private Vector3 lastPosition;

	private Vector2 lastVelocity;

	private float lastPointChangedTime;

	private float defaultMass;

	private new Rigidbody2D rigidbody;

	private Transform visual;

	private CircleCollider2D shape;

	private CircleCollider2D contact;

	private new BallAnimation animation;

	private new AudioSource audio;

	#endregion

	#region Properties

	public float MinSpeed
	{
		get { return minSpeed; }
	}

	public float MaxSpeed
	{
		get { return maxSpeed; }
	}

	public float ExcitedLevel
	{
		get { return excitedLevel; }
	}

	public bool IsOutOfControl
	{
		get { return (excitedLevel == GameConstants.MaxExcitedLevel); }
	}

	public Vector2 LastVelocity
	{
		get { return lastVelocity; }
	}

	public Rigidbody2D Rigidbody
	{
		get { return rigidbody; }
	}

	public CircleCollider2D Shape
	{
		get { return shape; }
	}

	public CircleCollider2D Contact
	{
		get { return contact; }
	}

	#endregion

	#region Messages

	private void Awake()
	{
		this.rigidbody = GetComponent<Rigidbody2D>();
		this.visual = transform.Find("Visual");
		this.shape = transform.Find("Shape").GetComponent<CircleCollider2D>();
		this.contact = transform.Find("Contact").GetComponent<CircleCollider2D>();
		this.animation = GetComponentInChildren<BallAnimation>();
		this.audio = GetComponent<AudioSource>();
	}

	private void Start()
	{
		this.defaultMass = rigidbody.mass;

		Vector2 start = GameScene.Layer.StartPosition;
		transform.localPosition = CellManager.CellToWorld(start, transform.localPosition.z);

		minSpeed = 0.0f;
		maxSpeed = Params.BaseMaxSpeed;
		excitedLevel = 0.0f;
		lastPosition = transform.localPosition;
		lastPointChangedTime = Time.time;
	}

	private void Update()
	{
		if (!GamePhaseManager.Running) return;

		UpdateExcitedLevel();
		UpdateBehaviour();
		UpdateVisual();

		Rotate();
	}

	private void FixedUpdate()
	{
		if (!GamePhaseManager.Running) return;

		Vector2 velocity = rigidbody.velocity;
		float sqrMagnitude = velocity.sqrMagnitude;
		if (sqrMagnitude > maxSpeed * maxSpeed)
		{
			velocity.Normalize();
			velocity *= maxSpeed;

			rigidbody.velocity = velocity;
		}
		else if (sqrMagnitude < minSpeed * minSpeed)
		{
			velocity.Normalize();
			velocity *= minSpeed;

			rigidbody.velocity = velocity;
		}

		lastVelocity = rigidbody.velocity;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject gameObject = collision.gameObject;
		if (gameObject.layer == Layers.Barrier)
		{
			if (GamePhaseManager.Ready)
			{
				GamePhaseManager.Next(GamePhase.Running);
			}
			
			Hit(collision);
		}
	}

	#endregion

	#region Methods

	public bool AddExcitingPoint(float point)
	{
		return AddExcitingPoint(point, false);
	}

	public bool AddExcitingPoint(float point, bool force)
	{
		return SetExcitingPoint(excitedLevel + point, force);
	}

	public bool SetExcitingPoint(float point)
	{
		return SetExcitingPoint(point, false);
	}

	public bool SetExcitingPoint(float point, bool force)
	{
		if (force || !IsOutOfControl)
		{
			float newValue = Mathf.Clamp(point, 0.0f, GameConstants.MaxExcitedLevel);
			if (force || excitedLevel != newValue)
			{
				excitedLevel = newValue;

				lastPointChangedTime = Time.time;

				if (IsOutOfControl)
				{
					audio.PlayOneShot(SEs.GetOoc);

					ActionCounter.GetOutOfControl();
				}

				return true;
			}
		}

		return false;
	}

	public void Fall()
	{
		GamePhaseManager.Next(GamePhase.Missed);

		FallingBallEffect effect = NewFallingBallEffect();
		effect.End += OnFallingBallEffectEnd;

		gameObject.SetActive(false);
	}

	public void FlyAway()
	{
		GamePhaseManager.Next(GamePhase.Completed);

		FlyingBirdEffect effect = NewFlyingBirdEffect();
		effect.End += OnFlyingBirdEffectEnd;

		gameObject.SetActive(false);
	}

	private void UpdateExcitedLevel()
	{
		if (IsOutOfControl)
		{
			if (Time.time - lastPointChangedTime > Params.RecoveryTime)
			{
				SetExcitingPoint(Params.ExcitedLevelAfterRecovery, true);

				audio.PlayOneShot(SEs.FinishOoc);
			}
		}
		else
		{
			if (Time.time - lastPointChangedTime > Params.AutoHealingInterval)
			{
				AddExcitingPoint(-Params.AutoHealingPoint, true);
			}
		}
	}

	private void UpdateBehaviour()
	{
		float threshold = GameConstants.MaxExcitedLevel * Params.ExcitedLevelThreshold;
		if (excitedLevel < threshold)
		{
			minSpeed = 0.0f;
			maxSpeed = Params.BaseMaxSpeed;
		}
		else if (IsOutOfControl)
		{
			minSpeed = maxSpeed = Params.SpeedWithOutOfControl;
		}
		else
		{
			float s = Mathf.Clamp01((excitedLevel - threshold) / (GameConstants.MaxExcitedLevel - threshold));
			float t = 1.0f - Mathf.Cos(s * (Mathf.PI / 2.0f));
			minSpeed = Params.SpeedWithOutOfControl * t;
			maxSpeed = Params.BaseMaxSpeed * (1.0f - t) + minSpeed;
		}

		rigidbody.mass = (IsOutOfControl ? Params.MassWithOutOfControl : defaultMass);
	}

	private void UpdateVisual()
	{
		if (IsOutOfControl)
		{
			animation.OutOfControl = true;

			BallBurningEffect.Play();
		}
		else
		{
			animation.OutOfControl = false;
			animation.ExcitedLevel = excitedLevel;

			BallBurningEffect.Stop();
		}
	}

	private void Hit(Collision2D collision)
	{
		BarrierControl barrier = collision.gameObject.GetComponentInParent<BarrierControl>();
		bool frozen = barrier.IsFrozen;
		bool ooc = IsOutOfControl;

		if (!ooc)
		{
			Vector2 normal, perp;
			ContactPoint2D[] contacts = collision.contacts;
			if (contacts.Length == 1)
			{
				normal = contacts[0].normal;
			}
			else
			{
				normal = Vector2.zero;
				foreach (ContactPoint2D contact in contacts)
				{
					normal += contact.normal;
				}
				normal.Normalize();
			}
			perp = new Vector2(-normal.y, normal.x);

			Vector2 v1 = LastVelocity;
			Vector2 v2 = barrier.LastVelocity;
			Vector2 vsum = v1 + v2;

			float pn = Math.Max(0.0f, Vector2.Dot(vsum, normal) * Params.ImpactAdjustment - Params.ImpactAbsorption);
			float vp = Vector2.Dot(vsum, perp) * Params.ImpactAdjustment;
			float pp = Mathf.Max(0.0f, Mathf.Abs(vp) - Params.ImpactFriction) * Mathf.Sign(vp);
			float ex = (frozen ? Params.HitPowerWithBarrierFrozen : Params.HitPower);

			rigidbody.velocity = normal * (pn + ex) + perp * pp;

			animation.Hit = true;
		}

		ActionCounter.HitBall(frozen, ooc);
	}

	private void Rotate()
	{
		Vector3 pos = transform.position;

		Vector2 delta = pos - lastPosition;
		float distance = delta.magnitude;
		if (distance > Vector2.kEpsilon)
		{
			float scale = shape.radius;
			float angle = distance / scale * Mathf.Rad2Deg;
			Vector2 direction = delta / distance;
			Vector3 axis = new Vector3(direction.y, -direction.x, 0.0f);
			Quaternion rotation = Quaternion.AngleAxis(angle, axis);

			visual.localRotation = rotation * visual.localRotation;
		}


		lastPosition = pos;
	}

	private FallingBallEffect NewFallingBallEffect()
	{
		GameObject instance = FieldEffectManager.Add(EffectStore.Instance.FallingBallEffect);
		instance.transform.localPosition = visual.transform.position;
		instance.transform.localRotation = visual.transform.localRotation;

		FallingBallEffect effect = instance.GetComponent<FallingBallEffect>();
		float distance = rigidbody.velocity.magnitude;
		Vector2 direction = (distance > Vector2.kEpsilon ? rigidbody.velocity / distance : Vector2.zero);
		effect.AngularVelocity = distance / shape.radius * Mathf.Rad2Deg;
		effect.Axis = new Vector3(direction.y, -direction.x, 0.0f);

		return effect;
	}

	private FlyingBirdEffect NewFlyingBirdEffect()
	{
		GameObject instance = ScreenEffectManager.Add(EffectStore.Instance.FlyingBirdEffect);

		FlyingBirdEffect effect = instance.GetComponent<FlyingBirdEffect>();
		Vector2 position = visual.transform.position;
		Quaternion rotation = visual.transform.localRotation;
		effect.Init(position, rotation);
		effect.Success = (GameScene.Depth == GameScene.Map.Layers.Length - 1);

		return effect;
	}

	private void OnFallingBallEffectEnd()
	{
		GamePhaseManager.Next(GamePhase.Failed);
	}

	private void OnFlyingBirdEffectEnd()
	{
		int depth = GameScene.Depth;
		int maxDepth = GameScene.Map.Layers.Length;

		if (depth + 1 < maxDepth)
		{
			GamePhaseManager.Next(GamePhase.Ending);
		}
		else
		{
			GamePhaseManager.Next(GamePhase.Result);
		}	
	}

	#endregion
}
