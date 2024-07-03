using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesController : MonoBehaviour
{
    public static ObstaclesController instance;

    public List<Transform> ObstaclesList;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
