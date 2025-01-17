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
            LandsController.instance.RemoveLand();
            System.Random rnd = new System.Random();
            // - > problem dekat sini
            int randomNumberInRange = rnd.Next(0, PrefabController.instance.prefabList.Count); // Generates random integer values between 1 and how many prefabs of lands left and straight only
            //            Debug.Log("random num : " + randomNumberInRange);
            if (GameController.instance.enterLevel[0])
            { // 0, 90 ,0 left // staright Quaternion.identity, Quaternion.Euler(0f, -90f, 0f) right
                //chech which lands that user standing on rn
                if (
                    player.LandType.Equals("Straight")
                    && player.transform.eulerAngles.y >= -10
                    && player.transform.eulerAngles.y < 80
                ) // if player facing straight and on land straight
                {
                    //then generate Left or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Right")
                        || PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Blue")
                        || PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Yellow")
                        || PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Pink")
                        || PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Cyan")
                    )
                    {
                        //                        Debug.Log("Random number contains right: " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        //                        Debug.Log("Generate new number: " + randomNumberInRange);
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
                    //then generate right or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Left")
                        || PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Blue")
                        || PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Yellow")
                        || PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Pink")
                        || PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Cyan")
                    )
                    {
                        Debug.Log("Random number contains Left: " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
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
                //90 degress and straight land
                else if (
                    player.LandType.Equals("Straight")
                    && player.transform.eulerAngles.y >= 80
                    && player.transform.eulerAngles.y < 180
                )
                {
                    //then generate right or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Left")
                        || PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Blue")
                        || PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Yellow")
                        || PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Pink")
                        || PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Cyan")
                    )
                    {
                        Debug.Log("Random number contains Left: " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
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
                    //then generate left or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Right")
                        || PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Blue")
                        || PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Yellow")
                        || PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Pink")
                        || PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Cyan")
                    )
                    {
                        Debug.Log("Random number contains Right: " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
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

                ///
            }
            /// blue pathway
            ///
            /// //
            /// //
            ///
            ///
            /// /// ///
            else if (GameController.instance.enterLevel[1])
            { // 0, 90 ,0 left // staright Quaternion.identity, Quaternion.Euler(0f, -90f, 0f) right
                //chech which lands that user standing on rn
                if (
                    player.LandType.Contains("Straight")
                    && player.transform.eulerAngles.y >= -10
                    && player.transform.eulerAngles.y < 80
                ) // if player facing straight and on land straight
                {
                    Debug.Log("Straight");
                    //then generate Left or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Right")
                            && PrefabController
                                .instance.prefabList[randomNumberInRange]
                                .ToString()
                                .Contains("RightBlue")
                        || !PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Blue")
                    )
                    {
                        Debug.Log("Current random number index : " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        Debug.Log("Generate new number: " + randomNumberInRange);
                    }

                    //spawn left lands and rotate to 90 degress angle
                    if (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("LeftBlue")
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
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Blue")
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
                    player.LandType.Contains("Left")
                    && player.transform.eulerAngles.y >= -10
                    && player.transform.eulerAngles.y < 80
                ) // if player facing straight and on left Land
                {
                    Debug.Log("Left");
                    //then generate right or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Left")
                            && PrefabController
                                .instance.prefabList[randomNumberInRange]
                                .ToString()
                                .Contains("LeftBlue")
                        || !PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Blue")
                    )
                    {
                        Debug.Log("Random number contains Left: " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        Debug.Log("Generate new number: " + randomNumberInRange);
                    }
                    //spawn Right lands and rotate to 90 degress angle
                    if (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Right")
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Blue")
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
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Blue")
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
                //90 degress and straight land
                else if (
                    player.LandType.Contains("Straight")
                    && player.transform.eulerAngles.y >= 80
                    && player.transform.eulerAngles.y < 180
                )
                {
                    Debug.Log("Straight 80");
                    //then generate right or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Left")
                            && PrefabController
                                .instance.prefabList[randomNumberInRange]
                                .ToString()
                                .Contains("LeftBlue")
                        || !PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Blue")
                    )
                    {
                        Debug.Log("Random number contains Left: " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        Debug.Log("Generate new number: " + randomNumberInRange);
                    }
                    // if player facing straight and on straight land
                    if (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Straight")
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Blue")
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
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Blue")
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
                    player.LandType.Contains("Right")
                    && player.transform.eulerAngles.y >= 80
                    && player.transform.eulerAngles.y < 180
                )
                {
                    Debug.Log("Right 80");
                    //then generate left or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Right")
                            && PrefabController
                                .instance.prefabList[randomNumberInRange]
                                .ToString()
                                .Contains("RightBlue")
                        || !PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Blue")
                    )
                    {
                        Debug.Log("Random number contains Right: " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        Debug.Log("Generate new number: " + randomNumberInRange);
                    }
                    // if player facing straight and on straight land
                    if (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Straight")
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Blue")
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
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Blue")
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

                ///
            }
            /// <summary>
            /// Yellow Pathwaty
            /// </summary>
            /// <returns></returns>
            else if (GameController.instance.enterLevel[2])
            { // 0, 90 ,0 left // staright Quaternion.identity, Quaternion.Euler(0f, -90f, 0f) right
                //chech which lands that user standing on rn
                if (
                    player.LandType.Contains("Straight")
                    && player.transform.eulerAngles.y >= -10
                    && player.transform.eulerAngles.y < 80
                ) // if player facing straight and on land straight
                {
                    Debug.Log("Straight");
                    //then generate Left or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Right")
                            && PrefabController
                                .instance.prefabList[randomNumberInRange]
                                .ToString()
                                .Contains("RightYellow")
                        || !PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Yellow")
                    )
                    {
                        Debug.Log("Current random number index : " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        Debug.Log("Generate new number: " + randomNumberInRange);
                    }

                    //spawn left lands and rotate to 90 degress angle
                    if (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("LeftYellow")
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
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Yellow")
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
                    player.LandType.Contains("Left")
                    && player.transform.eulerAngles.y >= -10
                    && player.transform.eulerAngles.y < 80
                ) // if player facing straight and on left Land
                {
                    Debug.Log("Left");
                    //then generate right or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Left")
                            && PrefabController
                                .instance.prefabList[randomNumberInRange]
                                .ToString()
                                .Contains("LeftYellow")
                        || !PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Yellow")
                    )
                    {
                        Debug.Log("Random number contains Left: " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        Debug.Log("Generate new number: " + randomNumberInRange);
                    }
                    //spawn Right lands and rotate to 90 degress angle
                    if (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Right")
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Yellow")
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
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Yellow")
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
                //90 degress and straight land
                else if (
                    player.LandType.Contains("Straight")
                    && player.transform.eulerAngles.y >= 80
                    && player.transform.eulerAngles.y < 180
                )
                {
                    Debug.Log("Straight 80");
                    //then generate right or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Left")
                            && PrefabController
                                .instance.prefabList[randomNumberInRange]
                                .ToString()
                                .Contains("LeftYellow")
                        || !PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Yellow")
                    )
                    {
                        Debug.Log("Random number contains Left: " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        Debug.Log("Generate new number: " + randomNumberInRange);
                    }
                    // if player facing straight and on straight land
                    if (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Straight")
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Yellow")
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
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Yellow")
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
                    player.LandType.Contains("Right")
                    && player.transform.eulerAngles.y >= 80
                    && player.transform.eulerAngles.y < 180
                )
                {
                    Debug.Log("Right 80");
                    //then generate left or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Right")
                            && PrefabController
                                .instance.prefabList[randomNumberInRange]
                                .ToString()
                                .Contains("RightYellow")
                        || !PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Yellow")
                    )
                    {
                        Debug.Log("Random number contains Right: " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        Debug.Log("Generate new number: " + randomNumberInRange);
                    }
                    // if player facing straight and on straight land
                    if (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Straight")
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Yellow")
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
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Yellow")
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

                ///
            } //
            ///
            //
            ///

            ///
            else if (GameController.instance.enterLevel[3])
            { // 0, 90 ,0 left // staright Quaternion.identity, Quaternion.Euler(0f, -90f, 0f) right
                //chech which lands that user standing on rn
                if (
                    player.LandType.Contains("Straight")
                    && player.transform.eulerAngles.y >= -10
                    && player.transform.eulerAngles.y < 80
                ) // if player facing straight and on land straight
                {
                    Debug.Log("Straight");
                    //then generate Left or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Right")
                            && PrefabController
                                .instance.prefabList[randomNumberInRange]
                                .ToString()
                                .Contains("RightPink")
                        || !PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Pink")
                    )
                    {
                        Debug.Log("Current random number index : " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        Debug.Log("Generate new number: " + randomNumberInRange);
                    }

                    //spawn left lands and rotate to 90 degress angle
                    if (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("LeftPink")
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
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Pink")
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
                    player.LandType.Contains("Left")
                    && player.transform.eulerAngles.y >= -10
                    && player.transform.eulerAngles.y < 80
                ) // if player facing straight and on left Land
                {
                    Debug.Log("Left");
                    //then generate right or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Left")
                            && PrefabController
                                .instance.prefabList[randomNumberInRange]
                                .ToString()
                                .Contains("LeftPink")
                        || !PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Pink")
                    )
                    {
                        Debug.Log("Random number contains Left: " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        Debug.Log("Generate new number: " + randomNumberInRange);
                    }
                    //spawn Right lands and rotate to 90 degress angle
                    if (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Right")
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Pink")
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
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Pink")
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
                //90 degress and straight land
                else if (
                    player.LandType.Contains("Straight")
                    && player.transform.eulerAngles.y >= 80
                    && player.transform.eulerAngles.y < 180
                )
                {
                    Debug.Log("Straight 80");
                    //then generate right or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Left")
                            && PrefabController
                                .instance.prefabList[randomNumberInRange]
                                .ToString()
                                .Contains("LeftPink")
                        || !PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Pink")
                    )
                    {
                        Debug.Log("Random number contains Left: " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        Debug.Log("Generate new number: " + randomNumberInRange);
                    }
                    // if player facing straight and on straight land
                    if (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Straight")
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Pink")
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
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Pink")
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
                    player.LandType.Contains("Right")
                    && player.transform.eulerAngles.y >= 80
                    && player.transform.eulerAngles.y < 180
                )
                {
                    Debug.Log("Right 80");
                    //then generate left or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Right")
                            && PrefabController
                                .instance.prefabList[randomNumberInRange]
                                .ToString()
                                .Contains("RightPink")
                        || !PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Pink")
                    )
                    {
                        Debug.Log("Random number contains Right: " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        Debug.Log("Generate new number: " + randomNumberInRange);
                    }
                    // if player facing straight and on straight land
                    if (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Straight")
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Pink")
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
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Pink")
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

                ///// Cyan is here hehe
                ///
            }
            else if (GameController.instance.enterLevel[4])
            {
                // 0, 90 ,0 left // staright Quaternion.identity, Quaternion.Euler(0f, -90f, 0f) right
                //chech which lands that user standing on rn
                if (
                    player.LandType.Contains("Straight")
                    && player.transform.eulerAngles.y >= -10
                    && player.transform.eulerAngles.y < 80
                ) // if player facing straight and on land straight
                {
                    Debug.Log("Straight");
                    //then generate Left or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Right")
                            && PrefabController
                                .instance.prefabList[randomNumberInRange]
                                .ToString()
                                .Contains("RightCyan")
                        || !PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Cyan")
                    )
                    {
                        Debug.Log("Current random number index : " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        Debug.Log("Generate new number: " + randomNumberInRange);
                    }

                    //spawn left lands and rotate to 90 degress angle
                    if (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("LeftCyan")
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
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Cyan")
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
                    player.LandType.Contains("Left")
                    && player.transform.eulerAngles.y >= -10
                    && player.transform.eulerAngles.y < 80
                ) // if player facing straight and on left Land
                {
                    Debug.Log("Left");
                    //then generate right or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Left")
                            && PrefabController
                                .instance.prefabList[randomNumberInRange]
                                .ToString()
                                .Contains("LeftCyan")
                        || !PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Cyan")
                    )
                    {
                        Debug.Log("Random number contains Left: " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        Debug.Log("Generate new number: " + randomNumberInRange);
                    }
                    //spawn Right lands and rotate to 90 degress angle
                    if (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Right")
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Cyan")
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
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Cyan")
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
                //90 degress and straight land
                else if (
                    player.LandType.Contains("Straight")
                    && player.transform.eulerAngles.y >= 80
                    && player.transform.eulerAngles.y < 180
                )
                {
                    Debug.Log("Straight 80");
                    //then generate right or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Left")
                            && PrefabController
                                .instance.prefabList[randomNumberInRange]
                                .ToString()
                                .Contains("LeftCyan")
                        || !PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Cyan")
                    )
                    {
                        Debug.Log("Random number contains Left: " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        Debug.Log("Generate new number: " + randomNumberInRange);
                    }
                    // if player facing straight and on straight land
                    if (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Straight")
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Cyan")
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
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Cyan")
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
                    player.LandType.Contains("Right")
                    && player.transform.eulerAngles.y >= 80
                    && player.transform.eulerAngles.y < 180
                )
                {
                    Debug.Log("Right 80");
                    //then generate left or straight land
                    while (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Right")
                            && PrefabController
                                .instance.prefabList[randomNumberInRange]
                                .ToString()
                                .Contains("RightCyan")
                        || !PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Cyan")
                    )
                    {
                        Debug.Log("Random number contains Right: " + randomNumberInRange);

                        randomNumberInRange = rnd.Next(
                            0,
                            PrefabController.instance.prefabList.Count
                        ); // Generates random integer values between 1 and how many prefabs of lands left and straight only
                        Debug.Log("Generate new number: " + randomNumberInRange);
                    }
                    // if player facing straight and on straight land
                    if (
                        PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Straight")
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Cyan")
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
                        && PrefabController
                            .instance.prefabList[randomNumberInRange]
                            .ToString()
                            .Contains("Cyan")
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
                    Debug.Log("The problem is here: y = " + player.transform.eulerAngles.y + " asdasd " + player.LandType);
                }

                ///
            }

        }
    }

    // Somewhat better code...
}
