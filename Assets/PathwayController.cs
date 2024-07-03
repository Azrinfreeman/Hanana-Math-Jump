using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathwayController : MonoBehaviour
{
    public static PathwayController instance;

    void Awake()
    {
        instance = this;
    }

    [Header("PlayerPathWay")]
    [SerializeField]
    public List<Transform> Points;

    [Header("startIndex")]
    [SerializeField]
    public int pointIndex;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
