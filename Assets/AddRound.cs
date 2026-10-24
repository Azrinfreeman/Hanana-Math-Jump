using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRound : MonoBehaviour
{


    void OnTriggerEnter(Collider other)
    {
        CollectionController.instance.rounds++;
    }
}
