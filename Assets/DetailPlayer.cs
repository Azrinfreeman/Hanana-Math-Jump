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
    public Transform playerDelete;

    private int tempno;
    // Start is called before the first frame update
    void Start()
    {
        playerDelete = transform.parent.transform.parent.transform.parent.transform.parent.transform.GetChild(1).transform;
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
        button.onClick.RemoveAllListeners();
        button.transform.gameObject.SetActive(true);
        names.text = PlayerPrefs.GetString("Player_" + no) + " (" + PlayerPrefs.GetString("Playerid_" + no) + ")";

        tempno = no + 1;
        noText.text = tempno.ToString();
        if (PlayerPrefs.GetInt("PlayerTotal") > 1)
        {
            if (no == PlayerPrefs.GetInt("CurrentPlayerNo_"))
            {
                transform.GetChild(0).GetComponent<Image>().color = new Color32(144, 0, 255, 255);
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "DELETE";
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.yellow;
                button.onClick.AddListener(() => DisplayDelete());
            }
            else
            {
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "TUKAR";
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
                button.onClick.AddListener(() => TukarPlayer());
            }
        }
        else
        {
            if (no == PlayerPrefs.GetInt("CurrentPlayerNo_"))
            {

                button.transform.gameObject.SetActive(false);
                //button.onClick.AddListener(() => DisplayDelete());
            }
        }

    }

    public void DisplayDelete()
    {
        playerDelete.gameObject.SetActive(true);
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
