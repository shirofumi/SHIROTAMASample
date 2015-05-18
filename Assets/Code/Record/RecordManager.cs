using System;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public static class RecordManager
{
	#region Fields

	private static volatile GameRecord record;

	private static volatile bool saved;

	private static readonly string RelativePath = "/Savedata/record.bin";

	private static readonly string RelativeBackupPath = "/Savedata/Backup/";

	private static readonly string BackupFormat = "yyyyMMdd_HHmmss_fff'.backup'";

	private static readonly string TempPath = Application.temporaryCachePath + RelativePath;

	private static readonly string TempDirectoryPath = Path.GetDirectoryName(TempPath);

	private static readonly string DataPath = Application.persistentDataPath + RelativePath;

	private static readonly string DataDirectoryPath = Path.GetDirectoryName(DataPath);

	private static readonly string BackupPath = Application.persistentDataPath + RelativeBackupPath;

	private static readonly string BackupDirectoryPath = Path.GetDirectoryName(BackupPath);

	private static readonly byte[] Key = Encoding.Unicode.GetBytes(Application.bundleIdentifier);

	private static readonly string Version = SnapshotHelper.DefaultVersion;

	private static readonly object syncObject = new object();

	#endregion

	#region Properties

	public static GameRecord Record
	{
		get
		{
			GameRecord value = record;
			if (value == null)
			{
				try
				{
					LoadRecord();
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}

				return record;
			}

			return value;
		}
	}

	#endregion

	#region Methods

	public static LevelRecord GetLevel(int index)
	{
		return Record.Levels[index - 1];
	}

	public static LevelRecord GetLevel(MapID map)
	{
		return Record.Levels[map.Primary - 1];
	}

	public static MapRecord GetMap(MapID map)
	{
		return Record.Levels[map.Primary - 1].Maps[map.Secondary - 1];
	}

	public static void Update(MapID map, MapRecord record)
	{
		GameRecord grec = Record;
		LevelRecord lrec = grec.Levels[map.Primary - 1];
		MapRecord mrec = lrec.Maps[map.Secondary - 1];

		lrec.Maps[map.Secondary - 1] = mrec.Merge(record);

		Save();
	}

	public static void Save()
	{
		ThreadPool.QueueUserWorkItem(context =>
		{
			try
			{
				SaveRecord();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		});
	}

	public static void Load()
	{
		ThreadPool.QueueUserWorkItem(context =>
		{
			try
			{
				LoadRecord();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		});
	}

	private static void SaveRecord()
	{
		if (record == null) return;

		lock (syncObject)
		{
			Directory.CreateDirectory(TempDirectoryPath);
			Directory.CreateDirectory(DataDirectoryPath);
			Directory.CreateDirectory(BackupDirectoryPath);

			SnapshotHelper.Save<GameRecord>(TempPath, record, Key, Version);

			FileInfo src = new FileInfo(TempPath);
			FileInfo dst = new FileInfo(DataPath);
			if (dst.Exists)
			{
				FileInfo backup = new FileInfo(BackupPath + DateTime.Now.ToString(BackupFormat));
				backup.Delete();

				File.Replace(src.FullName, dst.FullName, backup.FullName, true);

				if (saved)
				{
					backup.Delete();
				}

				saved = true;
			}
			else
			{
				src.MoveTo(dst.FullName);
			}
		}
	}

	private static void LoadRecord()
	{
		if (record != null) return;

		lock (syncObject)
		{
			if (record != null) return;

			try
			{
				FileInfo file = new FileInfo(DataPath);
				if (file.Exists)
				{
					string version;
					record = SnapshotHelper.Load<GameRecord>(DataPath, Key, out version);
					saved = true;
				}
			}
			finally
			{
				if (record == null)
				{
					record = new GameRecord(GameConstants.Levels, GameConstants.AllMaps);
					saved = false;
				}
			}
		}
	}

	#endregion
}
