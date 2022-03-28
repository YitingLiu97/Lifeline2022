using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public static GraphManager Instance;

    public Transform graphPosition, graphStart, graphEnd, graphTop, graphBottom, zTop, zBottom;
    public GameObject xAxis;
    public GameObject yAxis;
    public int intensityScale = 21;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

}
