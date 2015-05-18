using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

[Serializable]
public class BareList<T> : IList<T>, IList
{
	#region Constants

	private const int DefaultCapacity = 4;

	private const int ResizeRatio = 2;

	private const double TrimmingThreshold = 0.9;

	#endregion

	#region Fields

	private T[] items;

	private int size;

	[NonSerialized]
	private object syncRoot;

	private static readonly T[] empty = new T[0];

	#endregion

	#region Properties

	public T[] Items { get { return items; } }

	public int Capacity
	{
		get { return items.Length; }
		set
		{
			if (value < size) throw new ArgumentOutOfRangeException("value");

			if (value != items.Length)
			{
				T[] newArray = new T[value];
				if (size > 0) System.Array.Copy(this.items, 0, newArray, 0, size);

				this.items = newArray;
			}
		}
	}

	public int Count
	{
		get { return size; }
	}

	public T this[int index]
	{
		get { return items[index]; }
		set { items[index] = value; }
	}

	object IList.this[int index]
	{
		get { return items[index]; }
		set { items[index] = (T)value; }
	}

	bool IList.IsFixedSize
	{
		get { return false; }
	}

	bool IList.IsReadOnly
	{
		get { return false; }
	}

	bool ICollection<T>.IsReadOnly
	{
		get { return false; }
	}


	bool ICollection.IsSynchronized
	{
		get { return false; }
	}

	object ICollection.SyncRoot
	{
		get {
			if (syncRoot == null) Interlocked.CompareExchange<object>(ref syncRoot, new object(), null);

			return syncRoot;
		}
	}

	#endregion

	#region Constructors

	public BareList()
	{
		this.items = empty;
	}

	public BareList(int capacity)
	{
		if (capacity < 0) throw new ArgumentOutOfRangeException("capacity");
		
		this.items = (capacity != 0 ? new T[capacity] : empty);
	}

	public BareList(IEnumerable<T> collection)
	{
		if (collection == null) throw new ArgumentNullException("collection");

		ICollection<T> c = collection as ICollection<T>;
		if (c != null)
		{
			int count = c.Count;
			if (count == 0)
			{
				this.items = empty;
			}
			else
			{
				this.items = new T[count];
				this.size = count;
				c.CopyTo(items, 0);
			}
		}
		else
		{
			this.items = empty;
			foreach (T item in collection) Add(item);
		}
	}

	#endregion

	#region Methods

	#region Public

	public void Add(T item)
	{
		if (size >= items.Length) EnsureCapacity(size + 1);

		this.items[size++] = item;
	}

	public void Add(ref T item)
	{
		if (size >= items.Length) EnsureCapacity(size + 1);

		this.items[size++] = item;
	}

	public void AddRange(IEnumerable<T> collection)
	{
		this.InsertRange(this.size, collection);
	}

	public ReadOnlyCollection<T> AsReadOnly()
	{
		return new ReadOnlyCollection<T>(this);
	}

	public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
	{
		return System.Array.BinarySearch<T>(this.items, index, count, item, comparer);
	}

	public int BinarySearch(T item)
	{
		return this.BinarySearch(0, this.size, item, null);
	}

	public int BinarySearch(T item, IComparer<T> comparer)
	{
		return this.BinarySearch(0, this.size, item, comparer);
	}

	public int BinarySearch(int index, int count, ref T item, IComparer<T> comparer)
	{
		return System.Array.BinarySearch<T>(this.items, index, count, item, comparer);
	}

	public int BinarySearch(ref T item)
	{
		return this.BinarySearch(0, this.size, ref item, null);
	}

	public int BinarySearch(ref T item, IComparer<T> comparer)
	{
		return this.BinarySearch(0, this.size, ref item, comparer);
	}

	public void Clear()
	{
		if (this.size > 0)
		{
			System.Array.Clear(this.items, 0, this.size);
			this.size = 0;
		}
	}

	public bool Contains(T item)
	{
		if (item == null)
		{
			for (int i = 0; i < size; i++)
			{
				if (this.items[i] == null) return true;
			}
			return false;
		}
		else
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < size; i++)
			{
				if (@default.Equals(this.items[i], item)) return true;
			}
			return false;
		}
	}

	public bool Contains(ref T item)
	{
		if (item == null)
		{
			for (int i = 0; i < size; i++)
			{
				if (this.items[i] == null) return true;
			}
			return false;
		}
		else
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < size; i++)
			{
				if (@default.Equals(this.items[i], item)) return true;
			}
			return false;
		}
	}

	public BareList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
	{
		if (converter == null) throw new ArgumentNullException("converter");

		BareList<TOutput> list = new BareList<TOutput>(this.size);
		for (int i = 0; i < this.size; i++)
		{
			list.items[i] = converter(this.items[i]);
		}
		list.size = this.size;

		return list;
	}

	public void CopyTo(int index, T[] array, int arrayIndex, int count)
	{
		System.Array.Copy(this.items, index, array, arrayIndex, count);
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		this.CopyTo(0, array, arrayIndex, this.size);
	}

	public void CopyTo(T[] array)
	{
		this.CopyTo(0, array, 0, this.size);
	}

	public bool Exists(Predicate<T> match)
	{
		return this.FindIndex(match) != -1;
	}

	public T Find(Predicate<T> match)
	{
		if (match == null) throw new ArgumentNullException("match");

		for (int i = 0; i < this.size; i++)
		{
			if (match(this.items[i])) return this.items[i];
		}

		return default(T);
	}

	public void Find(Predicate<T> match, out T item)
	{
		if (match == null) throw new ArgumentNullException("match");

		for (int i = 0; i < this.size; i++)
		{
			if (match(this.items[i]))
			{
				item = this.items[i];
				return;
			}
		}

		item = default(T);
	}

	public BareList<T> FindAll(Predicate<T> match)
	{
		if (match == null) throw new ArgumentNullException("match");

		BareList<T> list = new BareList<T>();
		for (int i = 0; i < this.size; i++)
		{
			if (match(this.items[i])) list.Add(this.items[i]);
		}

		return list;
	}

	public int FindIndex(int startIndex, int count, Predicate<T> match)
	{
		unchecked
		{
			if ((uint)startIndex > (uint)this.size) throw new ArgumentOutOfRangeException("startIndex");
			if (count < 0 || count > this.size - startIndex) throw new ArgumentOutOfRangeException("count");
			if (match == null) throw new ArgumentNullException("match");

			for (int index = 0; index < this.size; index++)
			{
				if (match(this.items[index])) return index;
			}

			return -1;
		}
	}

	public int FindIndex(Predicate<T> match)
	{
		return this.FindIndex(0, this.size, match);
	}

	public int FindIndex(int startIndex, Predicate<T> match)
	{
		return this.FindIndex(startIndex, this.size - startIndex, match);
	}

	public T FindLast(Predicate<T> match)
	{
		if (match == null) throw new ArgumentNullException("match");

		for (int i = this.size - 1; i >= 0; i--)
		{
			if (match(this.items[i])) return this.items[i];
		}

		return default(T);
	}

	public void FindLast(Predicate<T> match, out T item)
	{
		if (match == null) throw new ArgumentNullException("match");

		for (int i = this.size - 1; i >= 0; i--)
		{
			if (match(this.items[i]))
			{
				item = this.items[i];
				return;
			}
		}

		item = default(T);
	}

	public int FindLastIndex(int startIndex, int count, Predicate<T> match)
	{
		unchecked
		{
			if ((uint)startIndex > (uint)this.size) throw new ArgumentOutOfRangeException("startIndex");
			if (count < 0 || count > this.size - startIndex) throw new ArgumentOutOfRangeException("count");
			if (match == null) throw new ArgumentNullException("match");

			for (int index = this.size - 1; index >= 0; index--)
			{
				if (match(this.items[index])) return index;
			}

			return -1;
		}
	}

	public int FindLastIndex(Predicate<T> match)
	{
		return this.FindIndex(this.size - 1, this.size, match);
	}

	public int FindLastIndex(int startIndex, Predicate<T> match)
	{
		return this.FindIndex(startIndex, startIndex + 1, match);
	}

	public void ForEach(Action<T> action)
	{
		if (action == null) throw new ArgumentNullException("action");

		for (int i = 0; i < this.size; i++)
		{
			action(this.items[i]);
		}
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(this);
	}

	public BareList<T> GetRange(int index, int count)
	{
		if (index < 0) throw new ArgumentOutOfRangeException("index");
		if (count < 0 || count > this.size - index) throw new ArgumentOutOfRangeException("count");

		BareList<T> list = new BareList<T>(count);
		System.Array.Copy(this.items, index, list.items, 0, count);
		list.size = this.size;

		return list;
	}

	public int IndexOf(T item, int index, int count)
	{
		return System.Array.IndexOf<T>(this.items, item, index, count);
	}

	public int IndexOf(T item)
	{
		return this.IndexOf(item, 0, this.size);
	}

	public int IndexOf(T item, int index)
	{
		return this.IndexOf(item, index, this.size - index);
	}

	public int IndexOf(ref T item, int index, int count)
	{
		return System.Array.IndexOf<T>(this.items, item, index, count);
	}

	public int IndexOf(ref T item)
	{
		return this.IndexOf(ref item, 0, this.size);
	}

	public int IndexOf(ref T item, int index)
	{
		return this.IndexOf(ref item, index, this.size - index);
	}

	public void Insert(int index, T item)
	{
		unchecked
		{
			if ((uint)index > (uint)this.size) throw new ArgumentOutOfRangeException("index");

			if (this.size == this.items.Length) EnsureCapacity(this.size + 1);

			if (index < this.size)
			{
				System.Array.Copy(this.items, index, this.items, index + 1, this.size - index);
			}
			this.items[index] = item;
			this.size++;
		}
	}

	public void Insert(int index, ref T item)
	{
		unchecked
		{
			if ((uint)index > (uint)this.size) throw new ArgumentOutOfRangeException("index");

			if (this.size == this.items.Length) EnsureCapacity(this.size + 1);

			if (index < this.size)
			{
				System.Array.Copy(this.items, index, this.items, index + 1, this.size - index);
			}
			this.items[index] = item;
			this.size++;
		}
	}

	public void InsertRange(int index, IEnumerable<T> collection)
	{
		unchecked
		{
			if ((uint)index > (uint)this.size) throw new ArgumentOutOfRangeException("index");
			if (collection == null) throw new ArgumentNullException("collection");

			ICollection<T> c = collection as ICollection<T>;
			if (c != null)
			{
				int count = c.Count;
				if (count > 0)
				{
					EnsureCapacity(this.size + count);

					if (index < this.size)
					{
						System.Array.Copy(this.items, index, this.items, index + count, this.size - index);
					}

					if (c == this)
					{
						System.Array.Copy(this.items, 0, this.items, index, index);
						System.Array.Copy(this.items, index + count, this.items, index * 2, this.size - index);
					}
					else
					{
						T[] temp = new T[count];
						c.CopyTo(temp, 0);
						temp.CopyTo(this.items, index);
					}

					this.size += count;
				}
			}
			else
			{
				foreach (T item in collection)
				{
					this.Insert(index++, item);
				}
			}
		}
	}

	public int LastIndexOf(T item, int index, int count)
	{
		return System.Array.LastIndexOf<T>(this.items, item, index, count);
	}

	public int LastIndexOf(T item)
	{
		return (this.size != 0 ? this.LastIndexOf(item, this.size - 1, this.size) : -1);
	}

	public int LastIndexOf(T item, int index)
	{
		return this.LastIndexOf(item, index, index + 1);
	}

	public int LastIndexOf(ref T item, int index, int count)
	{
		return System.Array.LastIndexOf<T>(this.items, item, index, count);
	}

	public int LastIndexOf(ref T item)
	{
		return (this.size != 0 ? this.LastIndexOf(ref item, this.size - 1, this.size) : -1);
	}

	public int LastIndexOf(ref T item, int index)
	{
		return this.LastIndexOf(ref item, index, index + 1);
	}

	public bool Remove(T item)
	{
		int index = this.IndexOf(item);
		if (index >= 0)
		{
			this.RemoveAt(index);

			return true;
		}

		return false;
	}

	public bool Remove(ref T item)
	{
		int index = this.IndexOf(item);
		if (index >= 0)
		{
			this.RemoveAt(index);

			return true;
		}

		return false;
	}

	public int RemoveAll(Predicate<T> match)
	{
		if (match == null) throw new ArgumentNullException("match");

		int index1, index2;
		for (index1 = 0; index1 < this.size && !match(this.items[index1]); index1++) { }
		for (index2 = index1 + 1; index2 < this.size; index2++)
		{
			if (match(this.items[index2])) this.items[index1++] = this.items[index2];
		}

		int n = this.size - index1;
		System.Array.Clear(this.items, index1, n);

		this.size = index1;

		return n;
	}

	public void RemoveAt(int index)
	{
		unchecked
		{
			if ((uint)index > (uint)this.size) throw new ArgumentOutOfRangeException("index");

			this.size--;
			if (index < this.size)
			{
				System.Array.Copy(this.items, index + 1, this.items, index, this.size - index);
			}
			this.items[this.size] = default(T);
		}
	}

	public void RemoveRange(int index, int count)
	{
		if (index < 0) throw new ArgumentOutOfRangeException("index");
		if (count < 0 || count > this.size - index) throw new ArgumentOutOfRangeException("count");

		if (count != 0)
		{
			this.size -= count;
			if (index < this.size)
			{
				System.Array.Copy(this.items, index + count, this.items, index, this.size - index);
			}
			System.Array.Clear(this.items, this.size, count);
		}
	}

	public void Reverse()
	{
		this.Reverse(0, this.size);
	}

	public void Reverse(int index, int count)
	{
		System.Array.Reverse(this.items, index, count);
	}

	public void Sort()
	{
		this.Sort(0, this.size, null);
	}

	public void Sort(IComparer<T> comparer)
	{
		this.Sort(0, this.size, comparer);
	}

	public void Sort(int index, int count, IComparer<T> comparer)
	{
		System.Array.Sort<T>(this.items, index, count, comparer);
	}

	public void Sort(Comparison<T> comparison)
	{
		if (comparison == null) throw new ArgumentNullException("comparison");

		System.Array.Sort<T>(this.items, 0, this.size, new FunctorComparer(comparison));
	}

	public T[] ToArray()
	{
		T[] array = new T[this.size];
		System.Array.Copy(this.items, 0, array, 0, this.size);
		return array;
	}

	public void TrimExcess()
	{
		if (this.size < (int)(this.items.Length * TrimmingThreshold))
		{
			this.Capacity = this.size;
		}
	}

	public bool TrueForAll(Predicate<T> match)
	{
		if (match == null) throw new ArgumentNullException("match");

		for (int i = 0; i < this.size; i++)
		{
			if (!match(this.items[i])) return false;
		}

		return true;
	}

	#endregion

	#region IList

	int IList.Add(object value)
	{
		Add((T)value);

		return (size - 1);
	}

	bool IList.Contains(object value)
	{
		return (IsCompatible(value) && this.Contains((T)value));
	}

	int IList.IndexOf(object value)
	{
		return (IsCompatible(value) ? this.IndexOf((T)value) : -1);
	}

	void IList.Insert(int index, object value)
	{
		Insert(index, (T)value);
	}

	void IList.Remove(object value)
	{
		if (IsCompatible(value)) this.Remove((T)value);
	}

	#endregion

	#region ICollection

	void ICollection.CopyTo(Array array, int index)
	{
		System.Array.Copy(this.items, 0, array, index, this.size);
	}

	#endregion

	#region IEnumerable

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new Enumerator(this);
	}

	#endregion

	#region Private

	private void EnsureCapacity(int min)
	{
		if (min < items.Length) return;

		int newSize = (items.Length != 0 ? items.Length * ResizeRatio : DefaultCapacity);
		if (newSize < min) newSize = min;

		this.Capacity = newSize;
	}

	private static bool IsCompatible(object value)
	{
		return (value is T || (value == null && default(T) == null));
	}

	#endregion

	#endregion

	#region Enumerator

	[Serializable]
	public struct Enumerator : IEnumerator<T>
	{
		#region Fields

		private readonly BareList<T> list;

		private int index;

		private T current;

		#endregion

		#region Properties

		public T Current
		{
			get { return current; }
		}

		object IEnumerator.Current
		{
			get { return current; }
		}

		#endregion

		#region Constructors

		internal Enumerator(BareList<T> list)
		{
			this.list = list;
			this.index = 0;
			this.current = default(T);
		}

		#endregion

		#region Methods

		public bool MoveNext()
		{
			unchecked
			{
				if ((uint)index >= (uint)list.size)
				{
					this.index = list.size + 1;
					this.current = default(T);
					
					return false;
				}

				this.current = list[this.index++];

				return true;
			}
		}

		public void Reset()
		{
			this.index = 0;
			this.current = default(T);
		}

		public void Dispose() { }

		#endregion
	}

	#endregion

	#region FunctorComparer

	[Serializable]
	private sealed class FunctorComparer : IComparer<T>
	{
		private readonly Comparison<T> comparison;

		public FunctorComparer(Comparison<T> comparison)
		{
			this.comparison = comparison;
		}

		public int Compare(T x, T y)
		{
			return this.comparison(x, y);
		}
	}

	#endregion
}