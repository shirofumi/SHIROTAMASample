using UnityEngine;
using System;

public class BarrierControl : MonoBehaviour
{
	#region Fields

	[NonSerialized]
	public Barrier Barrier;

	private float startTime;

	private TimerProperty frozen;

	private TimerProperty coolingdown;

	private Vector2 lastVelocity;

	private new Rigidbody2D rigidbody;

	private Rigidbody2D core;

	private GameObject shape;

	private SpriteRenderer visual;

	private Collider2D zone;

	private new BarrierAnimation animation;

	#endregion

	#region Properties

	public bool IsFrozen
	{
		get { return frozen.State; }
	}

	public bool IsCoolingDown
	{
		get { return coolingdown.State; }
	}

	public Vector2 LastVelocity
	{
		get { return lastVelocity; }
	}

	private BarrierParams Params
	{
		get { return BarrierManager.Params; }
	}

	private BarrierSEs SEs
	{
		get { return BarrierManager.SEs; }
	}

	#endregion

	#region Messages

	private void OnEnable()
	{
		startTime = Time.time;
	}

	private void OnDisable()
	{
		startTime = Single.PositiveInfinity;
	}

	private void Awake()
	{
		this.rigidbody = GetComponent<Rigidbody2D>();
		this.core = transform.Find("Core").GetComponent<Rigidbody2D>();
		this.shape = transform.Find("Shape").gameObject;
		this.zone = transform.Find("Zone").GetComponent<Collider2D>();
		this.visual = GetComponentInChildren<SpriteRenderer>();
		this.animation = GetComponentInChildren<BarrierAnimation>();
	}

	private void Start()
	{
		visual.sprite = SpriteSelector.GetBarrier(Barrier.Type, Barrier.Scale);
	}

	private void Update()
	{
		if (!GamePhaseManager.ReadyOrRunning) return;

		if (CheckLifeTime()) return;

		if (frozen.Update(Params.FreezeTime)) animation.Frozen = false;
		if (coolingdown.Update(Params.CooldownTime)) Warmup();
	}

	private void FixedUpdate()
	{
		if (!GamePhaseManager.ReadyOrRunning) return;

		Vector2 velocity = core.velocity;
		if (velocity.sqrMagnitude > Params.MaxSpeed * Params.MaxSpeed)
		{
			velocity.Normalize();
			velocity *= Params.MaxSpeed;

			core.velocity = velocity;
		}

		float angularVelocity = rigidbody.angularVelocity;
		if (Mathf.Abs(angularVelocity) > Params.MaxAngularSpeed)
		{
			angularVelocity = Mathf.Sign(angularVelocity) * Params.MaxAngularSpeed;

			rigidbody.angularVelocity = angularVelocity;
		}

		lastVelocity = rigidbody.velocity;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject other = collision.gameObject;
		BallControl ball = (other.layer == Layers.Ball ? GameScene.Ball : null);

		if(ball != null) ExciteBall(ball);
		KickBack(collision, ball);
		Cooldown();
		animation.Hitting = true;
	}

	#endregion
	
	#region Methods

	public bool Push(Vector2 position, Vector2 velocity)
	{
		if (IsFrozen) return false;

		if (zone.OverlapPoint(position))
		{
			core.velocity += velocity * Params.MovementAdjustment;
			frozen.Start();

			animation.Frozen = true;

			GameSEGlobalSource.Play(SEs.Push);

			ActionCounter.PushBarrier();

			return true;
		}

		return false;
	}

	private bool CheckLifeTime()
	{
		float time = Time.time - startTime;
		if (time > Params.LifeTime)
		{
			Destroy(gameObject);

			return true;
		}

		float disappearing = Params.LifeTime - Params.BlinkingTime;
		if (time > disappearing)
		{
			float prev = time - Time.deltaTime - Mathf.Epsilon;
			if (prev < disappearing)
			{
				animation.Disappearing = true;
			}
		}

		return false;
	}

	private void Warmup()
	{
		shape.SetActive(true);

		animation.CoolingDown = false;
	}

	private void Cooldown()
	{
		shape.SetActive(false);

		animation.CoolingDown = true;

		coolingdown.Start();
	}

	private void ExciteBall(BallControl ball)
	{
		float point = IsFrozen ? Params.HittingWithFrozenExcitingPoint : Params.HittingExcitingPoint;
		ball.AddExcitingPoint(point);
	}

	private void KickBack(Collision2D collision, BallControl ball)
	{
		bool ooc = (ball != null && ball.IsOutOfControl);
		bool frozen = IsFrozen;

		Vector2 normal;

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

		float power = ooc ? Params.KickBackPowerWithOutOfControl : Params.KickBackPower;
		if (frozen) power *= Params.KickBackPowerMultiplierWithFrozen;

		core.velocity = normal * power;

		AudioClip clip = frozen ? SEs.Slug : SEs.Hit;
		GameSEGlobalSource.Play(clip);
	}

	#endregion
}
