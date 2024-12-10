using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailPlayer : MonoBehaviour
{
    public int no;
    public TextMeshProUGUI noText;
    public TextMeshProUGUI names;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {

        no = transform.GetSiblingIndex();
        noText = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        names = transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        button = transform.GetChild(0).transform.GetChild(2).GetComponent<Button>();


        ApplyAgain();
    }

    private void OnEnable()
    {
        no = transform.GetSiblingIndex();
        noText = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        names = transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        button = transform.GetChild(0).transform.GetChild(2).GetComponent<Button>();
        ApplyAgain();
    }

    public void ApplyAgain()
    {
        names.text = PlayerPrefs.GetString("Player_" + no);
        button.onClick.RemoveAllListeners();
        if (no == PlayerPrefs.GetInt("CurrentPlayerNo_"))
        {
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";

        }
        else
        {
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "TUKAR";
            button.onClick.AddListener(() => TukarPlayer());
        }
    }

    public void TukarPlayer()
    {
        PlayerPrefs.SetString("CurrentPlayer_", PlayerPrefs.GetString("Player_" + no));
        PlayerPrefs.SetInt("CurrentPlayerNo_", no);
        CurrentPlayerName.instance.ApplyName();
        TransformController.instance.DismissPlayerSelect();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
