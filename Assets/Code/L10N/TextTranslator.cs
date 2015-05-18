using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextTranslator : BatchMonoBehaviour<TextTranslator>
{
	#region Fields

	private string @default;

	public string Japanese;

	#endregion

	#region Messages

	private void Awake()
	{
		this.@default = GetComponent<Text>().text;
	}

	private new void OnEnable()
	{
		base.OnEnable();

		UpdateText();
	}

	#endregion

	#region Methods

	protected override void Process()
	{
		UpdateText();
	}

	private void UpdateText()
	{
		Text target = GetComponent<Text>();

		switch (LanguageManager.Language)
		{
			case LanguageManager.DefaultLanguage:
				target.text = @default;
				break;
			case SystemLanguage.Japanese:
				target.text = Japanese;
				break;
		}
	}

	#endregion
}
