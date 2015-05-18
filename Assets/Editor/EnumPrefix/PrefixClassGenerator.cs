using UnityEditor;

public static class PrefixClassGenerator
{
	#region Constants

	public const string FileName = "EnumPrefix.cs";

	#endregion

	#region Methods

	[MenuItem("Assets/Create/" + FileName)]
	public static void Generate()
	{
		CodeGenerationHelper.Generate(FileName, new ICSharpClassBuilderFragment[]{
			new NameBasePrefixBuilderFragment<global::Theme>("ThemePrefix"),
			new NameBasePrefixBuilderFragment<global::GroundType>("GroundPrefix")
			{
				Exceptions = {
					{global::GroundType.NormalToRough, "nr"},
				}
			},
			new NameBasePrefixBuilderFragment<global::PanelType>("PanelPrefix")
			{
				Exceptions = {
					{global::PanelType.RotationCW, "rcw"},
					{global::PanelType.RotationCCW, "rccw"},
				}
			},
			new NameBasePrefixBuilderFragment<global::WallType>("WallPrefix"),
			new NameBasePrefixBuilderFragment<global::ItemType>("ItemPrefix"),
			new NameBasePrefixBuilderFragment<global::BarrierType>("BarrierPrefix"),
			new NameBasePrefixBuilderFragment<global::BarrierScale>("BarrierScalePrefix"),
		});
	}

	#endregion
}
