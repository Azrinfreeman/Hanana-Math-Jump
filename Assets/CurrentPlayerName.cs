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
    // Start is called before the first frame update
    void Start()
    {
        textname = GetComponent<TextMeshProUGUI>();
        ApplyName();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ApplyName()
    {
        textname.text = PlayerPrefs.GetString("CurrentPlayer_");
    }
}
