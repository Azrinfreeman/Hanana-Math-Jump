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

    Slider slider;
    TextMeshProUGUI label;
    int shownSecond = -1;

    void Start()
    {
        pathController = GameObject.Find("Player").GetComponent<PathController>();
        slider = GetComponent<Slider>();
        label = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (GameController.instance.gameStart && pathController.moveSpeed > 0f)
        {
            timeFull = pathController.OriginalDistanceBetween / pathController.moveSpeed;
            slider.maxValue = timeFull * 100;
            timeRemaining = pathController.DistanceBetween / pathController.moveSpeed;
            slider.value = timeRemaining * 100;

            int second = Mathf.RoundToInt(timeRemaining);
            if (second != shownSecond)
            {
                shownSecond = second;
                label.text = second.ToString("00");
            }
        }
    }
}
