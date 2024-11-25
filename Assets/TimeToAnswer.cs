using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

    // Start is called before the first frame update
    void Start()
    {

    }

    public void CountTime()
    {
        if (maxTime > 0)
        {

            maxTime -= Time.unscaledDeltaTime;
            transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = maxTime.ToString("00");
            transform.GetChild(0).GetComponent<Image>().fillAmount = maxTime / 10;

        }
        else
        {
            maxTime = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        CountTime();
    }

    private void OnDisable()
    {
        maxTime = 10;
    }
}
