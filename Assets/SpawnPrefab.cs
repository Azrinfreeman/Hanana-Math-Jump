using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    public bool isRightLine;
    public bool isLeftLine;

    [Header("References")]
    public PathController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PathController>();
    }

    // Update is called once per frame
    void Update() { }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            System.Random rnd = new System.Random();
            // - > problem dekat sini
            int randomNumberInRange = rnd.Next(0, PrefabController.instance.prefabList.Count); // Generates random integer values between 1 and how many prefabs of lands left and straight only

            // 0, 90 ,0 left // staright Quaternion.identity, Quaternion.Euler(0f, -90f, 0f) right
            //chech which lands that user standing on rn
            if (
                player.LandType.Equals("Straight")
                && player.transform.eulerAngles.y >= -10
                && player.transform.eulerAngles.y < 80
            ) // if player facing straight and on land straight
            {
                while (
                    PrefabController
                        .instance.prefabList[randomNumberInRange]
                        .ToString()
                        .Contains("Right")
                )
                {
                    Debug.Log("Random number contains right: " + randomNumberInRange);

                    randomNumberInRange = rnd.Next(0, PrefabController.instance.prefabList.Count); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                    Debug.Log("Generate new number: " + randomNumberInRange);
                }

                //spawn left lands and rotate to 90 degress angle
                if (
                    PrefabController
                        .instance.prefabList[randomNumberInRange]
                        .ToString()
                        .Contains("Left")
                )
                {
                    Debug.Log("Standing Straight land and spawn leftLand");
                    Transform t = Instantiate(
                        PrefabController.instance.prefabList[randomNumberInRange],
                        transform.parent.Find("spawnPosition").transform.position,
                        Quaternion.identity
                    );

                    LandsController.instance.LandsList.Add(t);
                }
                // if straight keep on straight
                else if (
                    PrefabController
                        .instance.prefabList[randomNumberInRange]
                        .ToString()
                        .Contains("Straight")
                )
                {
                    Debug.Log("Standing Straight land and spawn StraightLand");
                    Transform t = Instantiate(
                        PrefabController.instance.prefabList[randomNumberInRange],
                        transform.parent.Find("spawnPosition").transform.position,
                        Quaternion.identity
                    );

                    LandsController.instance.LandsList.Add(t);
                }
            }
            else if (
                player.LandType.Equals("Left")
                && player.transform.eulerAngles.y >= -10
                && player.transform.eulerAngles.y < 80
            ) // if player facing straight and on left Land
            {
                while (
                    PrefabController
                        .instance.prefabList[randomNumberInRange]
                        .ToString()
                        .Contains("Left")
                )
                {
                    Debug.Log("Random number contains Left: " + randomNumberInRange);

                    randomNumberInRange = rnd.Next(0, PrefabController.instance.prefabList.Count); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                    Debug.Log("Generate new number: " + randomNumberInRange);
                }
                //spawn Right lands and rotate to 90 degress angle
                if (
                    PrefabController
                        .instance.prefabList[randomNumberInRange]
                        .ToString()
                        .Contains("Right")
                )
                {
                    Debug.Log("RightLine");
                    Transform t = Instantiate(
                        PrefabController.instance.prefabList[randomNumberInRange],
                        transform.parent.Find("spawnPosition").transform.position,
                        Quaternion.Euler(0f, 90f, 0f)
                    );

                    LandsController.instance.LandsList.Add(t);
                }
                //spawn Straight lands and rotate to 90 degress angle
                else if (
                    PrefabController
                        .instance.prefabList[randomNumberInRange]
                        .ToString()
                        .Contains("Straight")
                )
                {
                    Debug.Log("StraightLine");
                    Transform t = Instantiate(
                        PrefabController.instance.prefabList[randomNumberInRange],
                        transform.parent.Find("spawnPosition").transform.position,
                        Quaternion.Euler(0f, 90f, 0f)
                    );

                    LandsController.instance.LandsList.Add(t);
                }
            }
            else if (
                player.LandType.Equals("Straight")
                && player.transform.eulerAngles.y >= 80
                && player.transform.eulerAngles.y < 180
            )
            {
                while (
                    PrefabController
                        .instance.prefabList[randomNumberInRange]
                        .ToString()
                        .Contains("Left")
                )
                {
                    Debug.Log("Random number contains Left: " + randomNumberInRange);

                    randomNumberInRange = rnd.Next(0, PrefabController.instance.prefabList.Count); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                    Debug.Log("Generate new number: " + randomNumberInRange);
                }
                // if player facing straight and on straight land
                if (
                    PrefabController
                        .instance.prefabList[randomNumberInRange]
                        .ToString()
                        .Contains("Straight")
                )
                {
                    Debug.Log("StraightLine");
                    Transform t = Instantiate(
                        PrefabController.instance.prefabList[randomNumberInRange],
                        transform.parent.Find("spawnPosition").transform.position,
                        Quaternion.Euler(0f, 90f, 0f)
                    );

                    LandsController.instance.LandsList.Add(t);
                }
                //spawn Right lands and rotate to 90 degress angle
                else if (
                    PrefabController
                        .instance.prefabList[randomNumberInRange]
                        .ToString()
                        .Contains("Right")
                )
                {
                    Debug.Log("RightLine");
                    Transform t = Instantiate(
                        PrefabController.instance.prefabList[randomNumberInRange],
                        transform.parent.Find("spawnPosition").transform.position,
                        Quaternion.Euler(0f, 90f, 0f)
                    );

                    LandsController.instance.LandsList.Add(t);
                }
            }
            // LEFT AND 90 degrees rotation




            else if (
                player.LandType.Equals("Right")
                && player.transform.eulerAngles.y >= 80
                && player.transform.eulerAngles.y < 180
            )
            {
                while (
                    PrefabController
                        .instance.prefabList[randomNumberInRange]
                        .ToString()
                        .Contains("Right")
                )
                {
                    Debug.Log("Random number contains Right: " + randomNumberInRange);

                    randomNumberInRange = rnd.Next(0, PrefabController.instance.prefabList.Count); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                    Debug.Log("Generate new number: " + randomNumberInRange);
                }
                // if player facing straight and on straight land
                if (
                    PrefabController
                        .instance.prefabList[randomNumberInRange]
                        .ToString()
                        .Contains("Straight")
                )
                {
                    Debug.Log("StraightLine");
                    Transform t = Instantiate(
                        PrefabController.instance.prefabList[randomNumberInRange],
                        transform.parent.Find("spawnPosition").transform.position,
                        Quaternion.identity
                    );

                    LandsController.instance.LandsList.Add(t);
                }
                //spawn left lands and rotate to 90 degress angle
                else if (
                    PrefabController
                        .instance.prefabList[randomNumberInRange]
                        .ToString()
                        .Contains("Left")
                )
                {
                    Debug.Log("LeftLine");
                    Transform t = Instantiate(
                        PrefabController.instance.prefabList[randomNumberInRange],
                        transform.parent.Find("spawnPosition").transform.position,
                        Quaternion.identity
                    );

                    LandsController.instance.LandsList.Add(t);
                }
            }
            else
            {
                Debug.Log("The problem is here: y = " + player.transform.eulerAngles.y);
            }
            /*
            else if (player.LandType.Equals("Left"))
            {
                if (
                    PrefabController
                        .instance.prefabList[randomNumberInRange]
                        .ToString()
                        .Contains("Straight")
                )
                {
                    Debug.Log("StraightLine");
                    Transform t = Instantiate(
                        PrefabController.instance.prefabList[randomNumberInRange],
                        transform.parent.Find("spawnPosition").transform.position,
                        Quaternion.Euler(0f, 90f, 0f)
                    );

                    LandsController.instance.LandsList.Add(t);
                }
                else if (
                    PrefabController
                        .instance.prefabList[randomNumberInRange]
                        .ToString()
                        .Contains("Left")
                )
                {
                    Debug.Log("LeftLine");
                    Transform t = Instantiate(
                        PrefabController.instance.prefabList[randomNumberInRange],
                        transform.parent.Find("spawnPosition").transform.position,
                        Quaternion.Euler(0f, 90f, 0f)
                    );

                    LandsController.instance.LandsList.Add(t);
                }
            }
            */
            /*
                        if (isRightLine)
                        {
                            Transform t = Instantiate(
                                PrefabController.instance.prefabList[randomNumberInRange],
                                transform.parent.Find("spawnPosition").transform.position,
                                Quaternion.Euler(0f, -90f, 0f)
                            );
            
                            LandsController.instance.LandsList.Add(t);
                        }
                        */
        }
    }

    // Somewhat better code...
}
