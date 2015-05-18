using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform), typeof(LayoutElement))]
public class AlternativeLayout : MonoBehaviour
{
	#region Fields

	[SerializeField]
	private bool preserveSize;

	[SerializeField]
	private Vector2 sizeDelta;

	[SerializeField]
	private Vector2 anchoredPosition;

	[SerializeField]
	private Vector2 anchorMin;

	[SerializeField]
	private Vector2 anchorMax;

	#endregion

	#region Properties

	public bool PreserveSize
	{
		get { return preserveSize; }
		set { preserveSize = value; }
	}

	public Vector2 SizeDelta
	{
		get { return sizeDelta; }
		set { sizeDelta = value; }
	}

	public Vector2 AnchoredPosition
	{
		get { return anchoredPosition; }
		set { anchoredPosition = value; }
	}

	public Vector2 AnchorMin
	{
		get { return anchorMin; }
		set { anchorMin = value; }
	}

	public Vector2 AnchorMax
	{
		get { return anchorMax; }
		set { anchorMax = value; }
	}

	#endregion

	#region Messages

	private void OnEnable()
	{
		Apply();
	}

	private void OnDisable()
	{
		LayoutElement layout = GetComponent<LayoutElement>();

		layout.ignoreLayout = false;
	}

#if UNITY_EDITOR
	
	private void OnValidate()
	{
		RectTransform rect = (RectTransform)transform;

		if (!preserveSize) rect.sizeDelta = sizeDelta;
		rect.anchoredPosition = anchoredPosition;
		rect.anchorMin = anchorMin;
		rect.anchorMax = anchorMax;
	}

#endif

	#endregion

	#region Methods

	public void Apply()
	{
		RectTransform rect = (RectTransform)transform;
		LayoutElement layout = GetComponent<LayoutElement>();

		if (!preserveSize) rect.sizeDelta = sizeDelta;
		rect.anchoredPosition = anchoredPosition;
		rect.anchorMin = anchorMin;
		rect.anchorMax = anchorMax;

		layout.ignoreLayout = true;
	}

	#endregion
}
