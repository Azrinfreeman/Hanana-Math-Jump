using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public void DisableObject()
    {
        transform.gameObject.SetActive(false);
    }

}
