using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BatchMonoBehaviour<T> : MonoBehaviour where T : BatchMonoBehaviour<T>
{
	#region Fields

	private T prev, next;

	private static T first, last;

	#endregion

	#region Properties

	public T Previous
	{
		get { return prev; }
	}

	public T Next
	{
		get { return next; }
	}

	public static T First
	{
		get { return first; }
	}

	public static T Last
	{
		get { return last; }
	}

	#endregion

	#region Messages

	protected void OnEnable()
	{
		T @this = this as T;
		if (@this != null)
		{
			Add(@this);
		}
		else
		{
			Debug.LogWarning("Unexpected type parameter.");
		}
	}

	protected void OnDisable()
	{
		T @this = this as T;
		if (@this != null)
		{
			Remove(@this);
		}
		else
		{
			Debug.LogWarning("Unexpected type parameter.");
		}
	}

	#endregion

	#region Methods

	protected virtual void Process() { }

	public static void Invoke()
	{
		for (T node = first; node != null; node = node.next)
		{
			node.Process();
		}
	}

	public static void ForEach(Action<T> action)
	{
		if (action == null) throw new ArgumentNullException("action");

		for (T node = first; node != null; node = node.next)
		{
			action(node);
		}
	}

	public static Enumerator GetEnumerator()
	{
		return new Enumerator();
	}

	private static void Add(T node)
	{
		if (first == null && last == null)
		{
			first = last = node;
		}
		else
		{
			node.prev = last;
			last.next = node;

			last = node;
		}
	}

	private static void Remove(T node)
	{
		if (first == node)
		{
			if (last == node)
			{
				first = last = null;
			}
			else
			{
				first = node.next;

				node.next = null;
				first.prev = null;
			}
		}
		else if (last == node)
		{
			last = node.prev;

			node.prev = null;
			last.next = null;
		}
		else
		{
			node.prev.next = node.next;
			node.next.prev = node.prev;

			node.prev = node.next = null;
		}
	}

	#endregion

	#region Enumerator

	public struct Enumerator : IEnumerator<T>
	{
		#region Fields

		private T current;

		private bool initialized;

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

		#region Methods

		public bool MoveNext()
		{
			if (initialized)
			{
				current = current.next;
			}
			else
			{
				current = first;
				initialized = true;
			}

			return (current != null);
		}

		public void Reset()
		{
			current = null;
			initialized = false;
		}

		public void Dispose() { }

		#endregion
	}

	#endregion
}