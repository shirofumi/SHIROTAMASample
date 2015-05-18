using System;
using UnityEngine;
using UnityEngine.UI;

public class MissionBoardControl : MonoBehaviour
{
	#region Fields

	public Sprite Success;

	public Sprite Failure;

	private Image[] images;

	#endregion

	#region Messages

	private void Awake()
	{
		images = new Image[transform.childCount];

		int count = 0;
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (child.gameObject.name == "MissionCheck")
			{
				Image image = child.GetComponent<Image>();
				if (image != null) images[count++] = image;
			}
		}

		if (images.Length != count)
		{
			Image[] array = new Image[count];
			Array.Copy(images, 0, array, 0, count);
			images = array;
		}
	}

	private void OnEnable()
	{
		for (int i = 0; i < images.Length; i++)
		{
			images[i].sprite = MissionChecker.IsAccomplished(i) ? Success : Failure;
		}
	}

	#endregion
}