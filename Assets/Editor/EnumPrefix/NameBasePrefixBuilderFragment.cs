using System;
using System.CodeDom;
using System.Collections.Generic;

public class NameBasePrefixBuilderFragment<T> : ICSharpClassBuilderFragment
{
	#region Fields

	private Dictionary<T, string> exceptions = new Dictionary<T, string>();

	#endregion

	#region Properties

	public string Name { get; private set; }

	public IDictionary<T, string> Exceptions { get { return exceptions; } }

	#endregion

	#region Constructors

	public NameBasePrefixBuilderFragment(string name)
	{
		if (name == null) throw new ArgumentNullException();
		if (!typeof(T).IsEnum) throw new InvalidOperationException();

		this.Name = name;
	}

	#endregion

	#region Methods

	public virtual void Edit(CodeCompileUnit unit)
	{
		CodeNamespace namesp = new CodeNamespace();
		unit.Namespaces.Add(namesp);

		CodeTypeDeclaration typeDecl = new CodeTypeDeclaration(Name)
		{
			Attributes = MemberAttributes.Public,
		};
		namesp.Types.Add(typeDecl);

		EditClass(typeDecl);
	}

	protected virtual void EditClass(CodeTypeDeclaration typeDecl)
	{
		Type type = typeof(T);

		string fieldName = "prefix";
		string paramName = type.Name.ToLower();
		string varName = "result";

		CodeMemberField field = GetPrefixMapField(type, fieldName);
		typeDecl.Members.Add(field);

		CodeTypeConstructor constructor = GetTypeConstructor(type, fieldName);
		typeDecl.Members.Add(constructor);

		CodeMemberMethod method = GetGetMethod(type, fieldName, paramName, varName);
		typeDecl.Members.Add(method);
	}

	private static CodeMemberField GetPrefixMapField(Type type, string fieldName)
	{
		string[] names = Enum.GetNames(type);
		Type dictType = typeof(Dictionary<,>).MakeGenericType(type, typeof(string));
		CodeMemberField field = new CodeMemberField(dictType, fieldName)
		{
			Attributes = MemberAttributes.Private | MemberAttributes.Static | MemberAttributes.Final,
			InitExpression = new CodeObjectCreateExpression(dictType, new CodePrimitiveExpression(names.Length))
		};

		return field;
	}

	private CodeTypeConstructor GetTypeConstructor(Type type, string fieldName)
	{
		CodeTypeConstructor constructor = new CodeTypeConstructor();
		foreach (T value in Enum.GetValues(type))
		{
			string name = Enum.GetName(type, value);

			string prefix;
			if (!exceptions.TryGetValue(value, out prefix))
			{
				prefix = (name != "None" ? Char.ToLower(name[0]).ToString() : null);
			}

			if (prefix != null)
			{
				constructor.Statements.Add(
					new CodeMethodInvokeExpression(
						new CodeVariableReferenceExpression(fieldName),
						"Add",
						new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(type), name),
						new CodePrimitiveExpression(prefix)
					)
				);
			}
		}

		return constructor;
	}

	private static CodeMemberMethod GetGetMethod(Type type, string fieldName, string paramName, string varName)
	{
		CodeMemberMethod method = new CodeMemberMethod()
		{
			Name = "Get",
			Attributes = MemberAttributes.Public | MemberAttributes.Static,
			Parameters = {
				new CodeParameterDeclarationExpression(type, paramName)
			},
			ReturnType = new CodeTypeReference(typeof(string)),
			Statements =
			{
				new CodeVariableDeclarationStatement(typeof(string), varName),
				new CodeConditionStatement(
					new CodeMethodInvokeExpression(
						new CodeVariableReferenceExpression(fieldName),
						"TryGetValue",
						new CodeVariableReferenceExpression(paramName),
						new CodeDirectionExpression(FieldDirection.Out, new CodeVariableReferenceExpression(varName))
					),
					new CodeMethodReturnStatement(
						new CodeVariableReferenceExpression(varName)
					)
				),
				new CodeMethodReturnStatement(new CodePrimitiveExpression(null))
			}
		};

		return method;
	}

	#endregion
}
