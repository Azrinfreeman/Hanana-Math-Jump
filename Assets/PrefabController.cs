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


}
