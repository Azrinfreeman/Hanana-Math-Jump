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

    //public int health;

    // Start is called before the first frame update
    void Start()
    {
        textStar = transform
            .GetChild(0)
            .GetChild(0)
            .GetChild(0)
            .GetChild(0)
            .GetComponent<TextMeshProUGUI>();

        textRound = transform
            .GetChild(1)
            .GetChild(0)
            .GetChild(0)
            .GetChild(0)
            .GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textStar.text = stars.ToString();
        textRound.text = rounds.ToString();
    }


}
