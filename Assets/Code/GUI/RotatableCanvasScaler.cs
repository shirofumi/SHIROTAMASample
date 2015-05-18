using UnityEngine;
using UnityEngine.UI;

public class RotatableCanvasScaler : CanvasScaler
{
	#region Constants

	private const float kLogBase = 2;

	#endregion

	#region Fields

	[SerializeField]
	private bool m_PortraitAsDefault;

	#endregion

	#region Properties

	public bool PortraitAsDefault
	{
		get { return m_PortraitAsDefault; }
		set { m_PortraitAsDefault = value; }
	}

	#endregion

	#region Methods

	protected override void HandleScaleWithScreenSize()
	{
		Vector2 screenSize;
		if (ScreenOrientationIsLandscape() ^ m_PortraitAsDefault)
		{
			screenSize = new Vector2(Screen.width, Screen.height);
		}
		else
		{
			screenSize = new Vector2(Screen.height, Screen.width);
		}

		float scaleFactor = 0;
		switch (m_ScreenMatchMode)
		{
			case ScreenMatchMode.MatchWidthOrHeight:
				{
					float logWidth = Mathf.Log(screenSize.x / m_ReferenceResolution.x, kLogBase);
					float logHeight = Mathf.Log(screenSize.y / m_ReferenceResolution.y, kLogBase);
					float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, m_MatchWidthOrHeight);
					scaleFactor = Mathf.Pow(kLogBase, logWeightedAverage);
					break;
				}
			case ScreenMatchMode.Expand:
				{
					scaleFactor = Mathf.Min(screenSize.x / m_ReferenceResolution.x, screenSize.y / m_ReferenceResolution.y);
					break;
				}
			case ScreenMatchMode.Shrink:
				{
					scaleFactor = Mathf.Max(screenSize.x / m_ReferenceResolution.x, screenSize.y / m_ReferenceResolution.y);
					break;
				}
		}

		SetScaleFactor(scaleFactor);
		SetReferencePixelsPerUnit(m_ReferencePixelsPerUnit);
	}

	private bool ScreenOrientationIsLandscape()
	{
#if UNITY_EDITOR
		if (!UnityEditor.EditorApplication.isPlaying)
		{
			return true;
		}
		else
		{
			return (Screen.width >= Screen.height);
		}
#else
		return (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight);
#endif
	}

	#endregion
}