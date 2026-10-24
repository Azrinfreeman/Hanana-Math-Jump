using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectTrigger : MonoBehaviour
{


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

        Sfx.PlayIfIdle("collected");
        yield return new WaitForSeconds(0.25f);
        GameController.instance.moneyTransform.GetComponent<Animator>().Play("afterCollected");
        Destroy(gameObject);
    }
}
