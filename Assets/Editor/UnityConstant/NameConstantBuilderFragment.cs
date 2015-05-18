using System;
using System.CodeDom;
using System.Collections.Generic;

public class NameConstantBuilderFragment : ICSharpClassBuilderFragment
{
	#region Fields

	private readonly string typeName;

	private readonly IEnumerable<string> names;

	#endregion

	#region Properties

	public string TypeName { get { return typeName; } }

	public IEnumerable<string> Names { get { return names; } }

	#endregion

	#region Constructors

	public NameConstantBuilderFragment(string typeName, IEnumerable<string> names)
	{
		this.typeName = typeName;
		this.names = names;
	}

	#endregion

	#region Methods

	public virtual void Edit(CodeCompileUnit unit)
	{
		CodeNamespace namesp = new CodeNamespace();
		unit.Namespaces.Add(namesp);

		CodeTypeDeclaration typeDecl = new CodeTypeDeclaration(typeName)
		{
			Attributes = MemberAttributes.Public,
		};
		namesp.Types.Add(typeDecl);

		EditClass(typeDecl);
	}

	protected virtual void EditClass(CodeTypeDeclaration typeDecl)
	{
		foreach (string name in names)
		{
			string id = name.Replace(" ", "");

			CodeMemberField field = new CodeMemberField(typeof(string), id)
			{
				Attributes = MemberAttributes.Public | MemberAttributes.Static | MemberAttributes.Final,
				InitExpression = new CodePrimitiveExpression(name)
			};

			typeDecl.Members.Add(field);
		}
	}

	#endregion
	
}

public class NameToValueConstantBuilderFragment : NameConstantBuilderFragment
{
	#region Fields

	private readonly IDictionary<string, int> table;

	#endregion

	#region Properties

	public IDictionary<string, int> Table { get { return table; } }

	#endregion

	#region Constructors

	public NameToValueConstantBuilderFragment(string typeName, IDictionary<string, int> table) : base(typeName, table.Keys) {
		this.table = table;
	}

	#endregion

	#region Methods

	protected override void EditClass(CodeTypeDeclaration typeDecl)
	{
		foreach (var pair in Table)
		{
			string name = pair.Key;
			int value = pair.Value;

			string id = name.Replace(" ", "");

			CodeMemberField field = new CodeMemberField(typeof(Int32), id)
			{
				Attributes = MemberAttributes.Public | MemberAttributes.Static | MemberAttributes.Final,
				InitExpression = new CodePrimitiveExpression(value)
			};

			typeDecl.Members.Add(field);
		}
	}

	#endregion
}
