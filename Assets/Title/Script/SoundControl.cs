using UnityEngine;
using UnityEngine.UI;

namespace Title
{
	public class SoundControl : MonoBehaviour
	{
		#region Fields

		public string On;

		public string Off;

		private Text textOnOff;

		#endregion

		#region Messages

		private void Awake()
		{
			this.textOnOff = transform.Find("OnOff").GetComponent<Text>();
		}

		private void Start()
		{
			if (AudioManager.Mute)
			{
				textOnOff.text = Off;
			}
			else
			{
				textOnOff.text = On;
			}
		}

		#endregion

		#region Methods

		public void Toggle()
		{
			if (AudioManager.Mute)
			{
				AudioManager.Mute = false;

				textOnOff.text = On;
			}
			else
			{
				AudioManager.Mute = true;

				textOnOff.text = Off;
			}
		}

		#endregion
	}
}
