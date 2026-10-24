using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPath : MonoBehaviour
{
    public string landType;

    void Awake() { }

    public void AddNewPath()
    {
        Transform paths = transform.Find("Paths");
        for (int i = 0; i < paths.childCount; i++)
        {
            PathwayController.instance.Points.Add(paths.GetChild(i));
        }
    }

    public void AddObstacle()
    {
        Transform obstacles = transform.Find("Obstacles");
        for (int i = 0; i < obstacles.childCount; i++)
        {
            ObstaclesController.instance.ObstaclesList.Add(obstacles.GetChild(i));
        }

        ObstaclesController.instance.TriggerJumpList.Add(
            transform.Find("triggerEdge").GetComponent<Transform>().transform
        );
    }

    // Start is called before the first frame update
    void Start()
    {
        AddNewPath();
        AddObstacle();
    }

}
