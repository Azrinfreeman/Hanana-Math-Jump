using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    public bool isRightLine;
    public bool isLeftLine;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            System.Random rnd = new System.Random();
            int randomNumberInRange = rnd.Next(0, PrefabController.instance.prefabList.Count); // Generates random integer values between 1 and 100

            if (isRightLine)
            {
                Transform t = Instantiate(
                    PrefabController.instance.prefabList[randomNumberInRange],
                    transform.parent.Find("spawnPosition").transform.position,
                    Quaternion.Euler(0f, -90f, 0f)
                );

                LandsController.instance.LandsList.Add(t);
            }
            else if (isLeftLine)
            {
                Transform t = Instantiate(
                    PrefabController.instance.prefabList[randomNumberInRange],
                    transform.parent.Find("spawnPosition").transform.position,
                    Quaternion.Euler(0f, 90f, 0f)
                );

                LandsController.instance.LandsList.Add(t);
            }
            else
            {
                Transform t = Instantiate(
                    PrefabController.instance.prefabList[randomNumberInRange],
                    transform.parent.Find("spawnPosition").transform.position,
                    Quaternion.identity
                );

                LandsController.instance.LandsList.Add(t);
            }
        }
    }

    // Somewhat better code...
}
