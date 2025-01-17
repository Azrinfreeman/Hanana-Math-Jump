using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionController : MonoBehaviour
{
    public static CollectionController instance;

    void Awake()
    {
        instance = this;
    }

    public int stars;
    public TextMeshProUGUI textStar;
    public int rounds;
    public TextMeshProUGUI textRound;

    public int roundsTotal;
    public int starsTotal;
    //public int health;

    // Start is called before the first frame update
    void Start()
    {
        stars = PlayerPrefs.GetInt("StarsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"));
        roundsTotal = PlayerPrefs.GetInt("RoundsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"));
        textStar = GameObject.Find("textStar")
            .GetComponent<TextMeshProUGUI>();

        textRound = GameObject.Find("textRound")
            .GetComponent<TextMeshProUGUI>();
    }


    public void addStars(int star)
    {
        stars += star;
        PlayerPrefs.SetInt("StarsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"), stars);
    }

    public void addRounds(int round)
    {
        roundsTotal += round;
        PlayerPrefs.SetInt("RoundsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"), roundsTotal);
    }

    // Update is called once per frame
    void Update()
    {
        stars = PlayerPrefs.GetInt("StarsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"));
        roundsTotal = PlayerPrefs.GetInt("RoundsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"));
        textStar.text = stars.ToString();
        textRound.text = roundsTotal.ToString();
    }


}
