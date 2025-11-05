using System;
using Unity.Collections;

namespace Omniverse
{
	public interface IHeapItem<T> : IComparable<T>
	{
		int Id { get; set; }
		int HeapIndex { get; set; }
	}

	public struct Heap<T> : IDisposable where T : struct, IHeapItem<T>
	{
		public NativeArray<T> Items;
		public NativeHashMap<int, int> heapIndices;

		public int Count;

		public Heap(int capacity)
		{
			Items = new NativeArray<T>(capacity, Allocator.Persistent);
			heapIndices = new NativeHashMap<int, int>(capacity, Allocator.Persistent);

			Count = 0;
		}

		public void Dispose()
		{
			Items.Dispose();
			heapIndices.Dispose();
		}

		public void Clear()
		{
			Count = 0;
			heapIndices.Clear();
		}

		public void Add(T item)
		{
			UpdateHeapIndex(item, Count);
			SortUp(Items[Count]);
			Count++;
		}

		public void UpdateHeapIndex(T item, int index)
		{
			item.HeapIndex = index;
			Items[index] = item;

			if (heapIndices.ContainsKey(item.Id))
			{
				heapIndices[item.Id] = index;
			}
			else
			{
				heapIndices.Add(item.Id, index);
			}
		}

		public bool Contains(int id)
		{
			return heapIndices.ContainsKey(id);
			//return Equals(Items[item.HeapIndex], item);
		}

		public T Get(int id)
		{
			return Items[heapIndices[id]];
			//return Equals(Items[item.HeapIndex], item);
		}

		public T RemoveFirst()
		{
			T item = Items[0];
			Count--;
			T lastItem = Items[Count];
			UpdateHeapIndex(lastItem, 0);
			SortDown(Items[0]);
			return item;
		}

		public void SortUp(T item)
		{
			int parentIndex = (item.HeapIndex - 1) / 2;

			while (true)
			{
				T parentItem = Items[parentIndex];
				if (item.CompareTo(parentItem) > 0)
				{
					item = Swap(item, parentItem);
				}
				else
				{
					break;
				}

				parentIndex = (item.HeapIndex - 1) / 2;
			}
		}

		public void SortDown(T item)
		{
			while (true)
			{
				int childIndexLeft = item.HeapIndex * 2 + 1;
				int childIndexRight = item.HeapIndex * 2 + 2;

				int swapIndex = 0;

				if (childIndexLeft < Count)
				{
					swapIndex = childIndexLeft;

					if (childIndexRight < Count)
					{
						if (Items[childIndexLeft].CompareTo(Items[childIndexRight]) < 0)
						{
							swapIndex = childIndexRight;
						}
					}

					if (item.CompareTo(Items[swapIndex]) < 0)
					{
						item = Swap(item, Items[swapIndex]);
					}
					else
					{
						return;
					}
				}
				else
				{
					return;
				}
			}
		}

		private T Swap(T a, T b)
		{
			UpdateHeapIndex(a, b.HeapIndex);
			UpdateHeapIndex(b, a.HeapIndex);

			return Items[b.HeapIndex];
		}
	}
}
