using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandling : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.gameStart)
        {
            transform.GetComponent<Button>().interactable = false;
        }
        else
        {
            transform.GetComponent<Button>().interactable = true;
        }
    }
}
