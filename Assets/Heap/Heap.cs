// Modified from https://gist.github.com/roufamatic/ee7e11469809f2b276c0d3dc6b8dd80b

using System;
using System.Collections.Generic;

/// <summary>
/// List-based Heap implementation. Does not allow duplicate elements.
/// </summary>
/// <typeparam name="T">Kind of thing being stored in the heap.</typeparam>
public class Heap<T> where T : IComparable
{
    private List<T> _heap = null;
    private Dictionary<T, int> _indexMap;
    private Comparison<T> _comparer;

    private int DefaultComparer(T x, T y)
    {
        return x.CompareTo(y);
    }

    /// <summary>
    /// Create a new heap.
    /// </summary>
    /// <param name="capacity">The minimum number of elements the heap is expected to hold.</param>
    public Heap(int capacity = 0 )
    {
        _heap = new List<T>(capacity);
        _indexMap = new Dictionary<T, int>();
        _comparer = new Comparison<T>(DefaultComparer);
    }

    /// <summary>
    /// Create a new heap.
    /// </summary>
    /// <param name="comparer">Custom Comparison delegate.</param>
    /// <param name="capacity">The minimum number of elements the heap is expected to hold.</param>
    public Heap(Comparison<T> comparer, int capacity = 0)
    {
        _heap = new List<T>(capacity);
        _indexMap = new Dictionary<T, int>();
        _comparer = comparer;
    }

    /// <summary>
    /// Current size of the Heap.
    /// </summary>
    public int Count { get { return _heap.Count; } }

    /// <summary>
    /// Test to see if the Heap is empty.
    /// </summary>
    public bool IsEmpty { get { return _heap.Count == 0; } }

    /// <summary>
    /// Add a new value to the Heap.
    /// </summary>
    /// <param name="val">The key to be inserted. Sh</param>
    public void Push(T val)
    {
        if (Exists(val))
        {
            throw new ArgumentException("Heap does not allow duplicate values");
        }

        _indexMap[val] = Count;
        _heap.Add(val);
        ShiftUp(_indexMap[val]);
    }

    /// <summary>
    /// View the value currently at the top of the Heap.
    /// </summary>
    /// <returns></returns>
    public T Peek()
    {
        return _heap[0];
    }

    /// <summary>
    /// Remove the value currently at the top of the Heap and return it.
    /// </summary>
    /// <returns></returns>
    public T Pop()
    {
        T output = Peek();

        _heap[0] = _heap[Count - 1];
        _indexMap[_heap[0]] = 0;

        _heap.RemoveAt(Count - 1);
        _indexMap.Remove(output);

        ShiftDown(0);
        return output;
    }

    /// <summary>
    /// Checks if the key exists.
    /// </summary>
    /// <param name="val">The key to test</param>
    /// <returns>True if the key already exists. False otherwise</returns>
    public bool Exists(T val)
    {
        return _indexMap.ContainsKey(val);
    }

    public T Find(T val)
    {
        if (!Exists(val))
        {
            throw new ArgumentException("Key does not exist");
        }

        return _heap[_indexMap[val]];
    }

    /// <summary>
    /// Update the value of the key. Used custom data types with key-value pairs.
    /// </summary>
    /// <param name="val">The new value of the key</param>
    public void UpdateKey(T val)
    {
        if (!Exists(val))
        {
            throw new ArgumentException("Key does not exist");
        }
        var index = _indexMap[val];

        var compare = _comparer(_heap[index], val);
        if (compare < 0)
        {
            _heap[index] = val;
            ShiftDown(index);
        }
        else if (compare > 0)
        {
            _heap[index] = val;
            ShiftUp(index);
        }
        else
        {
            throw new ArgumentException("Key did not change");
        }
    }

    /// <summary>
    /// Increase the value of the key. Used custom data types with key-value pairs.
    /// </summary>
    /// <param name="val">The new value of the key</param>
    public void IncreaseKey(T val)
    {
        if (!Exists(val))
        {
            throw new ArgumentException("Key does not exist");
        }
        var index = _indexMap[val];

        if (_comparer(_heap[index], val) >= 0)
        {
            throw new ArgumentException("Key is expected to increase");
        }

        _heap[index] = val;
        ShiftDown(index);
    }

    /// <summary>
    /// Decrease the value of the key. Used custom data types with key-value pairs.
    /// </summary>
    /// <param name="val">The new value of the key</param>
    public void DecreaseKey(T val)
    {
        if (!Exists(val))
        {
            throw new ArgumentException("Key does not exist");
        }
        var index = _indexMap[val];

        if (_comparer(_heap[index], val) <= 0)
        {
            throw new ArgumentException("Key is expected to decrease");
        }

        _heap[index] = val;
        ShiftUp(index);
    }

    /// <summary>
    /// Clears the data.
    /// </summary>
    public void Clear()
    {
        _heap.Clear();
        _indexMap.Clear();
    }

    /// <summary>
    /// Move an element up the Heap.
    /// </summary>
    /// <param name="heapIndex"></param>
    private void ShiftUp(int heapIndex)
    {
        if (heapIndex == 0)
        {
            return;
        }
        var parentIndex = (heapIndex - 1) / 2;
        if (_comparer(_heap[parentIndex], _heap[heapIndex]) > 0)
        {
            Swap(parentIndex, heapIndex);
            ShiftUp(parentIndex);
        }
    }

    /// <summary>
    /// Move an element down the Heap.
    /// </summary>
    /// <param name="heapIndex"></param>
    private void ShiftDown(int heapIndex)
    {
        var child1 = heapIndex * 2 + 1;
        var child2 = child1 + 1;
        var preferredChildIndex = child1;

        if (child1 >= Count)
        {
            return;
        }

        if (child2 < Count && _comparer(_heap[child1], _heap[child2]) > 0)
        {
            preferredChildIndex = child2;
        }

        if (_comparer(_heap[preferredChildIndex], _heap[heapIndex]) > 0)
        {
            return;
        }

        Swap(heapIndex, preferredChildIndex);
        ShiftDown(preferredChildIndex);
    }

    /// <summary>
    /// Swap two items in the underlying list.
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    private void Swap(int index1, int index2)
    {
        T temp = _heap[index1];
        _heap[index1] = _heap[index2];
        _heap[index2] = temp;

        _indexMap[_heap[index2]] = index2;
        _indexMap[_heap[index1]] = index1;
    }
}