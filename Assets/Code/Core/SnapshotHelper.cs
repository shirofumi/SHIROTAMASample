using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SnapshotHelper
{
	#region Constants

	private const int VersionSize = 16;

	#endregion

	#region Fields

	private static byte[] defaultKey;

	private static string defaultVersion;

	#endregion

	#region Properties

	public static byte[] DefaultKey
	{
		get { return defaultKey ?? (defaultKey = GetDefaultKey()); }
	}

	public static string DefaultVersion
	{
		get { return defaultVersion ?? (defaultVersion = GetDefaultVersion()); }
	}

	#endregion

	#region Methods

	public static void ForceInitDefaults()
	{
		defaultKey = GetDefaultKey();
		defaultVersion = GetDefaultVersion();
	}

	public static void Save<T>(string path, T value)
	{
		Save(path, value, null, null);
	}

	public static void Save<T>(Stream stream, T value)
	{
		Save(stream, value, null, null);
	}

	public static void Save<T>(string path, T value, byte[] key)
	{
		Save(path, value, key, null);
	}

	public static void Save<T>(Stream stream, T value, byte[] key)
	{
		Save(stream, value, key, null);
	}

	public static void Save<T>(string path, T value, byte[] key, string version)
	{
		FileInfo file = new FileInfo(path);

		using (var stream = file.Open(FileMode.Create, FileAccess.Write))
		{
			Save<T>(stream, value, key, version);
		}
	}

	public static void Save<T>(Stream stream, T value, byte[] key, string version)
	{
		byte[] data = Serialize<T>(value, key, version);

		stream.Write(data, 0, data.Length);
	}

	public static T Load<T>(string path)
	{
		string version;
		return Load<T>(path, null, out version);
	}

	public static T Load<T>(Stream stream)
	{
		string version;
		return Load<T>(stream, null, out version);
	}

	public static T Load<T>(string path, byte[] key)
	{
		string version;
		return Load<T>(path, key, out version);
	}

	public static T Load<T>(Stream stream, byte[] key)
	{
		string version;
		return Load<T>(stream, key, out version);
	}

	public static T Load<T>(string path, byte[] key, out string version)
	{
		FileInfo file = new FileInfo(path);

		if (file.Exists)
		{
			using (var stream = file.Open(FileMode.Open, FileAccess.Read))
			{
				return Load<T>(stream, key, out version);
			}
		}

		version = null;

		return default(T);
	}

	public static T Load<T>(Stream stream, byte[] key, out string version)
	{
		byte[] data = new byte[stream.Length];
		stream.Read(data, 0, data.Length);

		return Deserialize<T>(data, key, out version);
	}

	private static byte[] Serialize<T>(T value, byte[] key, string version)
	{
		if (value == null) throw new ArgumentNullException("value");

		if (key == null) key = DefaultKey;
		if (version == null) version = DefaultVersion;

		int hashSize = 0;
		GetHash(null, null, ref hashSize);

		using (var stream = new MemoryStream())
		{
			byte[] ver = EncodeVersion(version);
			stream.Position = hashSize;
			stream.Write(ver, 0, ver.Length);

			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Context = new StreamingContext(StreamingContextStates.All, version);
			stream.Position = hashSize + VersionSize;
			formatter.Serialize(stream, value);

			stream.Position = hashSize;
			byte[] hash = GetHash(stream, key, ref hashSize);

			stream.Position = 0;
			stream.Write(hash, 0, hash.Length);

			return stream.ToArray();
		}
	}

	private static T Deserialize<T>(byte[] data, byte[] key, out string version)
	{
		if (data == null) throw new ArgumentNullException("data");

		if (key == null) key = DefaultKey;

		int hashSize = 0;
		GetHash(null, null, ref hashSize);

		using (var stream = new MemoryStream(data, false))
		{
			byte[] buffer = new byte[hashSize];
			stream.Read(buffer, 0, buffer.Length);

			stream.Position = hashSize;
			byte[] hash = GetHash(stream, key, ref hashSize);

			if (CompareHash(buffer, hash, hashSize))
			{
				byte[] ver = new byte[VersionSize];
				stream.Position = hashSize;
				stream.Read(ver, 0, ver.Length);

				version = DecodeVersion(ver);

				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Context = new StreamingContext(StreamingContextStates.All, version);
				stream.Position = hashSize + VersionSize;
				object value = formatter.Deserialize(stream);

				return (value is T ? (T)value : default(T));
			}
		}

		version = null;

		return default(T);
	}

	private static byte[] EncodeVersion(string version)
	{
		byte[] code = Encoding.ASCII.GetBytes(version);
		if (code.Length < VersionSize)
		{
			byte[] buffer = new byte[VersionSize];
			Array.Copy(code, 0, buffer, 0, code.Length);
			code = buffer;
		}
		else if (code.Length > VersionSize)
		{
			byte[] buffer = new byte[VersionSize];
			Array.Copy(code, 0, buffer, 0, VersionSize);
			code = buffer;
		}

		return code;
	}

	private static string DecodeVersion(byte[] code)
	{
		int count = Array.IndexOf<byte>(code, 0);
		return Encoding.ASCII.GetString(code, 0, count);
	}

	private static byte[] GetHash(Stream stream, byte[] key, ref int size)
	{
		const int HashSize = 256 / 8;

		if (stream == null)
		{
			size = HashSize;

			return null;
		}

		byte[] hash;
		HMACSHA256 hmac = new HMACSHA256(key);
		hash = hmac.ComputeHash(stream);
		hmac.Clear();

		if (hash.Length != size) throw new InvalidOperationException("Hash Size Mismatch.");

		return hash;
	}

	private static bool CompareHash(byte[] x, byte[] y, int size)
	{
		for (int i = 0; i < size; i++)
		{
			if (x[i] != y[i]) return false;
		}

		return true;
	}

	private static byte[] GetDefaultKey()
	{
		return Encoding.Unicode.GetBytes(Application.bundleIdentifier + SystemInfo.deviceUniqueIdentifier);
	}

	private static string GetDefaultVersion()
	{
		return Application.version;
	}

	#endregion
}
