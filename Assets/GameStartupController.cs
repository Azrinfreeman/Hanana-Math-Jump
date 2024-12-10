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
        ApplyScoreAgain();
    }

    // Update is called once per frame
    void Update()
    {


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
            form.AddField("totalMark", PlayerPrefs.GetInt("RoundsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_")));

            using (
                UnityWebRequest www = UnityWebRequest.Post(
                    "https://hananaelearning.com/funmath/insertmark.php",
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
                    Debug.Log("error server");
                }
                else
                {
                    if (www.downloadHandler.text.Equals("Insert Successful"))
                    {
                        Debug.Log("First time user");
                    }
                    else if (www.downloadHandler.text.Equals("Update Successful"))
                    {
                        Debug.Log("Existing user");
                    }
                    else
                    {
                        Debug.Log("Something else");
                    }
                }
            }
        }


    }

    public void ApplyScoreAgain()
    {

        StartCoroutine(InsertScoreToDB());

    }


    public void SaveNewPlayerName(string name)
    {
        if (PlayerPrefs.GetInt("PlayerTotal") == 0)
        {

            PlayerPrefs.SetString("Player_0", name);
            PlayerPrefs.SetInt("PlayerTotal", PlayerPrefs.GetInt("PlayerTotal") + 1);
        }
        else
        {
            PlayerPrefs.SetString("Player_" + PlayerPrefs.GetInt("PlayerTotal"), name);
            PlayerPrefs.SetInt("PlayerTotal", PlayerPrefs.GetInt("PlayerTotal") + 1);

        }
    }

    public string returnPlayerName()
    {
        return PlayerPrefs.GetString("Player_");
    }

    public void SetAsCurrentPlayer(string name)
    {
        PlayerPrefs.SetString("CurrentPlayer_", name);
        PlayerPrefs.SetInt("CurrentPlayerNo_", PlayerPrefs.GetInt("PlayerTotal") - 1);
    }


}
