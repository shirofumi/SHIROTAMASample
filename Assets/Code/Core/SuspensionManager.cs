using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public static class SuspensionManager
{
	#region Fields

	private static readonly string RelativePath = "/SuspensionData/context.bin";

	private static readonly string DataPath = Application.persistentDataPath + RelativePath;

	private static readonly string DirectoryPath = Path.GetDirectoryName(DataPath);

	private static readonly byte[] Key = SnapshotHelper.DefaultKey;

	private static readonly string Version = SnapshotHelper.DefaultVersion;

	private static readonly object syncObject = new object();

	#endregion

	#region Methods

	public static bool Exists()
	{
		return File.Exists(DataPath);
	}

	public static void Delete()
	{
		lock (syncObject)
		{
			FileInfo file = new FileInfo(DataPath);
			if (file.Exists)
			{
				file.Delete();
			}
		}
	}

	public static void Save()
	{
		Monitor.Enter(syncObject);

		ThreadPool.QueueUserWorkItem(context =>
		{
			try
			{
				SaveContext();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
			finally
			{
				Monitor.Exit(syncObject);
			}
		});
	}

	public static bool Load()
	{
		lock (syncObject)
		{
			try
			{
				return LoadContext();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}

		return false;
	}

	private static void SaveContext()
	{
		Directory.CreateDirectory(DirectoryPath);

		SnapshotHelper.Save<Dictionary<string, object>>(DataPath, Context.Data, Key, Version);
	}

	private static bool LoadContext()
	{
		string version;
		var data = SnapshotHelper.Load<Dictionary<string, object>>(DataPath, Key, out version);

		if (data != null && version == Version)
		{
			Context.Data.Clear();
			foreach (var pair in data)
			{
				Context.Data.Add(pair.Key, pair.Value);
			}

			return true;
		}

		return false;
	}

	#endregion
}
