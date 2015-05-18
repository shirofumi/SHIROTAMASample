using System;
using UnityEngine;
using UnityEngine.UI;

public class FieldInfoLayoutGroup : LayoutGroup
{
	#region Constants

	private const int BarrierSize = 5;

	private const string SlotName = "BarrierSlot";

	private const string ButtonName = "NextBarrierButton";

	private const string GaugeName = "ExcitedLevelGauge";

	private const float sin0 = 0.6427876097f;
	private const float cos0 = 0.7660444431f;

	private const float sin1 = 0.9848077530f;
	private const float cos1 = 0.1736481777f;

	#endregion

	#region Fields

	[SerializeField]
	private float m_BarrierRadius;

	[SerializeField]
	private float m_GaugeRadius;

	[SerializeField]
	private float m_NextBarrierButtonSize;

	[SerializeField]
	private float m_Spacing;

	[SerializeField]
	private bool m_Flip;

	private RectTransform[] slots = new RectTransform[BarrierSize];

	private RectTransform button;

	private RectTransform gauge;

	#endregion

	#region Properties

	public float BarrierRadius
	{
		get { return m_BarrierRadius; }
		set { SetProperty(ref m_BarrierRadius, value); }
	}

	public float GaugeRadius
	{
		get { return m_GaugeRadius; }
		set { SetProperty(ref m_GaugeRadius, value); }
	}

	public float NextBarrierButtonSize
	{
		get { return m_NextBarrierButtonSize; }
		set { m_NextBarrierButtonSize = value; }
	}

	public float Spacing
	{
		get { return m_Spacing; }
		set { SetProperty(ref m_Spacing, value); }
	}

	public bool Flip
	{
		get { return m_Flip; }
		set { SetProperty(ref m_Flip, value); }
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
		float width = rectTransform.rect.width;
		float padh = m_Padding.horizontal;
		float padv = m_Padding.vertical;

		float size = GetPreferredHeight(width - padh);

		SetLayoutInputForAxis(0.0f, padv + size, 0.0f, 1);
	}

	public override void SetLayoutHorizontal()
	{
		float width = rectTransform.rect.width;
		float padh = m_Padding.horizontal;

		width -= padh;

		float sp = m_Spacing;
		float r1 = width * m_BarrierRadius;
		float r2 = GetSubBarrierRadius(width, r1, sp);
		float r3 = width * m_GaugeRadius;
		float d1 = r1 * 2.0f;
		float d2 = r2 * 2.0f;
		float d3 = r3 * 2.0f;
		float k1 = cos1 * (r2 * 2.0f + sp);
		float k2 = cos0 * (r2 * 2.0f + sp);

		SetChildLayoutHorizontal(slots[0], width, width - d1, d1);
		SetChildLayoutHorizontal(slots[1], width, k1, d2);
		SetChildLayoutHorizontal(slots[2], width, 0.0f, d2);
		SetChildLayoutHorizontal(slots[3], width, k1, d2);
		SetChildLayoutHorizontal(slots[4], width, k1 + k2, d2);

		SetChildLayoutHorizontal(gauge, width, width - r1 - r3, d3);

		float buttonSize = Mathf.Min(m_NextBarrierButtonSize, width);
		SetChildLayoutHorizontal(button, width, 0.0f, buttonSize);
	}

	public override void SetLayoutVertical()
	{
		Vector2 size = rectTransform.rect.size;
		float padh = m_Padding.horizontal;
		float padv = m_Padding.vertical;

		size.x -= padh;
		size.y -= padv;

		float sp = m_Spacing;
		float r1 = size.x * m_BarrierRadius;
		float r2 = GetSubBarrierRadius(size.x, r1, sp);
		float r3 = size.x * m_GaugeRadius;
		float d1 = r1 * 2.0f;
		float d2 = r2 * 2.0f;
		float d3 = r3 * 2.0f;
		float l1 = sin0 * (r1 + r2 + sp);
		float l2 = sin1 * (r2 * 2.0f + sp);
		float l3 = sin0 * (r2 * 2.0f + sp);

		float offset = 0.0f;
		SetChildLayoutVertical(slots[0], offset, d1);
		SetChildLayoutVertical(slots[1], offset += r1 + l1 - r2, d2);
		SetChildLayoutVertical(slots[2], offset += l2, d2);
		SetChildLayoutVertical(gauge, offset + r2 - r3, d3);
		SetChildLayoutVertical(slots[3], offset += l2, d2);
		SetChildLayoutVertical(slots[4], offset += l3, d2);

		bool landscape = ScreenMonitor.Landscape;
		float buttonSize = m_NextBarrierButtonSize;
		float buttonPos = landscape ? (size.y - buttonSize) : 0.0f;
		SetChildLayoutVertical(button, buttonPos, buttonSize);

		ClearObjects();
	}

	public float GetPreferredHeight(float width)
	{
		float sp = m_Spacing;
		float r1 = width * m_BarrierRadius;
		float r2 = GetSubBarrierRadius(width, r1, sp);
		float halfButtonSize = Mathf.Min(m_NextBarrierButtonSize, width) * 0.5f;

		float size = r1 * (1.0f + sin0) + r2 * (1.0f + sin0 * 3.0f + sin1 * 4.0f) + sp * (sin0 * 2.0f + sin1 * 2.0f) + halfButtonSize;

		return size;
	}

	private static float GetSubBarrierRadius(float width, float r, float sp)
	{
		return (width - r * (1.0f + cos0) - sp * (cos0 + cos1)) / (1.0f + cos0 + cos1 * 2.0f);
	}

	private void SetChildLayoutHorizontal(RectTransform rect, float width, float pos, float size)
	{
		if (rect == null) return;

		float padl = m_Padding.left;
		if (m_Flip)
		{
			SetChildAlongAxis(rect, 0, padl + width - (pos + size), size);
		}
		else
		{
			SetChildAlongAxis(rect, 0, padl + pos, size);
		}
	}

	private void SetChildLayoutVertical(RectTransform rect, float pos, float size)
	{
		if (rect == null) return;

		float padt = m_Padding.top;

		SetChildAlongAxis(rect, 1, padt + pos, size);
	}

	private void CacheObjects()
	{
		int index = 0;
		for (int i = 0; i < rectChildren.Count; i++)
		{
			RectTransform rect = rectChildren[i];
			switch (rect.gameObject.name)
			{
				case SlotName:
					slots[index++] = rect;
					break;
				case ButtonName:
					button = rect;
					break;
				case GaugeName:
					gauge = rect;
					break;
				default:
					throw new InvalidOperationException("Unknown object.");
			}
		}
	}

	private void ClearObjects()
	{
		Array.Clear(slots, 0, slots.Length);
		button = gauge = null;
	}

	#endregion
}