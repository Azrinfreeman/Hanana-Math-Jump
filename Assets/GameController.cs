using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    void Awake()
    {
        instance = this;
    }

    [Header("PlayerCollections")]
    public Transform moneyTransform;
    public Transform roundTransform;

    [Header("Questions")]
    bool gameStart;
    public int level;

    public TextMeshProUGUI levelText;
    public string questions;

    public int firstNum;
    public int secNum;

    public int answers;

    public bool isQuestionShowUp;

    [Header("Buttons")]
    public List<Button> ansButton;

    [Header("references")]
    public PathController pathController;

    public void buttonFunction()
    {
        HideQuestion();
    }

    public void CorrectButtonFunction()
    {
        CollectionController.instance.rounds++;
        HideQuestion();
    }

    public void ShowQuestion()
    {
        if (!isQuestionShowUp)
        {
            System.Random rnd = new System.Random();
            //if else level
            if (level == 1)
            {
                firstNum = rnd.Next(1, 10); // Generates random integer values between 1 and 10
                secNum = rnd.Next(1, 10); // Generates random integer values between 1 and 10
            }
            else if (level == 2)
            {
                firstNum = rnd.Next(10, 20); // Generates random integer values between 1 and 20
                secNum = rnd.Next(10, 20); // Generates random integer values between 1 and 20
            }
            answers = firstNum + secNum;

            //asign to question gameobject
            transform
                .GetChild(3)
                .transform.GetChild(0)
                .transform.GetChild(0)
                .GetComponent<TextMeshProUGUI>()
                .text = firstNum + " + " + secNum + " ";

            questions = firstNum + " + " + secNum + " ";

            //assign buttons in gameobject

            ansButton[0] = transform
                .GetChild(3)
                .transform.GetChild(1)
                .transform.GetChild(0)
                .transform.GetChild(0)
                .GetComponent<Button>();
            ansButton[1] = transform
                .GetChild(3)
                .transform.GetChild(1)
                .transform.GetChild(1)
                .transform.GetChild(0)
                .GetComponent<Button>();
            ansButton[2] = transform
                .GetChild(3)
                .transform.GetChild(2)
                .transform.GetChild(0)
                .transform.GetChild(0)
                .GetComponent<Button>();
            ansButton[3] = transform
                .GetChild(3)
                .transform.GetChild(2)
                .transform.GetChild(1)
                .transform.GetChild(0)
                .GetComponent<Button>();

            //if level 1 only show two answer buttons
            if (level == 1)
            {
                ansButton[2].gameObject.SetActive(false);
                ansButton[3].gameObject.SetActive(false);
            }

            if (level == 1)
            {
                //choose which button to put answer
                int buttonNum = rnd.Next(0, 1);
                ansButton[buttonNum].onClick.AddListener(() => CorrectButtonFunction());
                ansButton[buttonNum]
                    .transform.GetChild(0)
                    .transform.GetChild(0)
                    .GetComponent<TextMeshProUGUI>()
                    .text = answers.ToString();
                for (int i = 0; i < 2; i++)
                {
                    if (i != buttonNum)
                    {
                        int randomAnswer = rnd.Next(0, 20);
                        while (randomAnswer == answers)
                        {
                            randomAnswer = rnd.Next(0, 20);
                        }

                        ansButton[i].onClick.AddListener(() => buttonFunction());
                        ansButton[i]
                            .transform.GetChild(0)
                            .transform.GetChild(0)
                            .GetComponent<TextMeshProUGUI>()
                            .text = randomAnswer.ToString();
                    }
                }
            }
            else if (level == 2)
            {
                //choose which button to put answer
                int buttonNum = rnd.Next(0, 3);
                ansButton[buttonNum].onClick.AddListener(() => CorrectButtonFunction());
                ansButton[buttonNum]
                    .transform.GetChild(0)
                    .transform.GetChild(0)
                    .GetComponent<TextMeshProUGUI>()
                    .text = answers.ToString();
                for (int i = 0; i < 4; i++)
                {
                    if (i != buttonNum)
                    {
                        int randomAnswer = rnd.Next(10, 40);
                        while (randomAnswer == answers)
                        {
                            randomAnswer = rnd.Next(10, 40);
                        }

                        ansButton[i].onClick.AddListener(() => buttonFunction());
                        ansButton[i]
                            .transform.GetChild(0)
                            .transform.GetChild(0)
                            .GetComponent<TextMeshProUGUI>()
                            .text = randomAnswer.ToString();
                    }
                }
            }
            //play notidication sound
            if (!GameObject.Find("notification").GetComponent<AudioSource>().isPlaying)
            {
                GameObject.Find("notification").GetComponent<AudioSource>().Play();
            }
            //activate background and show question also off center;
            transform.GetChild(0).transform.gameObject.SetActive(true);
            transform.GetChild(2).transform.gameObject.SetActive(false);
            transform.GetChild(3).transform.gameObject.SetActive(true);

            Time.timeScale = 0;
            isQuestionShowUp = true;
        }
    }

    public void HideQuestion()
    {
        for (int i = 0; i < 4; i++)
        {
            ansButton[i].onClick.RemoveAllListeners();
        }
        //remove the current distance of obstacles
        ObstaclesController.instance.ObstaclesList.RemoveAt(0);

        //hide background and hide question also off center;
        transform.GetChild(0).transform.gameObject.SetActive(false);
        transform.GetChild(2).transform.gameObject.SetActive(false);
        transform.GetChild(3).transform.gameObject.SetActive(false);

        Time.timeScale = 1;

        isQuestionShowUp = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        moneyTransform = transform.GetChild(1).transform.GetChild(0).GetComponent<Transform>();
        roundTransform = transform.GetChild(1).transform.GetChild(1).GetComponent<Transform>();
        pathController = GameObject.Find("Player").GetComponent<PathController>();
        levelText = transform.GetChild(4).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            levelText.text = "LEVEL " + level.ToString();
        }
        if (pathController.DistanceBetween < 2f)
        {
            ShowQuestion();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        level = Int32.Parse(EventSystem.current.currentSelectedGameObject.name);
        gameStart = true;
    }
}
