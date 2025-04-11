using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResetController : MonoBehaviour
{
    public void DisableGameObject()
    {
        transform.gameObject.SetActive(false);
    }

    public TextMeshProUGUI text;
    public Button btnConfirm;

    public void ResetData()
    {
        PlayerPrefs.SetInt("RoundsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"), 0);

        PlayerPrefs.SetInt("StarsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"), 0);

        GetComponent<Animator>().Play("dismiss");
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
        text.text = "RESET DATA FOR " + PlayerPrefs.GetString("CurrentPlayer_").ToString();
        btnConfirm.onClick.AddListener(ResetData);
    }

    // Update is called once per frame
    void Update() { }
}
