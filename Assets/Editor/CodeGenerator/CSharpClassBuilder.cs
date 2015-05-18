using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

using Microsoft.CSharp;

public class CSharpClassBuilder
{
	#region Methods

	public string Build(params ICSharpClassBuilderFragment[] fragments)
	{
		CodeCompileUnit cu = new CodeCompileUnit();

		foreach (var fragment in fragments)
		{
			fragment.Edit(cu);
		}

		string code;
		using (var writer = new StringWriter())
		{
			CSharpCodeProvider provider = new CSharpCodeProvider();
			CodeGeneratorOptions options = new CodeGeneratorOptions();
			provider.GenerateCodeFromCompileUnit(cu, writer, options);

			code = writer.ToString();
		}

		return code;
	}

	#endregion
}