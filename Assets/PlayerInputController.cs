using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public TMP_InputField textInput;
    // Start is called before the first frame update
    void Start()
    {
        textInput = transform.Find("content").transform.GetChild(0).GetComponent<TMP_InputField>();

        if (PlayerPrefs.GetInt("FirstTime") == 1)
        {
            transform.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SubmitName()
    {
        if (!textInput.Equals(""))
        {
            GetComponent<Animator>().Play("dismiss");
            GameStartupController.instance.SaveNewPlayerName(textInput.text);
            GameStartupController.instance.SetAsCurrentPlayer(textInput.text);

            PlayerPrefs.SetInt("FirstTime", 1);
            GameStartupController.instance.ApplyScoreAgain();
        }

    }

    public void DisableGameObject()
    {
        transform.gameObject.SetActive(false);
    }
}
