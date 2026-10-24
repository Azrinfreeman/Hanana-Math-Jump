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

    const int MinNameLength = 2;
    const int MaxNameLength = 16;

    static string Clean(string raw)
    {
        return System.Text.RegularExpressions.Regex.Replace(raw.Trim(), @"\s+", " ");
    }

    static bool IsValid(string name)
    {
        return name.Length >= MinNameLength && name.Length <= MaxNameLength;
    }

    TextMeshProUGUI Message()
    {
        return textInput.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    char AllowedChar(string text, int index, char c)
    {
        bool ok = (char.IsLetterOrDigit(c) || c == ' ') && textInput.textComponent.font.HasCharacter(c, true);
        return ok ? c : '\0';
    }

    void OnNameChanged(string raw)
    {
        string name = Clean(raw);
        bool valid = IsValid(name);
        btnSubmit.interactable = valid;
        Message().text = raw.Length == 0 || valid ? "" : "2-16 LETTERS OR DIGITS";
        Message().color = UiColors.Wrong;
    }
    // Start is called before the first frame update
    void Start()
    {
        textInput = transform.Find("content").transform.GetChild(0).GetComponent<TMP_InputField>();
        btnSubmit = GameObject.Find("BtnConfirm").GetComponent<Button>();
        textInput.characterLimit = MaxNameLength;
        textInput.onValidateInput = AllowedChar;
        textInput.onValueChanged.AddListener(OnNameChanged);
        OnNameChanged(textInput.text);
        if (PlayerPrefs.GetInt("FirstTime") == 1)
        {
            transform.gameObject.SetActive(false);
        }
    }

    IEnumerator submit()
    {
        DateTime theTime = DateTime.Now;
        string time = theTime.ToString("HHmmssyy");

        string fullname = Clean(textInput.text);

        int slot = GameStartupController.instance.SaveNewPlayerName(fullname, time);
        if (slot < 0)
        {
            textInput.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "*Player limit reached";
            textInput.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = UiColors.Wrong;
            yield break;
        }
        GetComponent<Animator>().Play("dismiss");
        GameStartupController.instance.SetAsCurrentPlayer(fullname, time, slot);

        PlayerPrefs.SetInt("FirstTime", 1);
        GameStartupController.instance.ApplyScoreAgain();

        textInput.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "*Valid Name";
        textInput.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = UiColors.Correct;

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
                    textInput.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = UiColors.Correct;

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
        if (IsValid(Clean(textInput.text)))
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
        if (textInput)
            OnNameChanged(textInput.text);
    }
}
