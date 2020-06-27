using System.Collections.Generic;
using UnityEngine;

public class HeapTester : MonoBehaviour
{
    private struct SingleElement<K> : System.IComparable where K : System.IComparable
    {
        public K Key { get; private set; }

        public SingleElement(K key)
        {
            Key = key;
        }

        public int CompareTo(object obj)
        {
            var other = (SingleElement<K>)obj;
            return Key.CompareTo(other.Key);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }

    private struct KeyValElement<K, V> : System.IComparable where V : System.IComparable
    {
        private readonly K _key;
        public V Val { get; private set; }

        public KeyValElement(K key, V value)
        {
            _key = key;
            Val = value;
        }

        public int CompareTo(object obj)
        {
            var other = (KeyValElement<K, V>)obj;
            return Val.CompareTo(other.Val);
        }

        public override int GetHashCode()
        {
            return _key.GetHashCode();
        }
    }

    private int ReverseComparer<T>(T x, T y) where T : System.IComparable
    {
        return -x.CompareTo(y);
    }

    private delegate bool TestCase();

    private void Start()
    {
        var tests = new Dictionary<string, TestCase>();
        tests.Add("Unique", TestUnique);
        tests.Add("Sorting", TestSorting);
        tests.Add("Resizing", TestResizing);
        tests.Add("Custom Sorting", TestCustomSorting);
        tests.Add("Custom Type", TestCustomType);
        tests.Add("Key Value Type", TestKeyValueType);

        foreach (var test in tests)
        {
            Debug.LogFormat("{0} Test: {1}",
                test.Key,
                test.Value() ? "<color=green>Success!</color>" : "<color=red>Failed!</color>");
        }
    }

    private bool TestSorting()
    {
        var testHeap = new Heap<int>(5);

        for (var i = 0; i < 5; ++i)
        {
            var val = Random.Range(0, 100);

            while (testHeap.Exists(val))
            {
                val = Random.Range(0, 100);
            }

            testHeap.Push(val);
        }

        var lastVal = -1;
        while (!testHeap.IsEmpty)
        {
            var val = testHeap.Pop();
            if (lastVal >= val)
            {
                return false;
            }
            lastVal = val;
        }

        return true;
    }

    private bool TestResizing()
    {
        var testHeap = new Heap<int>(2);

        for (var i = 0; i < 10; ++i)
        {
            var val = Random.Range(0, 100);

            while (testHeap.Exists(val))
            {
                val = Random.Range(0, 100);
            }

            testHeap.Push(val);
        }

        var lastVal = -1;
        while (!testHeap.IsEmpty)
        {
            var val = testHeap.Pop();
            if (lastVal >= val)
            {
                return false;
            }
            lastVal = val;
        }

        return true;
    }

    private bool TestCustomSorting()
    {
        var testHeap = new Heap<int>(new System.Comparison<int>(ReverseComparer), 2);

        for (var i = 0; i < 10; ++i)
        {
            var val = Random.Range(0, 100);

            while (testHeap.Exists(val))
            {
                val = Random.Range(0, 100);
            }

            testHeap.Push(val);
        }

        var lastVal = 101;
        while (!testHeap.IsEmpty)
        {
            var val = testHeap.Pop();
            if (lastVal <= val)
            {
                return false;
            }
            lastVal = val;
        }

        return true;
    }

    private bool TestUnique()
    {
        var testHeap = new Heap<int>(5);

        var val = Random.Range(0, 100);
        testHeap.Push(val);

        try
        {
            testHeap.Push(val);
        }
        catch (System.Exception)
        {
            return true;
        }

        return false;
    }

    private bool TestCustomType()
    {
        var testHeap = new Heap<SingleElement<int>>();

        for (var i = 0; i < 5; ++i)
        {
            var val = Random.Range(0, 100);
            var newElement = new SingleElement<int>(val);

            while (testHeap.Exists(newElement))
            {
                val = Random.Range(0, 100);
                newElement = new SingleElement<int>(val);
            }

            testHeap.Push(newElement);
        }

        var lastVal = new SingleElement<int>(-1);
        while (!testHeap.IsEmpty)
        {
            var element = testHeap.Pop();
            if (lastVal.CompareTo(element) >= 0)
            {
                return false;
            }
            lastVal = element;
        }

        return true;
    }

    private bool TestKeyValueType()
    {
        var testHeap = new Heap<KeyValElement<int, float>>();

        for (var i = 0; i < 5; ++i)
        {
            var val = Random.Range(0.0f, 100.0f);
            var newElement = new KeyValElement<int, float>(i, val);

            while (testHeap.Exists(newElement))
            {
                val = Random.Range(0, 100);
                newElement = new KeyValElement<int, float>(i, val);
            }

            testHeap.Push(newElement);
        }

        var lastVal = new KeyValElement<int, float>(0, -1.0f);
        while (!testHeap.IsEmpty)
        {
            var element = testHeap.Pop();
            if (lastVal.CompareTo(element) >= 0)
            {
                return false;
            }
            lastVal = element;
        }

        return true;
    }
}
