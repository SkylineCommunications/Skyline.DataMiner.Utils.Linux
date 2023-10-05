namespace Skyline.DataMiner.Utils.Linux.Communication
{
	using System;

	internal class FixedBuffer<T>
	{
		private readonly T[] items;
		private T[] _values = null;
		private int pointer = 0;

		public FixedBuffer(int capacity = 50)
		{
			items = new T[capacity];
			Count = 0;
		}

		public int Count { get; private set; }

		public T[] Values
		{
			get
			{
				if (_values == null)
				{
					_values = GetValues();
				}

				return _values;
			}
		}

		public void Add(T item)
		{
			items[pointer] = item;
			_values = null;
			if (pointer >= items.Length - 1)
			{
				pointer = 0;
			}
			else
			{
				pointer++;
			}

			if (Count < items.Length)
			{
				Count++;
			}
		}

		private T[] GetValues()
		{
			if (Count == 0)
			{
				return new T[0];
			}

			var values = new T[Count];
			if (Count < items.Length)
			{
				Array.Copy(items, values, Count);
			}
			else if (pointer == 0)
			{
				values = items;
			}
			else
			{
				Array.Copy(items, pointer, values, 0, items.Length - pointer);
				Array.Copy(items, 0, values, items.Length - pointer, pointer);
			}

			return values;
		}
	}
}