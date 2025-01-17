using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleEye : MonoBehaviour
{
    public List<Transform> Center;

    public bool turnoff;
    // Start is called before the first frame update
    void Start()
    {
        turnoff = true;
    }

    public void Toggle()
    {
        if (!turnoff)
        {
            for (int i = 0; i < Center.Count; i++)
            {
                Center[i].gameObject.SetActive(true);
            }
            turnoff = true;
        }
        else if (turnoff)
        {
            for (int i = 0; i < Center.Count; i++)
            {
                Center[i].gameObject.SetActive(false);
            }
            turnoff = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
