using System;
using UnityEngine;
using UnityEngine.UI;

public class NumberTextSetter : MonoBehaviour
{
	#region Constants

	private const int UnsetValue = Int32.MinValue;

	#endregion

	#region Fields

	public bool PaddingByZero;

	private Text[] targets;

	private int number = UnsetValue;

	private bool dirty;

	#endregion

	#region Properties

	public int Number
	{
		get { return this.number; }
		set
		{
			if (this.number != value)
			{
				this.number = value;

				dirty = true;
			}
		}
	}

	#endregion

	#region Messages

	private void Start()
	{
		targets = GetComponentsInChildren<Text>();

		int count = 0;
		for (int i = 0; i < targets.Length; i++)
		{
			if (targets[i].text == "#")
			{
				targets[count++] = targets[i];
			}
		}

		if (targets.Length != count)
		{
			Text[] array = new Text[count];
			Array.Copy(targets, 0, array, 0, count);
			targets = array;
		}

		if (number == UnsetValue) Number = 0;

		UpdateText();
	}

	private void LateUpdate()
	{
		if (dirty) UpdateText();
	}

	#endregion

	#region Methods

	private void UpdateText()
	{
		if(targets == null) return;

		int n = number;
		for (int i = targets.Length - 1; i >= 0; i--)
		{
			Text target = targets[i];

			if (n == 0)
			{
				if (PaddingByZero || i == targets.Length - 1)
				{
					target.text = NumberString.Get(0);
					target.gameObject.SetActive(true);
				}
				else
				{
					target.gameObject.SetActive(false);
				}
			}
			else
			{
				target.text = NumberString.Get(n % 10);
				target.gameObject.SetActive(true);

				n /= 10;
			}
		}

		dirty = false;
	}

	#endregion

}
