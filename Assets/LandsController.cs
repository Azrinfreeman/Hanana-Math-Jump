using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandsController : MonoBehaviour
{
    public static LandsController instance;

    public List<Transform> LandsList;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
