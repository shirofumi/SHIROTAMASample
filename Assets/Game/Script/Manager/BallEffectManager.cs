using UnityEngine;

public class BallEffectManager : EffectManager<BallEffectManager>
{
	#region Fields

	private Transform directional;

	private Transform nondirectional;

	private new Rigidbody2D rigidbody;

	#endregion

	#region Messages

	private new void Awake()
	{
		base.Awake();

		this.directional = transform.Find("Directional");
		this.nondirectional = transform.Find("Nondirectional");
	}

	private void Start()
	{
		this.rigidbody = GetComponentInParent<Rigidbody2D>();
	}

	private void Update()
	{
		Vector3 backward = -rigidbody.velocity;
		if (backward.magnitude > Vector3.kEpsilon)
		{
			Quaternion rotation = Quaternion.LookRotation(backward, Vector3.back);
			directional.localRotation = rotation;
		}
	}

	#endregion

	#region Methods

	public static GameObject AddDirectional(GameObject original)
	{
		return Instance._AddDirectional(original);
	}

	public static GameObject AddNondirectional(GameObject original)
	{
		return Instance._AddNondirectional(original);
	}

	private GameObject _AddDirectional(GameObject original)
	{
		GameObject instance = Instantiate(original);
		instance.transform.SetParent(directional, false);
		return instance;
	}

	private GameObject _AddNondirectional(GameObject original)
	{
		GameObject instance = Instantiate(original);
		instance.transform.SetParent(nondirectional, false);
		return instance;
	}

	#endregion
}