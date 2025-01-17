using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentPlayerName : MonoBehaviour
{
    public static CurrentPlayerName instance;
    private void Awake()
    {
        instance = this;
    }
    public TextMeshProUGUI textname;
    public TextMeshProUGUI textId;
    // Start is called before the first frame update
    void Start()
    {
        textname = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        textId = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        ApplyName();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ApplyName()
    {
        textname.text = PlayerPrefs.GetString("CurrentPlayer_");
        //textId.text = PlayerPrefs.GetString("CurrentPlayerid_");
    }
}
