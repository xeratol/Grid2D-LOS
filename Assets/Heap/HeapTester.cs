using System.Collections.Generic;
using UnityEngine;

public class HeapTester : MonoBehaviour
{
    const int NUM_REPEAT_TEST = 1000000;

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
        public V Val;

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

        public override bool Equals(object obj)
        {
            var other = (KeyValElement<K, V>)obj;
            return _key.Equals(other._key);
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
        tests.Add("Find Existing", TestFind);
        tests.Add("Find Non-Existent", TestFindNonExistent);
        tests.Add("Sorting", TestSorting);
        tests.Add("Resizing", TestResizing);
        tests.Add("Custom Sorting", TestCustomSorting);
        tests.Add("Custom Type", TestCustomType);
        tests.Add("Key Value Type", TestKeyValueType);
        tests.Add("Key Value Type Decrease", TestKeyValueTypeDecrease);
        tests.Add("Key Value Type Increase", TestKeyValueTypeIncrease);
        tests.Add("Key Value Type Update", TestKeyValueTypeUpdate);

        foreach (var test in tests)
        {
            var success = true;

            var i = 0;
            for (; i < NUM_REPEAT_TEST; ++i)
            {
                if (!test.Value())
                {
                    success = false;
                    break;
                }
            }

            Debug.LogFormat("{0} Test: {1}",
                test.Key,
                success ? "<color=green>Success!</color>" : string.Format("<color=red>Failed at {0}!</color>", i));
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

    private bool TestFind()
    {
        var testHeap = new Heap<int>(5);
        var values = new List<int>(5);

        for (var i = 0; i < 5; ++i)
        {
            var val = (i * 10) + Random.Range(0, 9);
            testHeap.Push(val);
            values.Add(val);
        }

        foreach (var val in values)
        {
            if (testHeap.Find(val) != val)
            {
                return false;
            }
        }

        return true;
    }

    private bool TestFindNonExistent()
    {
        var testHeap = new Heap<int>(5);

        for (var i = 0; i < 5; ++i)
        {
            var val = (i * 10) + Random.Range(0, 9);
            testHeap.Push(val);
        }

        try
        {
            testHeap.Find(-1);
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

            testHeap.Push(newElement);
        }

        var lastElement = new KeyValElement<int, float>(0, -1.0f);
        while (!testHeap.IsEmpty)
        {
            var element = testHeap.Pop();
            if (lastElement.Equals(element) &&
                lastElement.CompareTo(element) >= 0)
            {
                return false;
            }
            lastElement = element;
        }

        return true;
    }

    private bool TestKeyValueTypeDecrease()
    {
        var testHeap = new Heap<KeyValElement<int, float>>();

        for (var i = 0; i < 5; ++i)
        {
            var val = Random.Range(11.0f, 20.0f);
            var newElement = new KeyValElement<int, float>(i, val);

            testHeap.Push(newElement);
        }

        for (var i = 0; i < 5; ++i)
        {
            var val = Random.Range(0.0f, 10.0f);
            var newElement = new KeyValElement<int, float>(i, val);

            testHeap.DecreaseKey(newElement);
        }

        var lastElement = new KeyValElement<int, float>(0, -1.0f);
        while (!testHeap.IsEmpty)
        {
            var element = testHeap.Pop();
            if (lastElement.Equals(element) && 
                lastElement.CompareTo(element) >= 0)
            {
                return false;
            }
            lastElement = element;
        }

        return true;
    }

    private bool TestKeyValueTypeIncrease()
    {
        var testHeap = new Heap<KeyValElement<int, float>>();

        for (var i = 0; i < 5; ++i)
        {
            var val = Random.Range(0.0f, 10.0f);
            var newElement = new KeyValElement<int, float>(i, val);

            testHeap.Push(newElement);
        }

        for (var i = 0; i < 5; ++i)
        {
            var val = Random.Range(11.0f, 20.0f);
            var newElement = new KeyValElement<int, float>(i, val);

            testHeap.IncreaseKey(newElement);
        }

        var lastElement = new KeyValElement<int, float>(0, -1.0f);
        while (!testHeap.IsEmpty)
        {
            var element = testHeap.Pop();
            if (lastElement.Equals(element) && 
                lastElement.CompareTo(element) >= 0)
            {
                return false;
            }
            lastElement = element;
        }

        return true;
    }

    private bool TestKeyValueTypeUpdate()
    {
        var testHeap = new Heap<KeyValElement<int, float>>();

        for (var i = 0; i < 5; ++i)
        {
            var val = Random.Range(0.0f, 10.0f);
            var newElement = new KeyValElement<int, float>(i, val);

            testHeap.Push(newElement);
        }

        for (var i = 0; i < 5; ++i)
        {
            var newElement = new KeyValElement<int, float>(i, 0);
            var oldElement = testHeap.Find(newElement);
            newElement.Val = oldElement.Val;

            while (oldElement.Val.CompareTo(newElement.Val) == 0)
            {
                var val = Random.Range(-100.0f, 100.0f);
                newElement.Val = val;
            }

            testHeap.UpdateKey(newElement);
        }

        var lastElement = new KeyValElement<int, float>(0, -200.0f);
        while (!testHeap.IsEmpty)
        {
            var element = testHeap.Pop();
            if (lastElement.Equals(element) && 
                lastElement.CompareTo(element) >= 0)
            {
                return false;
            }
            lastElement = element;
        }

        return true;
    }
}

