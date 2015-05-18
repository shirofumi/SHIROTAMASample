using System;
using UnityEngine;
using UnityEngine.UI;

namespace MapSelection
{
	public class MissionLayoutGroup : LayoutGroup
	{
		#region Constants

		private const string CheckName = "Check";

		private const string DescriptionName = "Description";

		#endregion

		#region Fields

		[SerializeField]
		private float m_Spacing;

		private bool rebuild;

		private Vector2 slot;

		private RectTransform check;

		private RectTransform description;

		#endregion

		#region Properties

		public float Spacing
		{
			get { return m_Spacing; }
			set { SetProperty(ref m_Spacing, value); }
		}

		#endregion

		#region Messages

		private void Update()
		{
			if (rebuild)
			{
				LayoutRebuilder.MarkLayoutForRebuild(rectTransform);

				rebuild = false;
			}
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
			float padv = padding.vertical;

			float min = LayoutUtility.GetMinHeight(check) + padv;
			float preferred = LayoutUtility.GetPreferredHeight(check) + padv;

			SetLayoutInputForAxis(min, preferred, 0.0f, 1);
		}

		public override void SetLayoutHorizontal()
		{
			if (slot.x == rectTransform.rect.width)
			{
				float width = slot.x - padding.horizontal;
				float height = slot.y - padding.vertical;
				float padl = padding.left;


				float checkSize = GetWidth(check, height);
				float descriptionSize = width - (height + m_Spacing);
				float offset;

				offset = padl + (height - checkSize) * 0.5f;
				SetChildAlongAxis(check, 0, offset, checkSize);

				offset = padl + height + m_Spacing;
				SetChildAlongAxis(description, 0, offset, descriptionSize);
			}
			else
			{
				SetChildAlongAxis(check, 0, 0.0f, 0.0f);
				SetChildAlongAxis(description, 0, 0.0f, 0.0f);
			}
		}

		public override void SetLayoutVertical()
		{
			Vector2 size = rectTransform.rect.size;
			if (slot == size)
			{
				float height = slot.y - padding.vertical;
				float padt = padding.top;

				float checkSize = GetHeight(check, height);
				float descriptionSize = height;
				float offset;

				offset = padt + (height - checkSize) * 0.5f;
				SetChildAlongAxis(check, 1, offset, checkSize);

				offset = padt + (height - descriptionSize) * 0.5f;
				SetChildAlongAxis(description, 1, offset, descriptionSize);

				ClearObjects();
			}
			else
			{
				SetChildAlongAxis(check, 1, 0.0f, 0.0f);
				SetChildAlongAxis(description, 1, 0.0f, 0.0f);

				slot = size;
				rebuild = true;
			}
		}

		private float GetWidth(RectTransform rect, float max)
		{
			float min = LayoutUtility.GetMinWidth(rect);
			float preferred = LayoutUtility.GetPreferredWidth(rect);
			return Mathf.Max(Mathf.Min(max, preferred), min);
		}

		private float GetHeight(RectTransform rect, float max)
		{
			float min = LayoutUtility.GetMinHeight(rect);
			float preferred = LayoutUtility.GetPreferredHeight(rect);
			return Mathf.Max(Mathf.Min(max, preferred), min);
		}

		private void CacheObjects()
		{
			if (check != null && description != null) return;

			for (int i = 0; i < rectChildren.Count; i++)
			{
				RectTransform rect = rectChildren[i];
				switch (rect.gameObject.name)
				{
					case CheckName:
						check = rect;
						break;
					case DescriptionName:
						description = rect;
						break;
					default:
						throw new InvalidOperationException("Unknown object.");
				}
			}
		}

		private void ClearObjects()
		{
			check = description = null;
		}

		#endregion
	}
}
