using UnityEngine;
using System;
using System.Collections.Generic;

public static class LanguageManager
{
	#region Constants

	public const string LanguageKey = "Language";

	public const SystemLanguage DefaultLanguage = SystemLanguage.English;

	#endregion

	#region Fields

	private static SystemLanguage cache = SystemLanguage.Unknown;

	private static readonly Dictionary<SystemLanguage, string> Languages = new Dictionary<SystemLanguage,string>()
	{
		{ SystemLanguage.English, "English" },
		{ SystemLanguage.Japanese, "Japanese" },
	};

	#endregion

	#region Properties

	public static SystemLanguage Language
	{
		get
		{
			if (cache == SystemLanguage.Unknown)
			{
				cache = GetLanguage();
			}

			return cache;
		}
		set
		{
			SetLanguage(value);

			cache = SystemLanguage.Unknown;
		}
	}

	public static string LanguageName
	{
		get { return Languages[Language]; }
	}

	#endregion

	#region Methods

	public static void Refresh()
	{
		cache = SystemLanguage.Unknown;
	}

	public static bool TryGetLanguageName(SystemLanguage language, out string name)
	{
		return Languages.TryGetValue(language, out name);
	}

	private static SystemLanguage GetLanguage()
	{
		SystemLanguage value;
		if (TryGetLanguageFromSettings(out value)) return value;

		value = Application.systemLanguage;
		if (!Languages.ContainsKey(value))
		{
			value = DefaultLanguage;
		}

		SetLanguage(value);

		return value;
	}

	private static bool TryGetLanguageFromSettings(out SystemLanguage value)
	{
		value = (SystemLanguage)PlayerPrefs.GetInt(LanguageKey, (int)SystemLanguage.Unknown);

		return Languages.ContainsKey(value);
	}

	private static void SetLanguage(SystemLanguage value)
	{
		if (Languages.ContainsKey(value))
		{
			PlayerPrefs.SetInt(LanguageKey, (int)value);
		}
	}

	#endregion
}
