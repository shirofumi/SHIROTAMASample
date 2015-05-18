using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionText : ScriptableObject
{
	#region Fields

	public string FastCompletion;

	public string LessBarrier;

	public string LessHitting;

	public string MoreHitting;

	public string LessSlugging;

	public string MoreSlugging;

	public string MoreAcceleration;

	public string MoreDicretionChange;

	public string MoreRotation;

	public string InitialBarrier;

	public string ExciteBall;

	public string DontExciteBall;

	public string StopBall;

	public string DontStopBall;

	public string BreakAll;

	private Dictionary<MissionType, string> templates;

	private static MissionText cache;

	private static SystemLanguage cacheLanguage = SystemLanguage.Unknown;

	#endregion

	#region Properties

	public Dictionary<MissionType, string> Templates
	{
		get
		{
			return templates ?? (templates = new Dictionary<MissionType, string>()
			{
				{ MissionType.FastCompletion, FastCompletion },
				{ MissionType.LessBarrier, LessBarrier },
				{ MissionType.LessHitting, LessHitting },
				{ MissionType.MoreHitting, MoreHitting },
				{ MissionType.LessSlugging, LessSlugging },
				{ MissionType.MoreSlugging, MoreSlugging },
				{ MissionType.MoreAcceleration, MoreAcceleration },
				{ MissionType.MoreDicretionChange, MoreDicretionChange },
				{ MissionType.MoreRotation, MoreRotation },
				{ MissionType.InitialBarrier, InitialBarrier },
				{ MissionType.ExciteBall, ExciteBall },
				{ MissionType.DontExciteBall, DontExciteBall },
				{ MissionType.StopBall, StopBall },
				{ MissionType.DontStopBall, DontStopBall },
				{ MissionType.BreakAll, BreakAll },
			});
		}}

	#endregion

	#region Methods

	public string Get(Mission mission)
	{
		return String.Format(Templates[mission.Type], mission.Option);
	}

	public static string GetDefault(Mission mission)
	{
		SystemLanguage language = LanguageManager.Language;
		if (language != cacheLanguage)
		{
			cache = Resources.Load<MissionText>(GetResourceName(language));
			cacheLanguage = language;
		}

		return cache.Get(mission);
	}

	public static string GetResourceName(SystemLanguage language)
	{
		string name;
		if (LanguageManager.TryGetLanguageName(language, out name))
		{
			return String.Concat(ResourceConstants.LanguagePath, name);
		}

		throw new ArgumentOutOfRangeException("language");
	}

	#endregion
}
