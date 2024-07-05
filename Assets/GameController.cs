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
    public int questionCount;

    public TextMeshProUGUI levelText;
    public string questions;

    public int firstNum;
    public int secNum;

    public int answers;

    public int randomAnswer;

    public int operation;

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
            System.Random rnd2 = new System.Random();
            operation = rnd2.Next(2);
            //if else level
            if (level == 1)
            {
                //questionCount based question

                if (questionCount >= 0 && questionCount <= 20)
                { //answer not more than 10
                    firstNum = rnd.Next(1, 5); // Generates random integer values between 1 and 5
                    secNum = rnd.Next(1, 5); // Generates random integer values between 1 and 5
                }
                else if (questionCount > 20 && questionCount <= 40)
                {
                    firstNum = rnd.Next(5, 10); // Generates random integer values between 5 and 10
                    secNum = rnd.Next(5, 10); // Generates random integer values between 5 and 10
                }
                else if (questionCount > 40 && questionCount <= 60)
                {
                    firstNum = rnd.Next(10, 15); // Generates random integer values between 10 and 15
                    secNum = rnd.Next(10, 15); // Generates random integer values between 10 and 15
                }
                else if (questionCount > 60 && questionCount <= 80)
                {
                    firstNum = rnd.Next(15, 20); // Generates random integer values between 10 and 15
                    secNum = rnd.Next(15, 20); // Generates random integer values between 10 and 15
                }
                answers = firstNum + secNum;
            }
            else if (level == 2)
            {
                if (operation == 0)
                { //plus operation
                    if (questionCount >= 0 && questionCount <= 20)
                    { //answer not more than 5
                        firstNum = rnd.Next(1, 5); // Generates random integer values between 1 and 5
                        secNum = rnd.Next(1, 5); // Generates random integer values between 1 and 5
                    }
                    else if (questionCount > 20 && questionCount <= 40)
                    {
                        firstNum = rnd.Next(5, 10); // Generates random integer values between 5 and 10
                        secNum = rnd.Next(5, 10); // Generates random integer values between 5 and 10
                    }
                    else if (questionCount > 40 && questionCount <= 60)
                    {
                        firstNum = rnd.Next(10, 15); // Generates random integer values between 10 and 15
                        secNum = rnd.Next(10, 15); // Generates random integer values between 10 and 15
                    }
                    else if (questionCount > 60 && questionCount <= 80)
                    {
                        firstNum = rnd.Next(15, 20); // Generates random integer values between 10 and 15
                        secNum = rnd.Next(15, 20); // Generates random integer values between 10 and 15
                    }
                    answers = firstNum + secNum;
                }
                else if (operation == 1)
                { // minus operation
                    Debug.Log("minus operation");
                    if (questionCount >= 0 && questionCount <= 20)
                    { //answer not more than 10
                        firstNum = rnd.Next(0, 10); // Generates random integer values between 1 and 10
                        secNum = rnd.Next(0, 10); // Generates random integer values between 1 and 10
                        while (secNum < firstNum || secNum == firstNum)
                        {
                            secNum = rnd.Next(0, 10);
                            firstNum = rnd.Next(0, 10);
                        }
                    }
                    else if (questionCount > 20 && questionCount <= 40)
                    {
                        //answer not more than 20
                        firstNum = rnd.Next(0, 20); // Generates random integer values between 1 and 10
                        secNum = rnd.Next(0, 20); // Generates random integer values between 1 and 10
                        while (secNum < firstNum || secNum == firstNum)
                        {
                            secNum = rnd.Next(0, 20);
                            firstNum = rnd.Next(0, 20);
                        }
                    }
                    else if (questionCount > 40 && questionCount <= 60)
                    {
                        //answer not more than 30
                        firstNum = rnd.Next(0, 30); // Generates random integer values between 10 and 15
                        secNum = rnd.Next(0, 30); // Generates random integer values between 10 and 15
                        while (secNum < firstNum || secNum == firstNum)
                        {
                            secNum = rnd.Next(0, 30);
                            firstNum = rnd.Next(0, 30);
                        }
                    }
                    else if (questionCount > 60 && questionCount <= 80)
                    {
                        //answer not more than 10
                        firstNum = rnd.Next(0, 40); // Generates random integer values between 10 and 15
                        secNum = rnd.Next(0, 40); // Generates random integer values between 10 and 15
                        while (secNum < firstNum || secNum == firstNum)
                        {
                            secNum = rnd.Next(0, 40);
                            firstNum = rnd.Next(0, 40);
                        }
                    }
                    answers = secNum - firstNum;
                }
            }

            //asign to question gameobject
            if (level == 1)
            {
                transform
                    .GetChild(3)
                    .transform.GetChild(0)
                    .transform.GetChild(0)
                    .GetComponent<TextMeshProUGUI>()
                    .text = firstNum + " + " + secNum + " ";

                questions = firstNum + " + " + secNum + " ";
            }
            else if (level == 2)
            {
                if (operation == 0)
                {
                    transform
                        .GetChild(3)
                        .transform.GetChild(0)
                        .transform.GetChild(0)
                        .GetComponent<TextMeshProUGUI>()
                        .text = firstNum + " + " + secNum + " ";

                    questions = firstNum + " + " + secNum + " ";
                }
                else if (operation == 1)
                {
                    transform
                        .GetChild(3)
                        .transform.GetChild(0)
                        .transform.GetChild(0)
                        .GetComponent<TextMeshProUGUI>()
                        .text = secNum + " - " + firstNum + " ";

                    questions = secNum + " - " + firstNum + " ";
                }
            }
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

            if (level == 1)
            { //if level 1 only show two answer buttons
                ansButton[2].gameObject.SetActive(false);
                ansButton[3].gameObject.SetActive(false);
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
                        if (questionCount >= 0 && questionCount <= 20) // not more than 10
                        {
                            randomAnswer = rnd.Next(2, 10);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(2, 10);
                            }
                            Debug.Log("answer not more than 10 ");
                        }
                        else if (questionCount > 20 && questionCount <= 40) // not more than 20
                        {
                            randomAnswer = rnd.Next(10, 20);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(10, 20);
                            }
                            Debug.Log("answer not more than 20 ");
                        }
                        else if (questionCount > 40 && questionCount <= 60) // not more than 30
                        {
                            randomAnswer = rnd.Next(20, 30);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(20, 30);
                            }
                            Debug.Log("answer not more than 30 ");
                        }
                        else if (questionCount > 60 && questionCount <= 80) // not more than 40
                        {
                            randomAnswer = rnd.Next(30, 40);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(30, 40);
                            }
                            Debug.Log("answer not more than 40 ");
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
                //only two answer buttons
                ansButton[2].gameObject.SetActive(false);
                ansButton[3].gameObject.SetActive(false);
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
                        if (questionCount >= 0 && questionCount <= 20) // not more than 10
                        {
                            randomAnswer = rnd.Next(0, 5);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(0, 5);
                            }
                            Debug.Log("answer not more than 10 ");
                        }
                        else if (questionCount > 20 && questionCount <= 40) // not more than 20
                        {
                            randomAnswer = rnd.Next(5, 10);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(5, 10);
                            }
                            Debug.Log("answer not more than 20 ");
                        }
                        else if (questionCount > 40 && questionCount <= 60) // not more than 30
                        {
                            randomAnswer = rnd.Next(10, 15);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(10, 15);
                            }
                            Debug.Log("answer not more than 30 ");
                        }
                        else if (questionCount > 60 && questionCount <= 80) // not more than 40
                        {
                            randomAnswer = rnd.Next(15, 20);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(15, 20);
                            }
                            Debug.Log("answer not more than 40 ");
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

        //add the questionCount++
        questionCount++;

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
        questionCount = 0;
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
