using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class GameStartupController : MonoBehaviour
{
    public static GameStartupController instance;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Invoke("UnlockAll", 1.2f);
        ApplyScoreAgain();
    }

    public void UnlockAll()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("MaxCharacters"); i++)
        {
            PlayerPrefs.SetInt("Character_No_" + i, 1);
        }
    }

    // Update is called once per frame
    void Update() { }

    IEnumerator InsertScoreToDB()
    {
        string str = PlayerPrefs.GetString("CurrentPlayer_");
        if (!string.IsNullOrEmpty(str))
        {
            WWWForm form = new WWWForm();

            if (PlayerPrefs.GetInt("RoundsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_")) < 0)
            {
                PlayerPrefs.SetInt("RoundsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"), 0);
            }

            form.AddField("PlayerName", PlayerPrefs.GetString("CurrentPlayer_"));
            form.AddField("Playerid", PlayerPrefs.GetString("CurrentPlayerid_"));
            form.AddField(
                "totalStars",
                PlayerPrefs.GetInt("StarsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"))
            );
            form.AddField(
                "totalRounds",
                PlayerPrefs.GetInt("RoundsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"))
            );

            using (
                UnityWebRequest www = UnityWebRequest.Post(
                    "https://app-hanana.com/funmath/insertmark.php",
                    form
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
                    // Debug.Log("error server");
                }
                else
                {
                    string retrieveData = www.downloadHandler.text.Trim();
                    if (retrieveData.Equals("Insert Successful"))
                    {
                        Debug.Log(retrieveData);
                        Debug.Log("First time user");
                    }
                    else if (retrieveData.Equals("Update Successful"))
                    {
                        Debug.Log(retrieveData);
                        Debug.Log("Existing user");
                    }
                    else
                    {
                        Debug.Log("Something else");
                        Debug.Log(retrieveData);
                    }
                }
                www.Dispose();
            }
        }
    }

    public void ApplyScoreAgain()
    {
        StartCoroutine(InsertScoreToDB());
    }

    public void SaveNewPlayerName(string name, string id)
    {
        if (PlayerPrefs.GetInt("PlayerTotal") == 0)
        {
            PlayerPrefs.SetString("Player_0", name);
            PlayerPrefs.SetString("Playerid_0", id);
            PlayerPrefs.SetInt("PlayerTotal", PlayerPrefs.GetInt("PlayerTotal") + 1);
        }
        else if (PlayerPrefs.GetInt("PlayerTotal") > 0)
        {
            string str = PlayerPrefs.GetString("Player_" + PlayerPrefs.GetInt("PlayerTotal"));

            if (string.IsNullOrEmpty(str) == true)
            {
                Debug.Log("empty");
                PlayerPrefs.SetString("Player_" + PlayerPrefs.GetInt("PlayerTotal"), name);
                PlayerPrefs.SetString("Playerid_" + PlayerPrefs.GetInt("PlayerTotal"), id);
                PlayerPrefs.SetInt("PlayerTotal", PlayerPrefs.GetInt("PlayerTotal") + 1);
            }
            else
            {
                int newPlayerTotal = PlayerPrefs.GetInt("PlayerTotal") + 1;
                string str2 = PlayerPrefs.GetString("Player_" + 0);
                int a = 0;
                while (a < 100)
                {
                    str2 = PlayerPrefs.GetString("Player_" + a);
                    if (string.IsNullOrEmpty(str2))
                    {
                        Debug.Log("existing player");
                        PlayerPrefs.SetString("Player_" + a, name);
                        PlayerPrefs.SetString("Playerid_" + a, id);
                        PlayerPrefs.SetInt("PlayerTotal", PlayerPrefs.GetInt("PlayerTotal") + 1);
                        a = 101;
                    }
                    a++;
                }

            }
        }
    }

    public string returnPlayerName()
    {
        return PlayerPrefs.GetString("Player_");
    }

    public void SetAsCurrentPlayer(string name, string id)
    {
        PlayerPrefs.SetString("CurrentPlayer_", name);
        PlayerPrefs.SetString("CurrentPlayerid_", id);
        PlayerPrefs.SetInt("CurrentPlayerNo_", PlayerPrefs.GetInt("PlayerTotal") - 1);
    }
}
