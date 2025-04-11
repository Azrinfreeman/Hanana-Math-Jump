using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerDeleteController : MonoBehaviour
{
    public void DisableGameObject()
    {
        transform.gameObject.SetActive(false);
    }

    public TextMeshProUGUI text;
    public Button btnConfirm;

    public void ResetData()
    {
        StartCoroutine(reset());
    }

    IEnumerator reset()
    {
        PlayerPrefs.DeleteKey("RoundsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"));
        PlayerPrefs.DeleteKey("StarsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"));
        PlayerPrefs.DeleteKey("Player_" + PlayerPrefs.GetInt("CurrentPlayerNo_"));
        PlayerPrefs.DeleteKey("Playerid_" + PlayerPrefs.GetInt("CurrentPlayerNo_"));
        PlayerPrefs.SetInt("PlayerTotal", PlayerPrefs.GetInt("PlayerTotal") - 1);

        int i = 0;
        int l = 0;
        while (i <= 1000)
        {
            if (!PlayerPrefs.GetString("Player_" + l).IsUnityNull())
            {
                Debug.Log(PlayerPrefs.GetString("Player_" + l));
                PlayerPrefs.SetInt("CurrentPlayerNo_", l);
                PlayerPrefs.SetString("CurrentPlayer_", PlayerPrefs.GetString("Player_" + l));
                i = 1000;
            }
            else
            {
                Debug.Log(PlayerPrefs.GetString("Player_" + l));
                l++;
            }

            i++;
        }
        GameObject.Find("Content").GetComponent<TransformController>().ClearChildrenAndDismiss();

        GetComponent<Animator>().Play("dismiss");

        //server down
        Debug.Log("deletPlayer");
        using (
            UnityWebRequest www = UnityWebRequest.Get(
                "https://hananaelearning.com/funmath/fetchDelete.php?name="
                    + PlayerPrefs.GetString("CurrentPlayer_")
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
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                if (www.downloadHandler.text.Equals("Player Deleted"))
                {
                    /*
                    PlayerPrefs.DeleteKey(
                        "RoundsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_")
                    );
                    PlayerPrefs.DeleteKey(
                        "StarsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_")
                    );
                    PlayerPrefs.DeleteKey("Player_" + PlayerPrefs.GetInt("CurrentPlayerNo_"));
                    PlayerPrefs.SetInt("PlayerTotal", PlayerPrefs.GetInt("PlayerTotal") - 1);

                    int i = 0;
                    int l = 0;
                    while (i <= 1000)
                    {
                        if (!PlayerPrefs.GetString("Player_" + l).IsUnityNull())
                        {
                            Debug.Log(PlayerPrefs.GetString("Player_" + l));
                            PlayerPrefs.SetInt("CurrentPlayerNo_", l);
                            PlayerPrefs.SetString(
                                "CurrentPlayer_",
                                PlayerPrefs.GetString("Player_" + l)
                            );
                            i = 1000;
                        }
                        else
                        {
                            Debug.Log(PlayerPrefs.GetString("Player_" + l));
                            l++;
                        }

                        i++;
                    }

                    GetComponent<Animator>().Play("dismiss");
                    */
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        btnConfirm = transform.GetChild(1).transform.GetChild(1).GetComponent<Button>();
    }

    private void OnEnable()
    {
        Invoke("PlayerDetails", 0.2f);
    }

    void PlayerDetails()
    {
        text.text = "DELETE ACCOUNT: " + PlayerPrefs.GetString("CurrentPlayer_").ToString();
        btnConfirm.onClick.AddListener(ResetData);
    }

    // Update is called once per frame
    void Update() { }
}
