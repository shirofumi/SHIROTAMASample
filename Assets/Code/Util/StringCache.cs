using System.Collections.Generic;
using System.Text;

public class StringCache
{
	#region Fields

	private Dictionary<object, string> cache;

	private static readonly Comparer comparer = new Comparer();

	#endregion

	#region Constructors

	public StringCache()
	{
		this.cache = null;
	}

	public StringCache(int capacity)
	{
		this.cache = new Dictionary<object, string>(capacity, comparer);
	}

	#endregion

	#region Methods

	public string Get(StringBuilder builder)
	{
		if (this.cache == null)
		{
			this.cache = new Dictionary<object, string>(comparer);
		}

		string value;
		if (!cache.TryGetValue(builder, out value))
		{
			value = builder.ToString();

			cache.Add(value, value);
		}

		return value;
	}

	public void Clear()
	{
		this.cache = null;
	}

	#endregion

	#region Comparer

	private class Comparer : IEqualityComparer<object>
	{
		#region Fields

		private const int FNVPrime = (1 << 24) + (1 << 8) + 0x93;

		private const int OffsetBasis = unchecked((int)2166136261);

		#endregion

		#region Methods

		public new bool Equals(object x, object y)
		{
			if (x == null) return (y == null);

			StringBuilder builder;

			builder = x as StringBuilder;
			if (builder != null)
			{
				return Equals(builder, (string)y);
			}

			builder = y as StringBuilder;
			if (builder != null)
			{
				return Equals(builder, (string)x);
			}

			return EqualityComparer<string>.Default.Equals((string)x, (string)y);
		}

		public int GetHashCode(object obj)
		{
			unchecked
			{
				StringBuilder builder = obj as StringBuilder;
				if (builder != null)
				{
					int hash = OffsetBasis;
					for (int i = 0, half = builder.Length >> 1; i < half; ++i)
					{
						int j = i << 1;
						int n = builder[j] << 16 | builder[j + 1];
						hash = (hash * FNVPrime) ^ n;
					}
					if ((builder.Length & 0x01) != 0) hash = (hash * FNVPrime) ^ builder[builder.Length - 1];

					return hash;
				}
				else
				{
					string s = (string)obj;

					int hash = OffsetBasis;
					for (int i = 0, half = s.Length >> 1; i < half; ++i)
					{
						int j = i << 1;
						int n = s[j] << 16 | s[j + 1];
						hash = (hash * FNVPrime) ^ n;
					}
					if ((s.Length & 0x01) != 0) hash = (hash * FNVPrime) ^ s[s.Length - 1];

					return hash;
				}
			}
		}

		private bool Equals(StringBuilder x, string y)
		{
			if (x.Length != y.Length) return false;

			for (int i = 0, len = x.Length; i < len; i++)
			{
				if (x[i] != y[i]) return false;
			}

			return true;
		}

		#endregion
	}

	#endregion
}
