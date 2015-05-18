using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenFadeManager : SingletonMonoBehaviour<ScreenFadeManager>
{
	#region Fields

	public float Duration;

	private bool visible;

	private float startTime;

	private Action action;

	private CanvasGroup group;

	#endregion

	#region Properties

	public static bool Visible
	{
		get { return Instance.visible; }
	}

	public static float Amount
	{
		get { return (1.0f - Instance.group.alpha); }
	}

	#endregion

	#region Messages

	private new void Awake()
	{
		base.Awake();

		this.group = GetComponent<CanvasGroup>();

		group.alpha = 1.0f;

		enabled = false;
	}

	private void Update()
	{
		float elapsed = Time.unscaledTime - startTime;
		if (elapsed < Duration)
		{
			if (visible)
			{
				group.alpha = Mathf.Clamp01(1.0f - elapsed / Duration);
			}
			else
			{
				group.alpha = Mathf.Clamp01(elapsed / Duration);
			}
		}
		else
		{
			if (visible)
			{
				group.alpha = 0.0f;

				gameObject.SetActive(false);
			}
			else
			{
				group.alpha = 1.0f;
			}

			if (action != null)
			{
				action();

				action = null;
			}

			enabled = false;
		}
	}

	#endregion

	#region Methods

	public static void FadeIn() { Instance._FadeIn(null); }

	public static void FadeIn(Action action) { Instance._FadeIn(action); }

	public static void FadeOut() { Instance._FadeOut(null); }

	public static void FadeOut(Action action) { Instance._FadeOut(action); }

	private void _FadeIn(Action action)
	{
		if (!visible)
		{
			float elapsed = Time.unscaledTime - startTime;
			if (elapsed < Duration)
			{
				if (this.action != null) this.action();
			}

			float t = Mathf.Clamp01(1.0f - elapsed / Duration);

			this.visible = true;
			this.startTime = Time.unscaledTime - Duration * t;
			this.action = action;

			gameObject.SetActive(true);
			enabled = true;
		}
	}

	private void _FadeOut(Action action)
	{
		if (visible)
		{
			float elapsed = Time.unscaledTime - startTime;
			if (elapsed < Duration)
			{
				if (this.action != null) this.action();
			}

			float t = Mathf.Clamp01(1.0f - elapsed / Duration);

			this.visible = false;
			this.startTime = Time.unscaledTime - Duration * t;
			this.action = action;

			gameObject.SetActive(true);
			enabled = true;
		}
	}

	#endregion
}
