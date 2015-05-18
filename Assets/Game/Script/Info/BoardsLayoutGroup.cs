using System;
using UnityEngine;
using UnityEngine.UI;

public class BoardsLayoutGroup : LayoutGroup
{
	#region Constants

	private const string FieldBoardName = "FieldInfoBoard";

	private const string PointBoardName = "PointBoard";

	private const string MiscBoardName = "MiscInfoBoard";

	private const string ButtonName = "PauseButton";

	#endregion

	#region Fields

	[SerializeField]
	private Vector2 m_Spacing;

	private RectTransform field;

	private RectTransform point;

	private RectTransform misc;

	private RectTransform button;

	#endregion

	#region Properties

	public Vector2 Spacing
	{
		get { return m_Spacing; }
		set { SetProperty(ref m_Spacing, value); }
	}

	#endregion

	#region Messages

	protected override void OnEnable()
	{
		base.OnEnable();

		ScreenMonitor.ScreenStateChanged += OnScreenStateChanged;
	}

	protected override void OnDisable()
	{
		if (ScreenMonitor.IsAlive) ScreenMonitor.ScreenStateChanged -= OnScreenStateChanged;

		base.OnDisable();
	}

	#endregion

	#region Methods

	public override void CalculateLayoutInputHorizontal()
	{
		base.CalculateLayoutInputHorizontal();

		CacheObjects();

		SetLayoutInputForAxis(0.0f, 0.0f, 1.0f, 0);
	}

	public override void CalculateLayoutInputVertical()
	{
		SetLayoutInputForAxis(0.0f, 0.0f, 1.0f, 1);
	}

	public override void SetLayoutHorizontal()
	{
		FieldInfoLayoutGroup layout = field.GetComponent<FieldInfoLayoutGroup>();
		if (layout != null)
		{
			layout.Flip = (ScreenMonitor.Orientation == ScreenOrientation.LandscapeRight);
		}

		m_Tracker.Add(this, field, DrivenTransformProperties.Pivot | DrivenTransformProperties.Rotation);
		if (ScreenMonitor.Landscape)
		{
			field.pivot = new Vector2(0.5f, 0.5f);
			field.localRotation = Quaternion.identity;

			SetLayoutLandscapeHorizontal();
		}
		else
		{
			field.pivot = Vector2.up;
			field.localRotation = Quaternion.AngleAxis(-90.0f, Vector3.forward);

			SetLayoutPortraitHorizontal(layout);
		}
	}

	public override void SetLayoutVertical()
	{
		if (ScreenMonitor.Landscape)
		{
			SetLayoutLandscapeVertical();
		}
		else
		{
			SetLayoutPortraitVertical();
		}

		ClearObjects();
	}

	private void SetLayoutLandscapeHorizontal()
	{
		float width = rectTransform.rect.width - padding.horizontal;
		float padl = padding.left;

		SetChildAlongAxis(field, 0, padl, width);
		SetChildAlongAxis(point, 0, padl, width);
		SetChildAlongAxis(misc, 0, padl, width);

		float min = LayoutUtility.GetMinWidth(button);
		float preferred = LayoutUtility.GetPreferredWidth(button);
		float buttonSize = Mathf.Max(Mathf.Min(preferred, width), min);
		if (ScreenMonitor.Orientation == ScreenOrientation.LandscapeLeft)
		{
			SetChildAlongAxis(button, 0, padl, buttonSize);
		}
		else
		{
			SetChildAlongAxis(button, 0, padl + width - buttonSize, buttonSize);
		}
	}

	private void SetLayoutPortraitHorizontal(FieldInfoLayoutGroup layout)
	{
		Vector2 size = rectTransform.rect.size;
		float width = size.x - padding.vertical;
		float height = size.y - padding.horizontal;
		float padl = padding.bottom;

		float min, preferred, buttonSize;
		min = LayoutUtility.GetMinWidth(button);
		preferred = LayoutUtility.GetPreferredWidth(button);
		buttonSize = Mathf.Max(Mathf.Min(preferred, width), min);

		min = Mathf.Max(LayoutUtility.GetMinWidth(point), LayoutUtility.GetMinWidth(misc));
		preferred = Mathf.Max(LayoutUtility.GetPreferredWidth(point), LayoutUtility.GetPreferredWidth(misc));

		float fieldSize = (layout != null ? layout.GetPreferredHeight(height) : 0.0f);
		float rest = width - (buttonSize + fieldSize + m_Spacing.y * 2.0f);
		float w = Mathf.Max(Mathf.Min(preferred, rest), min);

		float right = padl + width;
		float left = padl + buttonSize + m_Spacing.y + (rest - w) * 0.5f;

		SetChildAlongAxis(field, 0, right, height);
		SetChildAlongAxis(point, 0, left, w);
		SetChildAlongAxis(misc, 0, left, w);

		SetChildAlongAxis(button, 0, padl, buttonSize);
	}

	private void SetLayoutLandscapeVertical()
	{
		float height = rectTransform.rect.height - padding.vertical;
		float padt = padding.top;
		float offset = padt;
		float length;

		length = LayoutUtility.GetPreferredHeight(field);
		SetChildAlongAxis(field, 1, offset, length);
		offset += length + m_Spacing.y;

		length = LayoutUtility.GetPreferredHeight(point);
		SetChildAlongAxis(point, 1, offset, length);
		offset += length + m_Spacing.y;

		length = LayoutUtility.GetPreferredHeight(misc);
		SetChildAlongAxis(misc, 1, offset, length);

		float min = LayoutUtility.GetMinHeight(button);
		float preferred = LayoutUtility.GetPreferredHeight(button);
		float buttonSize = Mathf.Max(Mathf.Min(preferred, height), min);
		SetChildAlongAxis(button, 1, padt, buttonSize);
	}

	private void SetLayoutPortraitVertical()
	{
		float height = rectTransform.rect.height - padding.horizontal;
		float padt = padding.left;

		SetChildAlongAxis(field, 1, padt, LayoutUtility.GetPreferredHeight(field));

		float pointMin = LayoutUtility.GetMinHeight(point);
		float pointPreferred = LayoutUtility.GetPreferredHeight(point);
		float miscMin = LayoutUtility.GetMinHeight(misc);
		float miscPreferred = LayoutUtility.GetPreferredHeight(misc);
		float min = pointMin + miscMin;
		float preferred = pointPreferred + miscPreferred;

		float inner = height - m_Spacing.x;
		float t = Mathf.Clamp01((inner - min) / (preferred - min));
		float length = Mathf.Lerp(min, preferred, t);
		float offset = padt + (inner - length) * 0.5f;

		length = Mathf.Lerp(pointMin, pointPreferred, t);
		SetChildAlongAxis(point, 1, offset, length);
		offset += length + m_Spacing.x;

		length = Mathf.Lerp(miscMin, miscPreferred, t);
		SetChildAlongAxis(misc, 1, offset, length);

		float buttonSize;
		min = LayoutUtility.GetMinHeight(button);
		preferred = LayoutUtility.GetPreferredHeight(button);
		buttonSize = Mathf.Max(Mathf.Min(preferred, height), min);
		SetChildAlongAxis(button, 1, padt, buttonSize);
	}

	private void CacheObjects()
	{
		for (int i = 0; i < rectChildren.Count; i++)
		{
			RectTransform rect = rectChildren[i];
			switch (rect.gameObject.name)
			{
				case FieldBoardName:
					field = rect;
					break;
				case PointBoardName:
					point = rect;
					break;
				case MiscBoardName:
					misc = rect;
					break;
				case ButtonName:
					button = rect;
					break;
				default:
					throw new InvalidOperationException("Unknown object.");
			}
		}
	}

	private void ClearObjects()
	{
		field = point = misc = button = null;
	}

	private void OnScreenStateChanged(ScreenStateChangedEventData data)
	{
		if (!data.ResolutionChanged && data.OrientationChanged)
		{
			SetDirty();
		}
	}

	#endregion
}