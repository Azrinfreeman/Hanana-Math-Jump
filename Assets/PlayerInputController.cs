using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerInputController : MonoBehaviour
{
    public TMP_InputField textInput;

    public Button btnSubmit;
    // Start is called before the first frame update
    void Start()
    {
        textInput = transform.Find("content").transform.GetChild(0).GetComponent<TMP_InputField>();
        btnSubmit = GameObject.Find("BtnConfirm").GetComponent<Button>();
        if (PlayerPrefs.GetInt("FirstTime") == 1)
        {
            transform.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator submit()
    {
        DateTime theTime = DateTime.Now;
        string time = theTime.ToString("HHmmssyy");

        string fullname = textInput.text;

        GetComponent<Animator>().Play("dismiss");
        GameStartupController.instance.SaveNewPlayerName(fullname, time);
        GameStartupController.instance.SetAsCurrentPlayer(fullname, time);

        PlayerPrefs.SetInt("FirstTime", 1);
        GameStartupController.instance.ApplyScoreAgain();

        textInput.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "*Valid Name";
        textInput.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.green;

        btnSubmit.onClick.RemoveAllListeners();
        CurrentPlayerName.instance.ApplyName();
        yield return new WaitForSeconds(0);
        /*
        using (
            UnityWebRequest www = UnityWebRequest.Get(
                "https://hananaelearning.com/funmath/fetchMark.php?name=" + fullname
            )
        )
        {
            yield return www.SendWebRequest();
            if (
                www.result == UnityWebRequest.Result.ConnectionError
                || www.result == UnityWebRequest.Result.ProtocolError
            )
            {
                Debug.Log(www.error);
                Debug.Log("error server");

                textInput.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "*failed to connect to internet";
                yield return new WaitForSeconds(1f);
                GetComponent<Animator>().Play("dismiss");
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);

                if (www.downloadHandler.text.Equals("Valid"))
                {

                    GetComponent<Animator>().Play("dismiss");
                    GameStartupController.instance.SaveNewPlayerName(fullname, time);
                    GameStartupController.instance.SetAsCurrentPlayer(fullname, time);

                    PlayerPrefs.SetInt("FirstTime", 1);
                    GameStartupController.instance.ApplyScoreAgain();

                    textInput.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "*Valid Name";
                    textInput.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.green;

                }
                else
                {
                    textInput.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "*Invalid Name";
                }
            }
        }
        */
    }

    public void SubmitName()
    {
        if (!string.IsNullOrEmpty(textInput.text))
        {
            StartCoroutine(submit());
        }

    }

    public void DisableGameObject()
    {
        transform.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        btnSubmit = GameObject.Find("BtnConfirm").GetComponent<Button>();

        btnSubmit.onClick.RemoveAllListeners();
        btnSubmit.onClick.AddListener(() => SubmitName());
    }
}
