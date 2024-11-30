using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public static CharacterSelection instance;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
    public List<Transform> characters;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            characters.Add(transform.GetChild(i).transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
