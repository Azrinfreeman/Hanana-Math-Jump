using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeRemainingController : MonoBehaviour
{
    public static TimeRemainingController instance;

    void Awake()
    {
        instance = this;
    }

    public PathController pathController;
    public float timeRemaining;
    private float timeFull;

    // Start is called before the first frame update
    void Start()
    {
        pathController = GameObject.Find("Player").GetComponent<PathController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.gameStart)
        {
            timeFull = pathController.OriginalDistanceBetween / pathController.moveSpeed;
            GetComponent<Slider>().maxValue = timeFull * 100;
            timeRemaining = pathController.DistanceBetween / pathController.moveSpeed;

            //        Debug.Log("TimeRemaining: " + timeRemaining);
            GetComponent<Slider>().value = timeRemaining * 100;
            transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = timeRemaining.ToString(
                "00"
            );

            //Debug.Log(timeFull + ": TimeFull");
        }
    }
}
