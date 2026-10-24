using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesController : MonoBehaviour
{
    public static ObstaclesController instance;

    public List<Transform> ObstaclesList;

    public List<Transform> TriggerJumpList;

    void Awake()
    {
        instance = this;
    }


}
