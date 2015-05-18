using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

[Serializable]
public class RingList<T> : IList<T>, IList
{
	#region Constants

	private const int DefaultCapacity = 4;

	#endregion

	#region Fields

	private T[] items;

	private int offset, size;

	[NonSerialized]
	private object syncRoot;

	private static readonly T[] empty = new T[0];

	#endregion

	#region Properties

	public int Capacity
	{
		get { return this.items.Length; }
		set
		{
			if (value < this.size) throw new ArgumentOutOfRangeException("value");

			int newSize = Pow2RoundUp(value);
			if (newSize < this.size) throw new ArgumentOutOfRangeException("value", "'value' is too large.");

			if (newSize != this.items.Length)
			{
				T[] newArray = new T[newSize];
				if (this.size > 0)
				{
					if (IsSplit)
					{
						System.Array.Copy(this.items, this.offset, newArray, 0, this.items.Length - this.offset);
						System.Array.Copy(this.items, 0, newArray, this.items.Length - this.offset, this.offset + this.size - this.items.Length);
					}
					else
					{
						System.Array.Copy(this.items, this.offset, newArray, 0, this.size);
					}
				}
				this.items = newArray;
			}
		}
	}

	public int Count
	{
		get { return this.size; }
	}

	public T this[int index]
	{
		get { return this.items[(this.offset + index) & (this.Capacity - 1)]; }
		set { this.items[(this.offset + index) & (this.Capacity - 1)] = value; }
	}

	bool IList.IsFixedSize
	{
		get { return false; }
	}

	bool IList.IsReadOnly
	{
		get { return false; }
	}

	object IList.this[int index]
	{
		get { return this[index]; }
		set { this[index] = (T)value; }
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

	private bool IsSplit
	{
		get { return (this.offset > this.Capacity - this.size); }
	}

	#endregion

	#region Constructors

	public RingList()
	{
		this.items = empty;
	}

	public RingList(int capacity)
	{
		if (capacity < 0) throw new ArgumentOutOfRangeException("capacity");

		int newSize = Pow2RoundUp(capacity);
		if (newSize < capacity) throw new ArgumentOutOfRangeException("capacity", "'capacity' is too large.");

		this.items = (newSize != 0 ? new T[newSize] : empty);
	}

	public RingList(IEnumerable<T> collection)
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
				int newSize = Pow2RoundUp(count);
				if(newSize < count) throw new ArgumentOutOfRangeException("collection", "'collection' size is too large.");

				this.items = new T[newSize];
				this.size = count;
				c.CopyTo(items, 0);
			}
		}
		else
		{
			this.items = empty;
			foreach (T item in collection) PushBack(item);
		}
	}

	#endregion

	#region Methods

	#region Public

	public void PushFront(T item)
	{
		if (this.size == 0) this.offset = 0;

		this.offset--;
		this.offset &= this.Capacity - 1;
		this.size++;
		if (this.size > this.Capacity) this.size = this.Capacity;

		this.items[this.offset] = item;
	}

	public void PushBack(T item)
	{
		if (this.size == 0) this.offset = 0;

		if (this.size == this.Capacity)
		{
			this.offset++;
			this.offset &= this.Capacity - 1;
		}
		this.size++;
		if (this.size > this.Capacity) this.size = this.Capacity;

		this.items[(this.offset + this.size - 1) & (this.Capacity - 1)] = item;
	}

	public T PopFront()
	{
		if (this.size == 0) throw new InvalidOperationException("'RingList' is empty.");

		int index = this.offset;
		T item = this.items[index];
		this.items[index] = default(T);

		this.offset++;
		this.offset &= this.Capacity - 1;
		this.size--;

		return item;
	}

	public T PopBack()
	{
		if (this.size == 0) throw new InvalidOperationException("'RingList' is empty.");

		int index = (this.offset + this.size - 1) & (this.Capacity - 1);
		T item = this.items[index];
		this.items[index] = default(T);

		this.size--;

		return item;
	}

	public T PeekFront()
	{
		if (this.size == 0) throw new InvalidOperationException("'RingList' is empty.");

		return this.items[this.offset];
	}

	public T PeekBack()
	{
		if (this.size == 0) throw new InvalidOperationException("'RingList' is empty.");

		return this.items[(this.offset + this.size - 1) & (this.Capacity - 1)];
	}

	public ReadOnlyCollection<T> AsReadOnly()
	{
		return new ReadOnlyCollection<T>(this);
	}

	public void Clear()
	{
		if (this.size > 0)
		{
			if (IsSplit)
			{
				System.Array.Clear(this.items, this.offset, this.Capacity - this.offset);
				System.Array.Clear(this.items, 0, this.offset + this.size - this.Capacity);
			}
			else
			{
				System.Array.Clear(this.items, this.offset, this.size);
			}

			this.offset = this.size = 0;
		}
	}

	public bool Contains(T item)
	{
		if (item == null)
		{
			if (IsSplit)
			{
				for (int i = offset; i < this.Capacity; i++)
				{
					if (this.items[i] == null) return true;
				}
				for (int i = 0, end = this.offset + this.size - this.Capacity; i < end; i++)
				{
					if (this.items[i] == null) return true;
				}
			}
			else
			{
				for (int i = offset, end = this.offset + this.size; i < end; i++)
				{
					if (this.items[i] == null) return true;
				}
			}
			return false;
		}
		else
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			if (IsSplit)
			{
				for (int i = offset; i < this.Capacity; i++)
				{
					if (@default.Equals(this.items[i], item)) return true;
				}
				for (int i = 0, end = this.offset + this.size - this.Capacity; i < end; i++)
				{
					if (@default.Equals(this.items[i], item)) return true;
				}
			}
			else
			{
				for (int i = offset, end = this.offset + this.size; i < end; i++)
				{
					if (@default.Equals(this.items[i], item)) return true;
				}
			}
			return false;
		}
	}

	public void CopyTo(int index, T[] array, int arrayIndex, int count)
	{
		if (count == 0) return;

		index &= this.Capacity - 1;

		if (unchecked((uint)index >= (uint)this.size)) throw new ArgumentOutOfRangeException("index");
		if (array == null) throw new ArgumentNullException("array");
		if (unchecked((uint)arrayIndex >= (uint)array.Length)) throw new ArgumentOutOfRangeException("arrayIndex");
		if (unchecked((uint)count > (uint)(this.size - index) || (uint)count > (uint)(array.Length - arrayIndex))) throw new ArgumentOutOfRangeException("count");

		int start = ((this.offset + index) & (this.Capacity - 1));
		if (start > this.Capacity - count)
		{
			System.Array.Copy(this.items, start, array, arrayIndex, this.Capacity - start);
			System.Array.Copy(this.items, 0, array, arrayIndex + this.Capacity - start, start + count - this.Capacity);
		}
		else
		{
			System.Array.Copy(this.items, start, array, arrayIndex, count);
		}
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		this.CopyTo(0, array, arrayIndex, this.size);
	}

	public void CopyTo(T[] array)
	{
		this.CopyTo(0, array, 0, this.size);
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(this);
	}

	public int IndexOf(T item, int index, int count)
	{
		if (count == 0) return -1;

		index &= this.Capacity - 1;

		if (unchecked((uint)index >= (uint)this.size)) throw new ArgumentOutOfRangeException("index");
		if (unchecked((uint)count > (uint)(this.size - index))) throw new ArgumentOutOfRangeException("count");

		int result;

		int start = ((this.offset + index) & (this.Capacity - 1));
		if (start > this.Capacity - count)
		{
			result = System.Array.IndexOf(this.items, item, start, this.Capacity - start);
			if(result == -1) result = System.Array.IndexOf(this.items, item, 0, start + count - this.Capacity);
		}
		else
		{
			result = System.Array.IndexOf(this.items, item, start, count);
		}

		return (result != -1 ? ((result - this.offset) & (this.Capacity - 1)) : -1);
	}

	public int IndexOf(T item)
	{
		return this.IndexOf(item, 0, this.size);
	}

	public int IndexOf(T item, int index)
	{
		return this.IndexOf(item, index, this.size - index);
	}

	public int LastIndexOf(T item, int index, int count)
	{
		if (count == 0) return -1;

		index &= this.Capacity - 1;

		if (unchecked((uint)index >= (uint)this.size)) throw new ArgumentOutOfRangeException("index");
		if (unchecked((uint)count > (uint)(index + 1))) throw new ArgumentOutOfRangeException("count");

		int result;

		int start = ((this.offset + index) & (this.Capacity - 1));
		if (start < count - 1)
		{
			result = System.Array.LastIndexOf(this.items, item, start, start + 1);
			if (result == -1) result = System.Array.LastIndexOf(this.items, item, this.Capacity - 1, count - start - 1);
		}
		else
		{
			result = System.Array.LastIndexOf(this.items, item, start, count);
		}

		return (result != -1 ? ((result - offset) & (this.Capacity - 1)) : -1);
	}

	public int LastIndexOf(T item)
	{
		return this.LastIndexOf(item, this.size - 1, this.size);
	}

	public int LastIndexOf(T item, int index)
	{
		return this.LastIndexOf(item, index, index + 1);
	}

	public T[] ToArray()
	{
		T[] array = new T[this.size];
		if (IsSplit)
		{
			System.Array.Copy(this.items, this.offset, array, 0, this.Capacity - this.offset);
			System.Array.Copy(this.items, 0, array, this.Capacity - this.offset, this.offset + this.size - this.Capacity);
		}
		else
		{
			System.Array.Copy(this.items, this.offset, array, 0, this.size);
		}
		return array;
	}

	public void TrimExcess()
	{
		if (this.size <= Pow2RoundDown(this.items.Length))
		{
			this.Capacity = this.size;
		}
	}

	#endregion

	#region IList

	void IList<T>.Insert(int index, T item)
	{
		index &= this.Capacity - 1;

		if (unchecked((uint)index > (uint)this.size)) throw new ArgumentOutOfRangeException("index");

		int start = ((this.offset + index) & (this.Capacity - 1));
		if (IsSplit)
		{
			if (start >= this.offset)
			{
				ShiftFront(start - this.offset);
				start--;
			}
			else
			{
				ShiftBack(this.offset + this.size - this.Capacity - start);
			}
		}
		else
		{
			if (this.offset == 0)
			{
				ShiftBack(this.offset + this.size - start);
			}
			else if (this.offset + this.size == this.Capacity)
			{
				ShiftFront(start - this.offset);
				start--;
			}
			else
			{
				if (start - this.offset < this.offset + this.size - start)
				{
					ShiftFront(start - this.offset);
					start--;
				}
				else
				{
					ShiftBack(this.offset + this.size - start);
				}
			}
		}

		this.items[start] = item;

	}
	void IList<T>.RemoveAt(int index)
	{
		index &= this.Capacity - 1;

		if (unchecked((uint)index >= (uint)this.size)) throw new ArgumentOutOfRangeException("index");

		int start = ((this.offset + index) & (this.Capacity - 1));
		if (IsSplit)
		{
			if (start >= this.offset)
			{
				ShiftBackTo(start);
			}
			else
			{
				ShiftFrontTo(start);
			}
		}
		else
		{
			if (start - this.offset < this.offset + this.size - start)
			{
				ShiftBackTo(start);
			}
			else
			{
				ShiftFrontTo(start);
			}
		}
	}

	int IList.Add(object value)
	{
		PushBack((T)value);

		return (this.offset + this.size - 1);
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
		((IList<T>)this).Insert(index, (T)value);
	}

	void IList.Remove(object value)
	{
		if (IsCompatible(value)) ((ICollection<T>)this).Remove((T)value);
	}

	void IList.RemoveAt(int index)
	{
		((IList<T>)this).RemoveAt(index);
	}

	#endregion

	#region ICollection

	void ICollection<T>.Add(T item)
	{
		this.PushBack(item);
	}

	bool ICollection<T>.Remove(T item)
	{
		int index = this.IndexOf(item);
		if (index >= 0)
		{
			((IList<T>)this).RemoveAt(index);

			return true;
		}

		return false;
	}

	void ICollection.CopyTo(Array array, int index)
	{
		this.CopyTo((T[])array, index);
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

	private void ShiftFront(int count)
	{
		if (count != 0)
		{
			System.Array.Copy(this.items, this.offset, this.items, this.offset - 1, count);
		}

		this.offset--;
		this.size++;
		if (this.size > this.Capacity) this.size = this.Capacity;
	}

	private void ShiftBack(int count)
	{
		if (count != 0)
		{
			int start = ((this.offset + this.size - count) & (this.Capacity - 1));
			if (this.size == this.Capacity)
			{
				System.Array.Copy(this.items, start, this.items, start + 1, count - 1);
			}
			else
			{
				System.Array.Copy(this.items, start, this.items, start + 1, count);
			}
		}

		this.size++;
		if (this.size > this.Capacity) this.size = this.Capacity;
	}

	private void ShiftFrontTo(int index)
	{
		int last = ((this.offset + this.size - 1) & (this.Capacity - 1));
		if (index != last)
		{
			System.Array.Copy(this.items, index + 1, this.items, index, last - index);
		}

		this.items[last] = default(T);

		this.size--;
	}

	private void ShiftBackTo(int index)
	{
		if (index != this.offset)
		{
			System.Array.Copy(this.items, this.offset, this.items, this.offset + 1, index - this.offset);
		}

		this.items[this.offset] = default(T);

		this.offset++;
		this.offset &= this.Capacity - 1;
		this.size--;
	}

	private static int Pow2RoundUp(int x)
	{
		unchecked
		{
			if (x <= 0) return 0;

			--x;
			x |= (x >> 1);
			x |= (x >> 2);
			x |= (x >> 4);
			x |= (x >> 8);
			x |= (x >> 16);

			return (x + 1);
		}
	}

	private static int Pow2RoundDown(int x)
	{
		unchecked
		{
			if (x <= 0) return 0;

			x >>= 1;
			x |= (x >> 1);
			x |= (x >> 2);
			x |= (x >> 4);
			x |= (x >> 8);
			x |= (x >> 16);

			return (x + 1);
		}
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

		private readonly RingList<T> list;

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

		internal Enumerator(RingList<T> list)
		{
			this.list = list;
			this.index = 0;
			this.current = default(T);
		}

		#endregion

		#region Methods

		public bool MoveNext()
		{
			if (unchecked((uint)index >= (uint)list.size))
			{
				this.index = list.size + 1;
				this.current = default(T);

				return false;
			}

			this.current = list[this.index++];

			return true;
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
}