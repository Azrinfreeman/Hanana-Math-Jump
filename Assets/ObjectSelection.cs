using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelection : MonoBehaviour
{
    public Transform g1;
    public Transform g2;

    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ToDoAction()
    {
        StartCoroutine(start());
    }


    IEnumerator start()
    {
        anim.SetTrigger("onClick");
        yield return new WaitForSeconds(0.34f);
        if (g1)
        {
            g1.gameObject.SetActive(false);
        }
        if (g2)
        {

            g2.gameObject.SetActive(true);

        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
