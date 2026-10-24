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

    // Other scripts (shop, reset, delete) write PlayerPrefs directly, so re-read a few times a second.
    const float RefreshInterval = 0.25f;
    float nextRefresh;
    int shownStars = -1;
    int shownRounds = -1;

    static string StarsKey => "StarsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_");
    static string RoundsKey => "RoundsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_");

    void Start()
    {
        textStar = GameObject.Find("textStar").GetComponent<TextMeshProUGUI>();
        textRound = GameObject.Find("textRound").GetComponent<TextMeshProUGUI>();
        Refresh();
    }

    public void addStars(int star)
    {
        stars = PlayerPrefs.GetInt(StarsKey) + star;
        PlayerPrefs.SetInt(StarsKey, stars);
        Refresh();
    }

    public void addRounds(int round)
    {
        roundsTotal = PlayerPrefs.GetInt(RoundsKey) + round;
        PlayerPrefs.SetInt(RoundsKey, roundsTotal);
        Refresh();
    }

    void Update()
    {
        if (Time.unscaledTime >= nextRefresh)
        {
            nextRefresh = Time.unscaledTime + RefreshInterval;
            Refresh();
        }
    }

    void Refresh()
    {
        stars = PlayerPrefs.GetInt(StarsKey);
        roundsTotal = PlayerPrefs.GetInt(RoundsKey);
        if (stars != shownStars)
        {
            shownStars = stars;
            textStar.text = stars.ToString();
        }
        if (roundsTotal != shownRounds)
        {
            shownRounds = roundsTotal;
            textRound.text = roundsTotal.ToString();
        }
    }
}
