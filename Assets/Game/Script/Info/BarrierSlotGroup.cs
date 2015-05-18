using UnityEngine;
using UnityEngine.UI;

public class BarrierSlotGroup : LayoutGroup
{
	#region Fields

	[SerializeField]
	private float m_Spacing = 0.0f;

	#endregion

	#region Properties

	public float Spacing
	{
		get { return m_Spacing; }
		set { SetProperty(ref m_Spacing, value); }
	}

	#endregion

	#region Methods

	public override void CalculateLayoutInputHorizontal()
	{
		base.CalculateLayoutInputHorizontal();

		float hpadding = padding.horizontal;

		SetLayoutInputForAxis(hpadding, hpadding, 1.0f, 0);
	}

	public override void CalculateLayoutInputVertical()
	{
		float vpadding = padding.vertical;

		float size = rectTransform.rect.width;
		float totalSize = (size + m_Spacing) * rectChildren.Count - m_Spacing + vpadding;
		
		SetLayoutInputForAxis(totalSize, totalSize, 0.0f, 1);
	}

	public override void SetLayoutHorizontal()
	{
		float pos = padding.left;
		float size = rectTransform.rect.width - padding.horizontal;

		for (int i = 0; i < rectChildren.Count; i++)
		{
			SetChildAlongAxis(rectChildren[i], 0, pos, size);
		}
	}

	public override void SetLayoutVertical()
	{
		float pos = padding.top;
		float size = rectTransform.rect.width - padding.horizontal;

		for (int i = 0; i < rectChildren.Count; i++)
		{
			SetChildAlongAxis(rectChildren[i], 1, pos, size);

			pos += size + m_Spacing;
		}
	}

	#endregion
}
