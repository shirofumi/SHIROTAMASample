using UnityEngine;
using UnityEngine.UI;

namespace LevelSelection
{
	[RequireComponent(typeof(AlternativeLayout))]
	public class TotalPointControl : MonoBehaviour
	{
		#region Fields

		private AlternativeLayout altLayout;

		private ContentSizeFitter fitter;

		#endregion

		#region Messages

		private void Awake()
		{
			this.altLayout = GetComponent<AlternativeLayout>();
			this.fitter = GetComponent<ContentSizeFitter>();
		}

		private void OnEnable()
		{
			ScreenMonitor.ScreenStateChanged += OnScreenStateChanged;
		}

		private void OnDisable()
		{
			if (ScreenMonitor.IsAlive) ScreenMonitor.ScreenStateChanged -= OnScreenStateChanged;
		}

		private void Start()
		{
			NumberTextSetter setter = GetComponentInChildren<NumberTextSetter>();
			setter.Number = PointCalculator.TotalPoints;

			UpdateLayout();
		}

		#endregion

		#region Methods

		private void OnScreenStateChanged(ScreenStateChangedEventData data)
		{
			if (data.OrientationTypeChanged)
			{
				UpdateLayout();
			}
		}

		private void UpdateLayout()
		{
			if (ScreenMonitor.Landscape)
			{
				altLayout.enabled = true;
				fitter.enabled = true;
			}
			else
			{
				altLayout.enabled = false;
				fitter.enabled = false;
			}
		}

		#endregion
	}
}
