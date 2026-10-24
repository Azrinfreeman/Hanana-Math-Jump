using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeToAnswer : MonoBehaviour
{
    public static TimeToAnswer instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    public float maxTime;

    TextMeshProUGUI label;
    Image fill;
    int shownSecond = -1;
    Color baseColor;

    public void CountTime()
    {
        if (maxTime > 0)
        {
            if (!label)
            {
                Transform ring = transform.GetChild(0);
                fill = ring.GetComponent<Image>();
                baseColor = fill.color;
                label = ring.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            }

            maxTime -= Time.unscaledDeltaTime;
            int second = Mathf.RoundToInt(maxTime);
            if (second != shownSecond)
            {
                shownSecond = second;
                label.text = second.ToString("00");
            }
            fill.fillAmount = maxTime / 10;
            fill.color = maxTime > 5f ? baseColor : maxTime > 3f ? UiColors.Star : UiColors.Wrong;
            // gentle pulse for the last 3 seconds
            fill.transform.localScale = Vector3.one * (maxTime <= 3f ? 1f + 0.05f * Mathf.Sin(Time.unscaledTime * 10f) : 1f);
            if (maxTime <= 0)
            {
                maxTime = 0;
                GameController.instance.OnQuestionTimeUp();
            }
        }
        else
        {
            maxTime = 0;
        }
    }

    void Update()
    {
        CountTime();
    }

    private void OnDisable()
    {
        maxTime = 10;
        shownSecond = -1;
        if (fill)
        {
            fill.color = baseColor;
            fill.transform.localScale = Vector3.one;
        }
    }
}
