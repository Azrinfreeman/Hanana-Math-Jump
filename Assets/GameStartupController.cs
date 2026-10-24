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
                UnityWebRequest www = UnityWebRequest.Post(ServerApi.InsertMark, form)
            )
            {
                www.timeout = ServerApi.TimeoutSeconds;
                yield return www.SendWebRequest();
                if (www.result != UnityWebRequest.Result.Success)
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

    public const int MaxPlayers = 20;

    // Stores the player in the first free slot and returns the slot index (-1 when full).
    public int SaveNewPlayerName(string name, string id)
    {
        for (int a = 0; a < MaxPlayers; a++)
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("Player_" + a)))
            {
                PlayerPrefs.SetString("Player_" + a, name);
                PlayerPrefs.SetString("Playerid_" + a, id);
                PlayerPrefs.SetInt("PlayerTotal", PlayerPrefs.GetInt("PlayerTotal") + 1);
                return a;
            }
        }
        return -1;
    }

    // "Player_12" -> 12
    public static int SlotFromKey(string key)
    {
        int.TryParse(key.Substring(key.LastIndexOf('_') + 1), out int slot);
        return slot;
    }

    public string returnPlayerName()
    {
        return PlayerPrefs.GetString("Player_");
    }

    public void SetAsCurrentPlayer(string name, string id, int slot)
    {
        PlayerPrefs.SetString("CurrentPlayer_", name);
        PlayerPrefs.SetString("CurrentPlayerid_", id);
        PlayerPrefs.SetInt("CurrentPlayerNo_", slot);
    }
}
