using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEndBehavior : MonoBehaviour
{
    [SerializeField]
    private GridController gridController = null;

    [SerializeField]
    private int index = 0;

    void Start()
    {
        Debug.Assert(gridController, "Grid Controller not set");
    }

    void Update()
    {
        if (transform.hasChanged)
        {
            gridController.SetPOI(transform.position, index);
            transform.hasChanged = false;
        }
    }
}
