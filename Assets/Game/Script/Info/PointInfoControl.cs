using UnityEngine;

public class PointInfoControl : SingletonMonoBehaviour<PointInfoControl>
{
	#region Fields

	public AudioClip Sound;

	private NumberTextSetter setter;

	#endregion

	#region Properties

	public static int Point
	{
		get { return Instance.setter.Number; }
	}

	#endregion

	#region Messages

	private new void Awake()
	{
		base.Awake();

		this.setter = GetComponentInChildren<NumberTextSetter>();
	}

	private void Start()
	{
		setter.Number = 0;
	}

	#endregion

	#region Methods

	public static void GetPoint(ItemType source)
	{
		Instance._GetPoint(source);
	}

	private void _GetPoint(ItemType source)
	{
		setter.Number += 1;

		ItemIconControl.StartAnimation();

		float volumeScale = GetVolumeScale(source);
		GameSEGlobalSource.Play(Sound, volumeScale);
	}

	private float GetVolumeScale(ItemType source)
	{
		int point;
		switch (source)
		{
			case ItemType.Small:
				point = GameConstants.SmallItemPoint;
				break;
			case ItemType.Medium:
				point = GameConstants.MediumItemPoint;
				break;
			case ItemType.Large:
				point = GameConstants.LargeItemPoint;
				break;
			default: return 0.0f;
		}

		float scale = (2.0f - Mathf.Pow(0.5f, point - 1)) / point;

		return scale;
	}

	#endregion
}
