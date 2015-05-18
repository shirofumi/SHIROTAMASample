using System;
using System.Collections.Generic;
using UnityEngine;

public static class ThemeResourceManager
{
	#region Fields

	public static readonly int MaxTheme = 3;

	#endregion

	#region Constructors

	static ThemeResourceManager()
	{
		ResourceSet.Init(MaxTheme);
	}

	#endregion

	#region Methods

	public static Sprite Get(string name)
	{
		return ResourceSet.First.Sprites[name];
	}

	public static void Load(Theme theme)
	{
		if (theme == Theme.None) return;

		ResourceSet node = ResourceSet.First;
		do
		{
			if (node.Theme == theme)
			{
				node.SetFirst();

				return;
			}
			else if (node.Theme == Theme.None)
			{
				LoadCore(node, theme);

				return;
			}
		} while ((node = node.Next) != ResourceSet.First);

		node = ResourceSet.Last;
		UnloadCore(node);
		LoadCore(node, theme);
	}

	public static void Unload(Theme theme)
	{
		if (theme == Theme.None) return;

		ResourceSet node = ResourceSet.First;
		do
		{
			if (node.Theme == theme)
			{
				UnloadCore(node);
				node.SetLast();
				break;
			}
		} while ((node = node.Next) != ResourceSet.First);
	}

	public static void UnloadAll()
	{
		ResourceSet node = ResourceSet.First;
		do
		{
			UnloadCore(node);
		} while ((node = node.Next) != ResourceSet.First);
	}

	private static void LoadCore(ResourceSet resource, Theme theme)
	{
		string name = GetResourceName(theme);
		Sprite[] sprites = Resources.LoadAll<Sprite>(name);

		resource.Set(theme, sprites);

		resource.SetFirst();
	}

	private static void UnloadCore(ResourceSet resource)
	{
		foreach (var pair in resource.Sprites)
		{
			Resources.UnloadAsset(pair.Value);
		}

		resource.Clear();

		resource.SetLast();
	}

	private static string GetResourceName(Theme theme)
	{
		string name = Enum.GetName(typeof(Theme), theme);

		return (name != null ? String.Concat(ResourceConstants.ThemeSpritePath, name) : null);
	}

	#endregion

	#region ResourceSet

	private class ResourceSet
	{
		#region Fields

		public global::Theme Theme;

		public readonly Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();

		private ResourceSet prev, next;

		private static ResourceSet first;

		#endregion

		#region Properties

		public ResourceSet Next { get { return next; } }

		public static ResourceSet First { get { return first; } }

		public static ResourceSet Last { get { return first.prev; } }

		#endregion

		#region Methods

		public static void Init(int count)
		{
			ResourceSet node = new ResourceSet();
			node.prev = node.next = node;

			for (int i = 1; i < count; i++)
			{
				ResourceSet @new = new ResourceSet();
				@new.prev = node;
				@new.next = node.next;
				@new.prev.next = @new;
				@new.next.prev = @new;
			}

			first = node;
		}

		public void SetFirst()
		{
			if (this != first)
			{
				SetLast();

				first = this;
			}
		}

		public void SetLast()
		{
			if (this != first.prev)
			{
				if (this == first)
				{
					first = this.next;
				}
				else
				{
					this.prev.next = this.next;
					this.next.prev = this.prev;

					this.prev = first.prev;
					this.next = first;

					this.prev.next = this;
					this.next.prev = this;
				}
			}
		}

		public void Set(global::Theme theme, Sprite[] sprites)
		{
			this.Theme = theme;
			foreach (Sprite sprite in sprites)
			{
				this.Sprites.Add(sprite.name, sprite);
			}
		}

		public void Clear()
		{
			this.Theme = global::Theme.None;
			this.Sprites.Clear();
		}

		#endregion
	}

	#endregion
}