using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPath : MonoBehaviour
{
    public string landType;

    void Awake() { }

    public void AddNewPath()
    {
        for (int i = 0; i < GameObject.Find("Paths").GetComponent<Transform>().childCount; i++)
        {
            //Debug.Log(GameObject.Find("Paths").GetComponent<Transform>().GetChild(i).transform);
            PathwayController.instance.Points.Add(
                transform.Find("Paths").GetComponent<Transform>().GetChild(i).transform
            );
        }
    }

    public void AddObstacle()
    {
        for (int i = 0; i < GameObject.Find("Obstacles").GetComponent<Transform>().childCount; i++)
        {
            //Debug.Log(GameObject.Find("Obstacles").GetComponent<Transform>().GetChild(i).transform);
            ObstaclesController.instance.ObstaclesList.Add(
                transform.Find("Obstacles").GetComponent<Transform>().GetChild(i).transform
            );
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

    // Update is called once per frame
    void Update() { }
}
