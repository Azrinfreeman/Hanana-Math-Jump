using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailPlayer : MonoBehaviour
{
    public TransformController transformController;
    public int no;
    public TextMeshProUGUI noText;
    public TextMeshProUGUI names;
    public Button button;
    public Transform playerDelete;

    private int tempno;

    // Start is called before the first frame update
    void Start()
    {
        transformController = transform.parent.GetComponent<TransformController>();
        playerDelete = transform
            .parent.transform.parent.transform.parent.transform.parent.transform.GetChild(1)
            .transform;
        no = transform.GetSiblingIndex();
        noText = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        names = transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        button = transform.GetChild(0).transform.GetChild(2).GetComponent<Button>();

        Invoke("ApplyAgain", 0.2f);
    }

    private void OnEnable()
    {
        no = transform.GetSiblingIndex();
        noText = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        names = transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        button = transform.GetChild(0).transform.GetChild(2).GetComponent<Button>();
        //ApplyAgain();
    }

    IEnumerator applying()
    {
        yield return new WaitForSeconds(0.1f);
        button.onClick.RemoveAllListeners();
        button.transform.gameObject.SetActive(true);
        //Debug.Log());
        names.text = PlayerPrefs.GetString(
            transformController.userCollection[transform.GetSiblingIndex()]
        );

        if (PlayerPrefs.GetInt("PlayerTotal") > 1)
        {
            if (
                PlayerPrefs.GetString(
                    transformController.userCollection[transform.GetSiblingIndex()]
                ) == PlayerPrefs.GetString("CurrentPlayer_")
                && PlayerPrefs.GetString(
                    transformController.userIdCollection[transform.GetSiblingIndex()]
                ) == PlayerPrefs.GetString("CurrentPlayerid_")
            )
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
            string tempNom = transformController.userCollection[0];
            string nom = tempNom.Substring(tempNom.Length - 1);
            if (Int32.Parse(nom) == PlayerPrefs.GetInt("CurrentPlayerNo_"))
            {
                button.transform.gameObject.SetActive(false);
                //button.onClick.AddListener(() => DisplayDelete());
            }
        }
    }

    public void ApplyAgain()
    {
        StartCoroutine(applying());
    }

    public void DisplayDelete()
    {
        playerDelete.gameObject.SetActive(true);
    }

    public void TukarPlayer()
    {
        string tempNom = transformController.userCollection[transform.GetSiblingIndex()];
        string nom = tempNom.Substring(tempNom.Length - 1);
        PlayerPrefs.SetString("CurrentPlayer_", PlayerPrefs.GetString(tempNom));
        PlayerPrefs.SetInt("CurrentPlayerNo_", Int32.Parse(nom));
        PlayerPrefs.SetString(
            "CurrentPlayerid_",
            PlayerPrefs.GetString(transformController.userIdCollection[transform.GetSiblingIndex()])
        );

        CurrentPlayerName.instance.ApplyName();
        transformController.DismissPlayerSelect();
    }

    // Update is called once per frame
    void Update() { }
}
