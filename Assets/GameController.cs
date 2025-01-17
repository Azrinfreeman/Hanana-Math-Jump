using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public System.Random rnd = new System.Random();
    public System.Random rnd2 = new System.Random();

    public List<AudioSource> music;
    public static GameController instance;
    public Transform FreeLookCamera;

    public Animator CameraAnim;

    public Button SettingBtn;

    void Awake()
    {
        instance = this;
    }

    [Header("PlayerCollections")]
    public Transform moneyTransform;
    public Transform roundTransform;

    public bool[] enterLevel;

    [Header("Questions")]
    public bool gameStart;
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

    [Header("EndScreenMenu")]
    public Transform Bg;
    public Transform endMenuScreen;

    [Header("Buttons")]
    public List<Button> ansButton;

    [Header("TimeToAnswer")]
    public Transform Timing;
    [Header("references")]
    public PathController pathController;

    public EventSystem eventSystem;

    [Header("Optimization")]
    public List<Transform> itemsToHide;

    IEnumerator collectedRound()
    {
        //disable the trigger for tripping
        if (pathController.isTriggerJump)
        {
            //jump collider
            ObstaclesController
                .instance.ObstaclesList[0]
                .transform.GetChild(0)
                .GetComponent<BoxCollider>()
                .enabled = true;
            //crash collider
            ObstaclesController
                .instance.ObstaclesList[0]
                .transform.GetChild(1)
                .GetComponent<BoxCollider>()
                .enabled = false;
        }
        //disable the trigger for tripping
        if (pathController.isTriggerEdge)
        {
            ObstaclesController
                .instance.TriggerJumpList[0]
                .transform.GetChild(1)
                .GetComponent<BoxCollider>()
                .enabled = false;
        }

        //collect stars and timer
        if (TimeToAnswer.instance.maxTime > 8 && TimeToAnswer.instance.maxTime < 10)
        {
            CollectionController.instance.addStars(10);
        }
        else if (TimeToAnswer.instance.maxTime > 4 && TimeToAnswer.instance.maxTime <= 8)
        {
            int t3;
            t3 = (int)TimeToAnswer.instance.maxTime;
            CollectionController.instance.addStars(t3);
        }
        else if (TimeToAnswer.instance.maxTime > 0 && TimeToAnswer.instance.maxTime <= 4)
        {
            CollectionController.instance.addStars(3);
        }

        moneyTransform.GetComponent<Animator>().Play("collected");
        yield return new WaitForSeconds(0.45f);
        GameObject.Find("collected").GetComponent<AudioSource>().Play();
        moneyTransform.GetComponent<Animator>().Play("afterCollected");

        //Collect the round count 
        CollectionController.instance.addRounds(1);
        roundTransform.GetComponent<Animator>().Play("collected");

        yield return new WaitForSeconds(0.45f);
        //play sound
        if (!GameObject.Find("collected").GetComponent<AudioSource>().isPlaying)
        {
            GameObject.Find("collected").GetComponent<AudioSource>().Play();
        }
        yield return new WaitForSeconds(0.25f);
        roundTransform.GetComponent<Animator>().Play("afterCollected");
        //Destroy(gameObject);
    }

    IEnumerator collectedRound80()
    {
        HideQuestionsButtons();
        pathController.timeToReach.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        //disable the trigger for tripping
        if (pathController.isTriggerJump)
        {
            //jump collider need to turn on
            ObstaclesController
                .instance.ObstaclesList[0]
                .transform.GetChild(0)
                .GetComponent<BoxCollider>()
                .enabled = true;

            //crash collider not needed actually
            ObstaclesController
                .instance.ObstaclesList[0]
                .transform.GetChild(1)
                .GetComponent<BoxCollider>()
                .enabled = false;
        }
        //disable the trigger for tripping
        if (pathController.isTriggerEdge)
        {
            ObstaclesController
                .instance.TriggerJumpList[0]
                .transform.GetChild(1)
                .GetComponent<BoxCollider>()
                .enabled = false;
        }

        //Destroy(gameObject);
        //pathController.timeToReach.gameObject.SetActive(false);

        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        ///
        /// make buttons no click again
        for (int i = 0; i < 4; i++)
        {
            ansButton[i].onClick.RemoveAllListeners();
        }
        Time.timeScale = 1;

        /*if (pathController.isTriggerJump)
        {
            //remove the current distance of obstacles
            ObstaclesController.instance.ObstaclesList.RemoveAt(0);
            pathController.isTriggerJump = false;
        }
*/
        //add the questionCount++
        //questionCount++;



        //isQuestionShowUp = false;
        //pathController.isOriginalDistance = false;
        //pathController.timeToReach.gameObject.SetActive(false);
    }

    IEnumerator incorrectAnswer()
    {
        Debug.Log("Incorrect");
        HideQuestionsButtons();
        //disable the trigger for jumping and code into PathController crash Trigger
        if (pathController.isTriggerJump)
        {
            //colllider jump
            ObstaclesController
                .instance.ObstaclesList[0]
                .transform.GetChild(0)
                .GetComponent<BoxCollider>()
                .enabled = false;

            //collider crash not need because already on
        }

        //this is for the edge pathway jumping
        if (pathController.isTriggerEdge)
        {
            ObstaclesController
                .instance.TriggerJumpList[0]
                .transform.GetChild(0)
                .GetComponent<BoxCollider>()
                .enabled = false;
        }

        /// make buttons no click again
        for (int i = 0; i < 4; i++)
        {
            ansButton[i].onClick.RemoveAllListeners();
        }
        Time.timeScale = 1;
        yield return null;
    }

    public void buttonFunction()
    {
        StartCoroutine(incorrectAnswer());
        HideQuestion();
    }

    public void CorrectButtonFunction()
    {
        StartCoroutine(collectedRound());
        HideQuestion();
    }

    public void buttonFunction80()
    {
        StartCoroutine(incorrectAnswer());
        //HideQuestion80();
    }

    public void CorrectButtonFunction80()
    {
        Debug.Log("correctbuttonfunctioin");
        StartCoroutine(collectedRound80());
        //HideQuestion80();
    }

    public void ShowQuestion()
    {
        if (!isQuestionShowUp)
        {
            SettingBtn.interactable = false;
            pathController.timeToReach.gameObject.SetActive(false);
            Timing.gameObject.SetActive(true);
            operation = rnd2.Next(2);
            //if else level
            if (level == 1)
            {
                //questionCount based question

                if (questionCount >= 0 && questionCount <= 20)
                { //answer not more than 10
                    firstNum = rnd.Next(1, 6); // Generates random integer values between 1 and 5
                    secNum = rnd.Next(1, 6); // Generates random integer values between 1 and 5
                }
                else if (questionCount > 20 && questionCount <= 40)
                {
                    firstNum = rnd.Next(6, 11); // Generates random integer values between 5 and 10
                    secNum = rnd.Next(6, 11); // Generates random integer values between 5 and 10
                    // pathController.SetFirstChildToLast();
                    //change character
                    if (enterLevel[1] == false)
                    {
                        Debug.Log("change characeter once");
                        //  pathController.SetFirstChildToLast();
                        enterLevel[0] = false;
                        enterLevel[1] = true;
                    }
                }
                else if (questionCount > 40 && questionCount <= 60)
                {
                    firstNum = rnd.Next(11, 16); // Generates random integer values between 10 and 15
                    secNum = rnd.Next(10, 16); // Generates random integer values between 10 and 15
                    //change character
                    if (enterLevel[2] == false)
                    {
                        //   pathController.SetFirstChildToLast();
                        enterLevel[1] = false;
                        enterLevel[2] = true;
                    }
                }
                else if (questionCount > 60 && questionCount <= 80)
                {
                    firstNum = rnd.Next(16, 21); // Generates random integer values between 10 and 15
                    secNum = rnd.Next(16, 21); // Generates random integer values between 10 and 15
                    //change character
                    if (enterLevel[3] == false)
                    {
                        //   pathController.SetFirstChildToLast();
                        enterLevel[2] = false;
                        enterLevel[3] = true;
                    }
                }
                else if (questionCount > 80)
                {
                    Debug.Log("more than 80");
                    firstNum = rnd.Next(16, 21); // Generates random integer values between 10 and 15
                    secNum = rnd.Next(16, 21); // Generates random integer values between 10 and 15

                    //change character
                    if (enterLevel[4] == false)
                    {
                        //   pathController.SetFirstChildToLast();
                        enterLevel[3] = false;
                        enterLevel[4] = true;
                    }
                }
                answers = firstNum + secNum;
            }
            else if (level == 2)
            {
                if (operation == 0)
                { //plus operation
                    if (questionCount >= 0 && questionCount <= 20)
                    { //answer not more than 5
                        firstNum = rnd.Next(1, 6); // Generates random integer values between 1 and 5
                        secNum = rnd.Next(1, 6); // Generates random integer values between 1 and 5
                    }
                    else if (questionCount > 20 && questionCount <= 40)
                    {
                        firstNum = rnd.Next(6, 11); // Generates random integer values between 5 and 10
                        secNum = rnd.Next(6, 11); // Generates random integer values between 5 and 10

                        //change character
                        if (enterLevel[1] == false)
                        {
                            //  pathController.SetFirstChildToLast();
                            enterLevel[0] = false;
                            enterLevel[1] = true;
                        }
                    }
                    else if (questionCount > 40 && questionCount <= 60)
                    {
                        firstNum = rnd.Next(11, 16); // Generates random integer values between 10 and 15
                        secNum = rnd.Next(11, 16); // Generates random integer values between 10 and 15

                        //change character
                        if (enterLevel[2] == false)
                        {
                            //   pathController.SetFirstChildToLast();
                            enterLevel[1] = false;
                            enterLevel[2] = true;
                        }
                    }
                    else if (questionCount > 60 && questionCount <= 80)
                    {
                        firstNum = rnd.Next(16, 21); // Generates random integer values between 10 and 15
                        secNum = rnd.Next(16, 21); // Generates random integer values between 10 and 15

                        //change character
                        if (enterLevel[3] == false)
                        {
                            //  pathController.SetFirstChildToLast();
                            enterLevel[2] = false;
                            enterLevel[3] = true;
                        }
                    }
                    else if (questionCount > 80)
                    {
                        Debug.Log("more than 80");
                        firstNum = rnd.Next(16, 21); // Generates random integer values between 10 and 15
                        secNum = rnd.Next(16, 21); // Generates random integer values between 10 and 15

                        //change character
                        if (enterLevel[4] == false)
                        {
                            //  pathController.SetFirstChildToLast();
                            enterLevel[3] = false;
                            enterLevel[4] = true;
                        }
                    }
                    answers = firstNum + secNum;
                }
                else if (operation == 1)
                { // minus operation
                    Debug.Log("minus operation");
                    if (questionCount >= 0 && questionCount <= 20)
                    { //answer not more than 10
                        firstNum = rnd.Next(0, 11); // Generates random integer values between 1 and 10
                        secNum = rnd.Next(0, 11); // Generates random integer values between 1 and 10
                        while (secNum < firstNum || secNum == firstNum)
                        {
                            secNum = rnd.Next(0, 11);
                            firstNum = rnd.Next(0, 11);
                        }
                    }
                    else if (questionCount > 20 && questionCount <= 40)
                    {
                        //answer not more than 20
                        firstNum = rnd.Next(0, 21); // Generates random integer values between 1 and 10
                        secNum = rnd.Next(0, 21); // Generates random integer values between 1 and 10
                        while (secNum < firstNum || secNum == firstNum)
                        {
                            secNum = rnd.Next(0, 21);
                            firstNum = rnd.Next(0, 21);
                        } //change character
                        if (enterLevel[1] == false)
                        {
                            //   pathController.SetFirstChildToLast();
                            enterLevel[0] = false;
                            enterLevel[1] = true;
                        }
                    }
                    else if (questionCount > 40 && questionCount <= 60)
                    {
                        //answer not more than 30
                        firstNum = rnd.Next(0, 31); // Generates random integer values between 10 and 15
                        secNum = rnd.Next(0, 31); // Generates random integer values between 10 and 15
                        while (secNum < firstNum || secNum == firstNum)
                        {
                            secNum = rnd.Next(0, 31);
                            firstNum = rnd.Next(0, 31);
                        } //change character
                        if (enterLevel[2] == false)
                        {
                            //  pathController.SetFirstChildToLast();
                            enterLevel[1] = false;
                            enterLevel[2] = true;
                        }
                    }
                    else if (questionCount > 60 && questionCount <= 80)
                    {
                        //answer not more than 10
                        firstNum = rnd.Next(0, 41); // Generates random integer values between 10 and 15
                        secNum = rnd.Next(0, 41); // Generates random integer values between 10 and 15
                        while (secNum < firstNum || secNum == firstNum)
                        {
                            secNum = rnd.Next(0, 41);
                            firstNum = rnd.Next(0, 41);
                        } //change character
                        if (enterLevel[3] == false)
                        {
                            // pathController.SetFirstChildToLast();

                            enterLevel[2] = false;
                            enterLevel[3] = true;
                        }
                    }
                    else if (questionCount > 80)
                    {
                        Debug.Log("more than 80");
                        firstNum = rnd.Next(0, 41); // Generates random integer values between 10 and 15
                        secNum = rnd.Next(0, 41); // Generates random integer values between 10 and 15
                        while (secNum < firstNum || secNum == firstNum)
                        {
                            secNum = rnd.Next(0, 41);
                            firstNum = rnd.Next(0, 41);
                        }
                        //change character
                        if (enterLevel[4] == false)
                        {
                            //  pathController.SetFirstChildToLast();
                            enterLevel[3] = false;
                            enterLevel[4] = true;
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

            //assign random answer to random buttons
            if (level == 1)
            { //if level 1 only show two answer buttons
                ansButton[2].gameObject.SetActive(false);
                ansButton[3].gameObject.SetActive(false);
                //choose which button to put answer
                int buttonNum = rnd.Next(2);
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
                            randomAnswer = rnd.Next(2, 11);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(2, 11);
                            }
                            //                            Debug.Log("answer not more than 10 ");
                        }
                        else if (questionCount > 20 && questionCount <= 40) // not more than 20
                        {
                            randomAnswer = rnd.Next(11, 21);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(10, 21);
                            }
                            Debug.Log("answer not more than 20 ");
                        }
                        else if (questionCount > 40 && questionCount <= 60) // not more than 30
                        {
                            randomAnswer = rnd.Next(21, 31);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(21, 31);
                            }
                            Debug.Log("answer not more than 30 ");
                        }
                        else if (questionCount > 60 && questionCount <= 80) // not more than 40
                        {
                            randomAnswer = rnd.Next(31, 41);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(31, 41);
                            }
                            //                            Debug.Log("answer not more than 40 ");
                        }
                        else if (questionCount > 80)
                        {
                            randomAnswer = rnd.Next(31, 41);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(31, 41);
                            }
                            Debug.Log("answer not more than 80 ");
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
                int buttonNum = rnd.Next(2);
                ansButton[buttonNum].onClick.AddListener(() => CorrectButtonFunction());
                ansButton[buttonNum]
                    .transform.GetChild(0)
                    .transform.GetChild(0)
                    .GetComponent<TextMeshProUGUI>()
                    .text = answers.ToString();
                for (int i = 0; i < 2; i++)
                {
                    if (operation == 0)
                    {
                        if (i != buttonNum)
                        {
                            if (questionCount >= 0 && questionCount <= 20) // not more than 10
                            {
                                randomAnswer = rnd.Next(2, 11);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(2, 11);
                                }
                                Debug.Log("answer not more than 10 ");
                            }
                            else if (questionCount > 20 && questionCount <= 40) // not more than 20
                            {
                                randomAnswer = rnd.Next(11, 21);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(11, 21);
                                }
                                Debug.Log("answer not more than 20 ");
                            }
                            else if (questionCount > 40 && questionCount <= 60) // not more than 30
                            {
                                randomAnswer = rnd.Next(21, 31);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(21, 31);
                                }
                                Debug.Log("answer not more than 30 ");
                            }
                            else if (questionCount > 60 && questionCount <= 80) // not more than 40
                            {
                                randomAnswer = rnd.Next(31, 41);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(31, 41);
                                }
                                Debug.Log("answer not more than 40 ");
                            }
                            else if (questionCount > 80)
                            {
                                randomAnswer = rnd.Next(31, 41);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(31, 41);
                                }
                                Debug.Log("answer not more than 80 ");
                            }

                            ansButton[i].onClick.AddListener(() => buttonFunction());
                            ansButton[i]
                                .transform.GetChild(0)
                                .transform.GetChild(0)
                                .GetComponent<TextMeshProUGUI>()
                                .text = randomAnswer.ToString();
                        }
                    }
                    else if (operation == 1)
                    {
                        if (i != buttonNum)
                        {
                            if (questionCount >= 0 && questionCount <= 20) // not more than 10
                            {
                                randomAnswer = rnd.Next(0, 6);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(0, 6);
                                }
                                Debug.Log("answer not more than 10 ");
                            }
                            else if (questionCount > 20 && questionCount <= 40) // not more than 20
                            {
                                randomAnswer = rnd.Next(6, 11);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(6, 11);
                                }
                                Debug.Log("answer not more than 20 ");
                            }
                            else if (questionCount > 40 && questionCount <= 60) // not more than 30
                            {
                                randomAnswer = rnd.Next(11, 16);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(11, 16);
                                }
                                Debug.Log("answer not more than 30 ");
                            }
                            else if (questionCount > 60 && questionCount <= 80) // not more than 40
                            {
                                randomAnswer = rnd.Next(16, 21);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(16, 21);
                                }
                                Debug.Log("answer not more than 40 ");
                            }
                            else if (questionCount > 80)
                            {
                                randomAnswer = rnd.Next(16, 21);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(16, 21);
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

    public void ShowQuestion80()
    {
        if (!isQuestionShowUp)
        {
            Timing.gameObject.SetActive(true);
            operation = rnd2.Next(2);
            //if else level
            if (level == 1)
            {
                //questionCount based question

                if (questionCount >= 0 && questionCount <= 20)
                { //answer not more than 10
                    firstNum = rnd.Next(1, 6); // Generates random integer values between 1 and 5
                    secNum = rnd.Next(1, 6); // Generates random integer values between 1 and 5
                }
                else if (questionCount > 20 && questionCount <= 40)
                {
                    firstNum = rnd.Next(6, 11); // Generates random integer values between 5 and 10
                    secNum = rnd.Next(6, 11); // Generates random integer values between 5 and 10
                    // pathController.SetFirstChildToLast();
                    //change character
                    if (enterLevel[1] == false)
                    {
                        Debug.Log("change characeter once");
                        //  pathController.SetFirstChildToLast();
                        enterLevel[0] = false;
                        enterLevel[1] = true;
                    }
                }
                else if (questionCount > 40 && questionCount <= 60)
                {
                    firstNum = rnd.Next(11, 16); // Generates random integer values between 10 and 15
                    secNum = rnd.Next(10, 16); // Generates random integer values between 10 and 15
                    //change character
                    if (enterLevel[2] == false)
                    {
                        //  pathController.SetFirstChildToLast();
                        enterLevel[1] = false;
                        enterLevel[2] = true;
                    }
                }
                else if (questionCount > 60 && questionCount <= 80)
                {
                    firstNum = rnd.Next(16, 21); // Generates random integer values between 10 and 15
                    secNum = rnd.Next(16, 21); // Generates random integer values between 10 and 15
                    //change character
                    if (enterLevel[3] == false)
                    {
                        //  pathController.SetFirstChildToLast();
                        enterLevel[2] = false;
                        enterLevel[3] = true;
                    }
                }
                else if (questionCount > 80)
                {
                    //                    Debug.Log("more than 80");
                    firstNum = rnd.Next(16, 21); // Generates random integer values between 10 and 15
                    secNum = rnd.Next(16, 21); // Generates random integer values between 10 and 15
                    //pathController.SetFirstChildToLast();
                    //change character
                    if (enterLevel[4] == false)
                    {
                        //  pathController.SetFirstChildToLast();
                        enterLevel[3] = false;
                        enterLevel[4] = true;
                    }
                }
                answers = firstNum + secNum;
            }
            else if (level == 2)
            {
                if (operation == 0)
                { //plus operation
                    if (questionCount >= 0 && questionCount <= 20)
                    { //answer not more than 5
                        firstNum = rnd.Next(1, 6); // Generates random integer values between 1 and 5
                        secNum = rnd.Next(1, 6); // Generates random integer values between 1 and 5
                    }
                    else if (questionCount > 20 && questionCount <= 40)
                    {
                        firstNum = rnd.Next(6, 11); // Generates random integer values between 5 and 10
                        secNum = rnd.Next(6, 11); // Generates random integer values between 5 and 10

                        //change character
                        if (enterLevel[1] == false)
                        {
                            // pathController.SetFirstChildToLast();
                            enterLevel[0] = false;
                            enterLevel[1] = true;
                        }
                    }
                    else if (questionCount > 40 && questionCount <= 60)
                    {
                        firstNum = rnd.Next(11, 16); // Generates random integer values between 10 and 15
                        secNum = rnd.Next(11, 16); // Generates random integer values between 10 and 15

                        //change character
                        if (enterLevel[2] == false)
                        {
                            // pathController.SetFirstChildToLast();
                            enterLevel[1] = false;
                            enterLevel[2] = true;
                        }
                    }
                    else if (questionCount > 60 && questionCount <= 80)
                    {
                        firstNum = rnd.Next(16, 21); // Generates random integer values between 10 and 15
                        secNum = rnd.Next(16, 21); // Generates random integer values between 10 and 15

                        //change character
                        if (enterLevel[3] == false)
                        {
                            //pathController.SetFirstChildToLast();
                            enterLevel[2] = false;
                            enterLevel[3] = true;
                        }
                    }
                    else if (questionCount > 80)
                    {
                        Debug.Log("more than 80");
                        firstNum = rnd.Next(16, 21); // Generates random integer values between 10 and 15
                        secNum = rnd.Next(16, 21); // Generates random integer values between 10 and 15

                        //change character
                        if (enterLevel[4] == false)
                        {
                            //pathController.SetFirstChildToLast();
                            enterLevel[3] = false;
                            enterLevel[4] = true;
                        }
                    }
                    answers = firstNum + secNum;
                }
                else if (operation == 1)
                { // minus operation
                    Debug.Log("minus operation");
                    if (questionCount >= 0 && questionCount <= 20)
                    { //answer not more than 10
                        firstNum = rnd.Next(0, 11); // Generates random integer values between 1 and 10
                        secNum = rnd.Next(0, 11); // Generates random integer values between 1 and 10
                        while (secNum < firstNum || secNum == firstNum)
                        {
                            secNum = rnd.Next(0, 11);
                            firstNum = rnd.Next(0, 11);
                        }
                    }
                    else if (questionCount > 20 && questionCount <= 40)
                    {
                        //answer not more than 20
                        firstNum = rnd.Next(0, 21); // Generates random integer values between 1 and 10
                        secNum = rnd.Next(0, 21); // Generates random integer values between 1 and 10
                        while (secNum < firstNum || secNum == firstNum)
                        {
                            secNum = rnd.Next(0, 21);
                            firstNum = rnd.Next(0, 21);
                        } //change character
                        if (enterLevel[1] == false)
                        {
                            //pathController.SetFirstChildToLast();
                            enterLevel[0] = false;
                            enterLevel[1] = true;
                        }
                    }
                    else if (questionCount > 40 && questionCount <= 60)
                    {
                        //answer not more than 30
                        firstNum = rnd.Next(0, 31); // Generates random integer values between 10 and 15
                        secNum = rnd.Next(0, 31); // Generates random integer values between 10 and 15
                        while (secNum < firstNum || secNum == firstNum)
                        {
                            secNum = rnd.Next(0, 31);
                            firstNum = rnd.Next(0, 31);
                        } //change character
                        if (enterLevel[2] == false)
                        {
                            //pathController.SetFirstChildToLast();
                            enterLevel[1] = false;
                            enterLevel[2] = true;
                        }
                    }
                    else if (questionCount > 60 && questionCount <= 80)
                    {
                        //answer not more than 10
                        firstNum = rnd.Next(0, 41); // Generates random integer values between 10 and 15
                        secNum = rnd.Next(0, 41); // Generates random integer values between 10 and 15
                        while (secNum < firstNum || secNum == firstNum)
                        {
                            secNum = rnd.Next(0, 41);
                            firstNum = rnd.Next(0, 41);
                        } //change character
                        if (enterLevel[3] == false)
                        {
                            //pathController.SetFirstChildToLast();

                            enterLevel[2] = false;
                            enterLevel[3] = true;
                        }
                    }
                    else if (questionCount > 80)
                    {
                        Debug.Log("more than 80");
                        firstNum = rnd.Next(0, 41); // Generates random integer values between 10 and 15
                        secNum = rnd.Next(0, 41); // Generates random integer values between 10 and 15
                        while (secNum < firstNum || secNum == firstNum)
                        {
                            secNum = rnd.Next(0, 41);
                            firstNum = rnd.Next(0, 41);
                        }
                        //change character
                        if (enterLevel[4] == false)
                        {
                            //pathController.SetFirstChildToLast();
                            enterLevel[3] = false;
                            enterLevel[4] = true;
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

            //assign random answer to random buttons
            if (level == 1)
            { //if level 1 only show two answer buttons
                ansButton[2].gameObject.SetActive(false);
                ansButton[3].gameObject.SetActive(false);
                //choose which button to put answer
                int buttonNum = rnd.Next(2);
                ansButton[buttonNum].onClick.AddListener(() => CorrectButtonFunction80());
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
                            randomAnswer = rnd.Next(2, 11);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(2, 11);
                            }
                            //                            Debug.Log("answer not more than 10 ");
                        }
                        else if (questionCount > 20 && questionCount <= 40) // not more than 20
                        {
                            randomAnswer = rnd.Next(11, 21);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(10, 21);
                            }
                            Debug.Log("answer not more than 20 ");
                        }
                        else if (questionCount > 40 && questionCount <= 60) // not more than 30
                        {
                            randomAnswer = rnd.Next(21, 31);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(21, 31);
                            }
                            Debug.Log("answer not more than 30 ");
                        }
                        else if (questionCount > 60 && questionCount <= 80) // not more than 40
                        {
                            randomAnswer = rnd.Next(31, 41);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(31, 41);
                            }
                            //                            Debug.Log("answer not more than 40 ");
                        }
                        else if (questionCount > 80)
                        {
                            randomAnswer = rnd.Next(31, 41);
                            while (randomAnswer == answers)
                            {
                                randomAnswer = rnd.Next(31, 41);
                            }
                            //                            Debug.Log("answer not more than 80 ");
                        }

                        ansButton[i].onClick.AddListener(() => buttonFunction80());
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
                int buttonNum = rnd.Next(2);
                ansButton[buttonNum].onClick.AddListener(() => CorrectButtonFunction80());
                ansButton[buttonNum]
                    .transform.GetChild(0)
                    .transform.GetChild(0)
                    .GetComponent<TextMeshProUGUI>()
                    .text = answers.ToString();
                for (int i = 0; i < 2; i++)
                {
                    if (operation == 0)
                    {
                        if (i != buttonNum)
                        {
                            if (questionCount >= 0 && questionCount <= 20) // not more than 10
                            {
                                randomAnswer = rnd.Next(2, 11);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(2, 11);
                                }
                                Debug.Log("answer not more than 10 ");
                            }
                            else if (questionCount > 20 && questionCount <= 40) // not more than 20
                            {
                                randomAnswer = rnd.Next(11, 21);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(11, 21);
                                }
                                Debug.Log("answer not more than 20 ");
                            }
                            else if (questionCount > 40 && questionCount <= 60) // not more than 30
                            {
                                randomAnswer = rnd.Next(21, 31);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(21, 31);
                                }
                                Debug.Log("answer not more than 30 ");
                            }
                            else if (questionCount > 60 && questionCount <= 80) // not more than 40
                            {
                                randomAnswer = rnd.Next(31, 41);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(31, 41);
                                }
                                Debug.Log("answer not more than 40 ");
                            }
                            else if (questionCount > 80)
                            {
                                randomAnswer = rnd.Next(31, 41);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(31, 41);
                                }
                                Debug.Log("answer not more than 80 ");
                            }

                            ansButton[i].onClick.AddListener(() => buttonFunction());
                            ansButton[i]
                                .transform.GetChild(0)
                                .transform.GetChild(0)
                                .GetComponent<TextMeshProUGUI>()
                                .text = randomAnswer.ToString();
                        }
                    }
                    else if (operation == 1)
                    {
                        if (i != buttonNum)
                        {
                            if (questionCount >= 0 && questionCount <= 20) // not more than 10
                            {
                                randomAnswer = rnd.Next(0, 6);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(0, 6);
                                }
                                Debug.Log("answer not more than 10 ");
                            }
                            else if (questionCount > 20 && questionCount <= 40) // not more than 20
                            {
                                randomAnswer = rnd.Next(6, 11);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(6, 11);
                                }
                                Debug.Log("answer not more than 20 ");
                            }
                            else if (questionCount > 40 && questionCount <= 60) // not more than 30
                            {
                                randomAnswer = rnd.Next(11, 16);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(11, 16);
                                }
                                Debug.Log("answer not more than 30 ");
                            }
                            else if (questionCount > 60 && questionCount <= 80) // not more than 40
                            {
                                randomAnswer = rnd.Next(16, 21);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(16, 21);
                                }
                                Debug.Log("answer not more than 40 ");
                            }
                            else if (questionCount > 80)
                            {
                                randomAnswer = rnd.Next(16, 21);
                                while (randomAnswer == answers)
                                {
                                    randomAnswer = rnd.Next(16, 21);
                                }
                                Debug.Log("answer not more than 40 ");
                            }

                            ansButton[i].onClick.AddListener(() => buttonFunction80());
                            ansButton[i]
                                .transform.GetChild(0)
                                .transform.GetChild(0)
                                .GetComponent<TextMeshProUGUI>()
                                .text = randomAnswer.ToString();
                        }
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

            //Time.timeScale = 0;
            isQuestionShowUp = true;
        }
    }

    public void HideQuestionsButtons()
    {
        //hide background and hide question also off center;
        transform.GetChild(0).transform.gameObject.SetActive(false);
        transform.GetChild(2).transform.gameObject.SetActive(false);
        transform.GetChild(3).transform.gameObject.SetActive(false);
    }

    public void HideQuestion80()
    {
        for (int i = 0; i < 4; i++)
        {
            ansButton[i].onClick.RemoveAllListeners();
        }

        if (pathController.isTriggerJump)
        {
            //remove the current distance of obstacles
            ObstaclesController.instance.ObstaclesList.RemoveAt(0);
            pathController.isTriggerJump = false;
        }

        if (pathController.isTriggerEdge)
        {
            //remove the current distance of obstacles
            ObstaclesController.instance.TriggerJumpList.RemoveAt(0);
            pathController.isTriggerEdge = false;
        } //hide background and hide question also off center;
        transform.GetChild(0).transform.gameObject.SetActive(false);
        transform.GetChild(2).transform.gameObject.SetActive(false);
        transform.GetChild(3).transform.gameObject.SetActive(false);

        //add the questionCount++
        questionCount++;

        Time.timeScale = 1;

        //isQuestionShowUp = false;
        pathController.isOriginalDistance = false;
        pathController.timeToReach.gameObject.SetActive(false);
    }

    public void HideQuestion()
    {
        SettingBtn.interactable = true;
        for (int i = 0; i < 4; i++)
        {
            ansButton[i].onClick.RemoveAllListeners();
        }

        if (pathController.isTriggerJump)
        {
            //remove the current distance of obstacles
            ObstaclesController.instance.ObstaclesList.RemoveAt(0);
            pathController.isTriggerJump = false;
        }

        if (pathController.isTriggerEdge)
        {
            //remove the current distance of obstacles
            ObstaclesController.instance.TriggerJumpList.RemoveAt(0);
            pathController.isTriggerEdge = false;
        } //hide background and hide question also off center;
        transform.GetChild(0).transform.gameObject.SetActive(false);
        transform.GetChild(2).transform.gameObject.SetActive(false);
        transform.GetChild(3).transform.gameObject.SetActive(false);

        //add the questionCount++
        questionCount++;

        Time.timeScale = 1;

        isQuestionShowUp = false;
        pathController.isOriginalDistance = false;
        pathController.timeToReach.gameObject.SetActive(false);

        //tiing
        Timing.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("MaxCharacters", PathController.instance.TransformAnimList.Count);
        if (PlayerPrefs.GetInt("Character_No_0") == 0)
        {
            Debug.Log("true");
            for (int i = 0; i < PathController.instance.TransformAnimList.Count; i++)
            {
                if (i == 0)
                {

                    PlayerPrefs.SetInt("Character_No_" + PathController.instance.TransformAnimList[i].transform.GetSiblingIndex(), 1);

                }
                else
                    PlayerPrefs.SetInt("Character_No_" + PathController.instance.TransformAnimList[i].transform.GetSiblingIndex(), 0);
            }
        }

        enterLevel = new bool[5];
        enterLevel[0] = true;
        Bg = transform.Find("bg").GetComponent<Transform>();
        endMenuScreen = transform.Find("EndScreen").GetComponent<Transform>();
        moneyTransform = transform.GetChild(1).transform.GetChild(0).GetComponent<Transform>();
        roundTransform = transform.GetChild(1).transform.GetChild(1).GetComponent<Transform>();
        pathController = GameObject.Find("Player").GetComponent<PathController>();
        levelText = transform.Find("Top").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        questionCount = 0;
        //Time.timeScale = 1;
        gameStart = false;
        SettingBtn = GameObject.Find("SettingButton").transform.GetChild(0).GetComponent<Button>();
        SettingBtn.interactable = false;
    }

    public void freezeTime()
    {
        Time.timeScale = 0f;
    }

    public void unfreezeTime()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            levelText.text = "LEVEL " + level.ToString();

            if (pathController.DistanceBetween < 2f)
            {
                if (questionCount < 80)
                {
                    pathController.isTriggerJump = true;
                    ShowQuestion();
                }
            }
            else if (pathController.DistanceBetween > 0f)
            {
                if (questionCount >= 80)
                {
                    StartCoroutine(pathController.displayTimer(1.34f));
                    //pathController.isTriggerJump = true;
                }
            }
        }
        //temp disabled
        /*
        if (pathController.DistanceBetweenEdge < 2f)
        {
            pathController.isTriggerEdge = true;
            ShowQuestion();
        }
        */
    }

    public void StartGame()
    {
        StartCoroutine(startingGame());
    }

    IEnumerator startingGame()
    {
        //Time.timeScale = 1;
        CameraAnim.Play("startCameraAnimation");
        level = Int32.Parse(EventSystem.current.currentSelectedGameObject.name);
        eventSystem.enabled = false;
        yield return new WaitForSeconds(2.3f);
        eventSystem.enabled = true;
        for (int i = 0; i < itemsToHide.Count; i++)
        {
            itemsToHide[i].gameObject.SetActive(false);
        }
        gameStart = true;

        if (!music[1].isPlaying)
        {
            music[0].Stop();
            music[1].Play();
        }

        FreeLookCamera.gameObject.SetActive(true);
        CameraAnim.enabled = false;
        SettingBtn.interactable = true;
        PathController.instance.wind.transform.gameObject.SetActive(true);
    }
}
