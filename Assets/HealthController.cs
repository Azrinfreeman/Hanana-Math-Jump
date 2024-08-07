using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public static HealthController instance;

    void Awake()
    {
        instance = this;
    }

    public int healthCount;
    public List<Transform> Healths;

    // Start is called before the first frame update
    void Start()
    {
        healthCount = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            healthCount++;
            Healths.Add(transform.GetChild(i).GetChild(0));
        }
    }

    public void getHurt()
    {
        Healths[healthCount - 1].gameObject.SetActive(false);
        healthCount--;
    }

    // Update is called once per frame
    void Update() { }
}
