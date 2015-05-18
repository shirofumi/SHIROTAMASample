using System;
using UnityEngine;

namespace Title
{
	public class DialogManager : SingletonMonoBehaviour<DialogManager>
	{
		#region Fields

		public GameObject Dialog;

		public GameObject Contents;

		private GameObject current;

		#endregion

		#region Messages

		private new void Awake()
		{
			base.Awake();

			Transform root = Contents.transform;
			for (int i = 0; i < root.childCount; i++)
			{
				Transform child = root.GetChild(i);

				child.gameObject.SetActive(false);
			}

			Dialog.SetActive(false);
		}

		#endregion

		#region Methods

		public static void Show(GameObject content) { Instance._Show(content); }

		public static void Hide() { Instance._Hide(null); }

		public static void Hide(GameObject content) { Instance._Hide(content); }

		private void _Show(GameObject content)
		{
			if (content == null) throw new ArgumentNullException("content");
			if (content.transform.parent != Contents.transform) throw new ArgumentException("Content must be a 'Contents' child.", "content");

			if (current != null)
			{
				current.SetActive(false);
			}

			content.SetActive(true);
			current = content;

			Dialog.SetActive(true);
		}

		private void _Hide(GameObject content)
		{
			if (content == current)
			{
				content.SetActive(false);

				current = null;
			}
			else if (content != null) return;

			Dialog.SetActive(false);
		}

		#endregion
	}
}
