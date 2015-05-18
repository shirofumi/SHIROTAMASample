using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public class StringDictionary : Dictionary<string, object>
{
	#region Constructors

	public StringDictionary() : base() { }

	public StringDictionary(IDictionary<string, object> dictionary) : base(dictionary) { }

	public StringDictionary(IEqualityComparer<string> comparer) : base(comparer) { }

	public StringDictionary(int capacity) : base(capacity) { }

	public StringDictionary(IDictionary<string, object> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer) { }

	public StringDictionary(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer) { }

	protected StringDictionary(SerializationInfo info, StreamingContext context) :base(info, context) { }

	#endregion
}