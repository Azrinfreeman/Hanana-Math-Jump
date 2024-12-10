using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            StartCoroutine(triggerMoney());
        }
    }

    IEnumerator triggerMoney()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        GameController.instance.moneyTransform.GetComponent<Animator>().Play("collected");

        yield return new WaitForSeconds(0.45f);
        CollectionController.instance.addStars(1);

        if (!GameObject.Find("collected").GetComponent<AudioSource>().isPlaying)
        {
            GameObject.Find("collected").GetComponent<AudioSource>().Play();
        }
        yield return new WaitForSeconds(0.25f);
        GameController.instance.moneyTransform.GetComponent<Animator>().Play("afterCollected");
        Destroy(gameObject);
    }
}
