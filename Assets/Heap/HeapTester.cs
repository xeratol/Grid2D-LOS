using System.Collections.Generic;
using UnityEngine;

public class HeapTester : MonoBehaviour
{
    private struct Element<K, V> : System.IComparable where V : System.IComparable
    {
        private readonly K _key;
        private V _val;

        public Element(K key, V value)
        {
            _key = key;
            _val = value;
        }

        public int CompareTo(object obj)
        {
            var other = (Element<K, V>)obj;
            return _val.CompareTo(other._val);
        }

        public override int GetHashCode()
        {
            return _key.GetHashCode();
        }
    }

    private delegate bool TestCase();

    private void Start()
    {
        var tests = new Dictionary<string, TestCase>();
        tests["Sorting"] = TestSorting;
        tests["Resizing"] = TestResizing;
        tests["Custom Sorting"] = TestCustomSorting;
        tests["Unique"] = TestUnique;

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

        var lastVal = -101;
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

        var lastVal = -101;
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

    private int ReverseComparer(int x, int y)
    {
        return -x.CompareTo(y);
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
}
