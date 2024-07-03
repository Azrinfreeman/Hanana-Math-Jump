using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabController : MonoBehaviour
{
    public static PrefabController instance;

    void Awake()
    {
        instance = this;
    }

    public List<Transform> prefabList;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
