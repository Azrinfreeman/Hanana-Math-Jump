using System;
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

    public TransformController transformController;
    public TextMeshProUGUI text;
    public Button btnConfirm;

    public void ResetData()
    {
        StartCoroutine(reset());
    }

    IEnumerator reset()
    {
        //server down
        Debug.Log("deletPlayer");

        using (
            UnityWebRequest www = UnityWebRequest.Get(
                "https://app-hanana.com/funmath/fetchDelete.php?name="
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
                string str = www.downloadHandler.text.Trim();
                if (str.Equals("Player Deleted"))
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
                    for (int a = 0; a < transformController.userCollection.Count; a++)
                    {
                        if (
                            PlayerPrefs.GetString(transformController.userCollection[a])
                            == PlayerPrefs.GetString(
                                "Player_" + PlayerPrefs.GetInt("CurrentPlayerNo_")
                            )
                        )
                        {
                            transformController.userCollection.RemoveAt(a);
                            a = 100;
                            Debug.Log("user count : " + transformController.userCollection.Count);
                        }
                    }
                    //yield return new WaitForSeconds(0.5f);
                    PlayerPrefs.DeleteKey(
                        "RoundsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_")
                    );
                    PlayerPrefs.DeleteKey(
                        "StarsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_")
                    );
                    PlayerPrefs.DeleteKey("Player_" + PlayerPrefs.GetInt("CurrentPlayerNo_"));
                    PlayerPrefs.DeleteKey("Playerid_" + PlayerPrefs.GetInt("CurrentPlayerNo_"));
                    PlayerPrefs.SetInt("PlayerTotal", PlayerPrefs.GetInt("PlayerTotal") - 1);
                    PlayerPrefs.SetString(
                        "CurrentPlayer_",
                        PlayerPrefs.GetString(transformController.userCollection[0])
                    );
                    string tempNom = transformController.userCollection[0];
                    string nom = tempNom.Substring(tempNom.Length - 1);
                    PlayerPrefs.SetInt("CurrentPlayerNo_", Int32.Parse(nom));
                    CurrentPlayerName.instance.ApplyName();
                    ///
                    GetComponent<Animator>()
                        .Play("dismiss");
                    GameObject
                        .Find("PlayerSelect")
                        .transform.GetChild(0)
                        .transform.GetChild(1)
                        .transform.GetChild(0)
                        .GetComponent<TransformController>()
                        .ClearChildrenAndDismiss();
                    transform.gameObject.SetActive(false);
                    yield return new WaitForSeconds(1.1f);

                    GameObject.Find("PlayerSelect").transform.gameObject.SetActive(false);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transformController = transform
            .parent.GetChild(0)
            .transform.GetChild(1)
            .transform.GetChild(0)
            .GetComponent<TransformController>();
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
