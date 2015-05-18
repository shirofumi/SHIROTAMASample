using System.Text;
using UnityEngine;

public static class SpriteSelector
{
	#region Constants

	private const int BufferSize = 16;

	#endregion

	#region Fields

	private static StringBuilder builder = new StringBuilder(BufferSize, BufferSize);

	private static StringCache cache = new StringCache();

	#endregion

	#region Methods

	public static Sprite GetGround(GroundType type, int number)
	{
		return ThemeResourceManager.Get(GetGroundName(type, number));
	}

	public static Sprite GetPanel(PanelType type, int number)
	{
		return ThemeResourceManager.Get(GetPanelName(type, number));
	}

	public static Sprite GetWall(WallType type, int number)
	{
		return ThemeResourceManager.Get(GetWallName(type, number));
	}

	public static Sprite GetItem(ItemType type, int number)
	{
		return ThemeResourceManager.Get(GetItemName(type, number));
	}

	public static Sprite GetBarrier(BarrierType type, BarrierScale scale)
	{
		return ThemeResourceManager.Get(GetBarrierName(type, scale));
	}

	public static string GetGroundName(GroundType type, int number)
	{
		builder.Length = 0;
		AppendThemeSeq();
		AppendGroundSeq(type, number);
		return cache.Get(builder);
	}

	public static string GetPanelName(PanelType type, int number)
	{
		builder.Length = 0;
		AppendThemeSeq();
		AppendPanelSeq(type, number);
		return cache.Get(builder);
	}

	public static string GetWallName(WallType type, int number)
	{
		builder.Length = 0;
		AppendThemeSeq();
		AppendWallSeq(type, number);
		return cache.Get(builder);
	}

	public static string GetItemName(ItemType type, int number)
	{
		builder.Length = 0;
		AppendThemeSeq();
		AppendItemSeq(type, number);
		return cache.Get(builder);
	}

	public static string GetBarrierName(BarrierType type, BarrierScale scale)
	{
		builder.Length = 0;
		AppendThemeSeq();
		AppendBarrierSeq(type, scale);
		return cache.Get(builder);
	}

	private static void AppendThemeSeq()
	{
		Theme theme = GameScene.Layer.Theme;
		string prefix = ThemePrefix.Get(theme);

		builder.Append(prefix);
	}

	private static void AppendGroundSeq(GroundType type, int number)
	{
		string prefix = GroundPrefix.Get(type);

		builder.Append('g');
		builder.Append(prefix);

		switch (type)
		{
			case GroundType.Normal:
			case GroundType.Rough:
			case GroundType.NormalToRough:
				builder.Append(number);
				break;
		}
	}


	private static void AppendPanelSeq(PanelType type, int number)
	{
		string prefix = PanelPrefix.Get(type);

		builder.Append('p');
		builder.Append(prefix);

		switch (type)
		{
			case PanelType.Crack:
				builder.Append(number);
				break;
		}
	}

	private static void AppendWallSeq(WallType type, int number)
	{
		string prefix = WallPrefix.Get(type);

		builder.Append('w');
		builder.Append(prefix);

		switch (type)
		{
			case WallType.Breakable:
				builder.Append(number);
				break;
		}
	}

	private static void AppendItemSeq(ItemType type, int number)
	{
		string prefix = ItemPrefix.Get(type);

		builder.Append("i");
		builder.Append(prefix);
		switch (type)
		{
			case ItemType.Small:
			case ItemType.Medium:
			case ItemType.Large:
				builder.Append(number);
				break;
		}
	}

	private static void AppendBarrierSeq(BarrierType type, BarrierScale scale)
	{
		string typePrefix = BarrierPrefix.Get(type);
		string scalePrefix = BarrierScalePrefix.Get(scale);

		builder.Append("b");
		builder.Append(typePrefix);
		builder.Append(scalePrefix);
	}

	#endregion
}