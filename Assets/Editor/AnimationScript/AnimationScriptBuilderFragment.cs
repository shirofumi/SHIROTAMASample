using System;
using System.CodeDom;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor.Animations;

public class AnimationScriptBuilderFragment : ICSharpClassBuilderFragment
{
	#region Constants

	private const string NameClassName = "Name";

	private const string HashClassName = "Hash";

	private const string AnimatorFieldName = "animator";

	#endregion

	#region Properties

	public AnimatorController Source { get; private set; }

	#endregion

	#region Constructors

	public AnimationScriptBuilderFragment(AnimatorController source)
	{
		if (source == null) throw new ArgumentNullException("source");

		this.Source = source;
	}

	#endregion

	#region Methods

	public virtual void Edit(CodeCompileUnit unit)
	{
		CodeNamespace namesp = new CodeNamespace();
		unit.Namespaces.Add(namesp);

		CodeTypeDeclaration typeDecl = new CodeTypeDeclaration(Source.name)
		{
			TypeAttributes = TypeAttributes.Public,
			BaseTypes = { new CodeTypeReference(typeof(MonoBehaviour)) },
			CustomAttributes = {
				new CodeAttributeDeclaration(
					new CodeTypeReference(typeof(RequireComponent)),
					new CodeAttributeArgument(new CodeTypeOfExpression(typeof(Animator)))
				)
			}
		};
		namesp.Types.Add(typeDecl);

		EditClass(typeDecl);
	}

	protected virtual void EditClass(CodeTypeDeclaration typeDecl)
	{
		AddNameClass(typeDecl);
		AddHashClass(typeDecl);
		AddAnimatorField(typeDecl);
		AddParameterProperties(typeDecl);
		AddAwakeMethod(typeDecl);
	}

	private void AddNameClass(CodeTypeDeclaration typeDecl)
	{
		CodeTypeDeclaration nameTypeDecl = new CodeTypeDeclaration(NameClassName)
		{
			TypeAttributes = TypeAttributes.NestedPrivate,
		};
		nameTypeDecl.Members.AddRange(Source.parameters.Select(
			x => new CodeMemberField(typeof(string), x.name) {
				Attributes = MemberAttributes.Public | MemberAttributes.Const,
				InitExpression = new CodePrimitiveExpression(x.name),
			}
		).ToArray());

		typeDecl.Members.Add(nameTypeDecl);
	}

	private void AddHashClass(CodeTypeDeclaration typeDecl)
	{
		CodeTypeDeclaration hashTypeDecl = new CodeTypeDeclaration(HashClassName)
		{
			TypeAttributes = TypeAttributes.NestedPrivate,
		};
		hashTypeDecl.Members.AddRange(Source.parameters.Select(
			x => new CodeMemberField(typeof(int), x.name) {
				Attributes = MemberAttributes.Public | MemberAttributes.Static,
				InitExpression = new CodeMethodInvokeExpression(
					new CodeTypeReferenceExpression(typeof(Animator)),
					"StringToHash",
					new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(NameClassName), x.name)
				),
			}
		).ToArray());

		typeDecl.Members.Add(hashTypeDecl);
	}

	private void AddAnimatorField(CodeTypeDeclaration typeDecl)
	{
		typeDecl.Members.Add(new CodeMemberField(typeof(Animator), AnimatorFieldName) { Attributes = MemberAttributes.Private });
	}

	private void AddParameterProperties(CodeTypeDeclaration typeDecl)
	{
		typeDecl.Members.AddRange(Source.parameters.Select(
			x => new CodeMemberProperty()
			{
				Name = x.name,
				Attributes = MemberAttributes.Public | MemberAttributes.Final,
				Type = new CodeTypeReference(GetTypeFromParameterType(x.type)),
				GetStatements = { CreateGetStatement(x) },
				SetStatements = { CreateSetStatement(x) },
			}
		).ToArray());
	}

	private void AddAwakeMethod(CodeTypeDeclaration typeDecl)
	{
		typeDecl.Members.Add(new CodeMemberMethod()
		{
			Name = "Awake",
			Attributes = MemberAttributes.Private,
			Parameters = { },
			ReturnType = new CodeTypeReference(typeof(void)),
			Statements = {
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), AnimatorFieldName),
					new CodeMethodInvokeExpression(
						new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "GetComponent", new CodeTypeReference(typeof(Animator)))
					)
				)
			}
		});
	}

	private CodeStatement CreateGetStatement(AnimatorControllerParameter parameter)
	{
		return new CodeMethodReturnStatement(
			parameter.type != AnimatorControllerParameterType.Trigger
			? (CodeExpression)
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), AnimatorFieldName),
					"Get" + GetMethodSuffixFromParameterType(parameter.type),
					new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(HashClassName), parameter.name)
				)
			: (CodeExpression)
				new CodePrimitiveExpression(false)
		);
	}

	private CodeStatement CreateSetStatement(AnimatorControllerParameter parameter)
	{
		return new CodeExpressionStatement(
			parameter.type != AnimatorControllerParameterType.Trigger
			? (CodeExpression)
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), AnimatorFieldName),
					"Set" + GetMethodSuffixFromParameterType(parameter.type),
					new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(HashClassName), parameter.name),
					new CodePropertySetValueReferenceExpression()
				)
			: (CodeExpression)
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), AnimatorFieldName),
					"Set" + GetMethodSuffixFromParameterType(parameter.type),
					new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(HashClassName), parameter.name)
				)
		);
	}

	private Type GetTypeFromParameterType(AnimatorControllerParameterType type)
	{
		switch (type)
		{
			case AnimatorControllerParameterType.Int: return typeof(Int32);
			case AnimatorControllerParameterType.Float: return typeof(Single);
			case AnimatorControllerParameterType.Bool: return typeof(Boolean);
			case AnimatorControllerParameterType.Trigger: return typeof(Boolean);
			default:
				throw new NotSupportedException("Unsupported type.");
		}
	}

	private string GetMethodSuffixFromParameterType(AnimatorControllerParameterType type)
	{
		switch (type)
		{
			case AnimatorControllerParameterType.Int: return "Int";
			case AnimatorControllerParameterType.Float: return "Float";
			case AnimatorControllerParameterType.Bool: return "Bool";
			case AnimatorControllerParameterType.Trigger: return "Trigger";
			default:
				throw new NotSupportedException("Unsupported type.");
		}
	}

	#endregion
}