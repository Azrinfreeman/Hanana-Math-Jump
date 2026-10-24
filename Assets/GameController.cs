using System;
using System.Collections;
using System.Collections.Generic;
using FunMath;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    int lastStars;

    void AwardStars()
    {
        float t = TimeToAnswer.instance.maxTime;
        lastStars = 0;
        if (t > 8 && t < 10)
        {
            lastStars = 10;
        }
        else if (t > 4 && t <= 8)
        {
            lastStars = (int)t;
        }
        else if (t > 0 && t <= 4)
        {
            lastStars = 3;
        }
        if (lastStars > 0)
        {
            CollectionController.instance.addStars(lastStars);
        }
    }

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

        AwardStars();

        moneyTransform.GetComponent<Animator>().Play("collected");
        yield return new WaitForSeconds(0.45f);
        Sfx.Play("collected");
        moneyTransform.GetComponent<Animator>().Play("afterCollected");

        //Collect the round count 
        CollectionController.instance.addRounds(1);
        roundTransform.GetComponent<Animator>().Play("collected");

        yield return new WaitForSeconds(0.45f);
        //play sound
        Sfx.PlayIfIdle("collected");
        yield return new WaitForSeconds(0.25f);
        roundTransform.GetComponent<Animator>().Play("afterCollected");
        //Destroy(gameObject);
    }

    IEnumerator collectedRound80()
    {
        AwardStars();
        Timing.gameObject.SetActive(false);
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
        if (!questionIsPaused)
        {
            HideQuestionsButtons();
        }
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
        if (answering)
            return;
        answering = true;
        CountAnswer(false);
        StartCoroutine(incorrectAnswer());
        Time.timeScale = 0; // stay paused while the result is shown
        StartCoroutine(Feedback(false, HideQuestion));
    }

    public void CorrectButtonFunction()
    {
        if (answering)
            return;
        answering = true;
        CountAnswer(true);
        StartCoroutine(collectedRound());
        StartCoroutine(StarPopup(lastStars));
        StartCoroutine(Feedback(true, HideQuestion));
    }

    public void buttonFunction80()
    {
        CountAnswer(false);
        StartCoroutine(incorrectAnswer());
        Timing.gameObject.SetActive(false);
        //HideQuestion80();
    }

    public void CorrectButtonFunction80()
    {
        CountAnswer(true);
        StartCoroutine(collectedRound80());
        StartCoroutine(StarPopup(lastStars));
        //HideQuestion80();
    }

    // true when the current question froze the game (before question 80)
    bool questionIsPaused;

    public void ShowQuestion()
    {
        if (!isQuestionShowUp)
        {
            SettingBtn.interactable = false;
            pathController.timeToReach.gameObject.SetActive(false);
            Timing.gameObject.SetActive(true);
            questionIsPaused = true;
            BuildQuestion(CorrectButtonFunction, buttonFunction);
            Time.timeScale = 0;
        }
    }

    public void ShowQuestion80()
    {
        if (!isQuestionShowUp)
        {
            Timing.gameObject.SetActive(true);
            questionIsPaused = false;
            BuildQuestion(CorrectButtonFunction80, buttonFunction80);
        }
    }

    // Called by TimeToAnswer when the countdown reaches 0: counts as a wrong answer.
    public void OnQuestionTimeUp()
    {
        if (!isQuestionShowUp || answering)
            return;
        if (questionIsPaused)
            buttonFunction();
        else
            buttonFunction80();
    }

    int correctIndex;
    bool answering;
    TextMeshProUGUI popup;

    // run summary for the end screen
    int runStartStars;
    int runStartRounds;
    int streak;
    int bestStreak;
    TextMeshProUGUI summary;

    static int Saved(string key)
    {
        return PlayerPrefs.GetInt(key + PlayerPrefs.GetInt("CurrentPlayerNo_"));
    }

    void CountAnswer(bool correct)
    {
        streak = correct ? streak + 1 : 0;
        bestStreak = Mathf.Max(bestStreak, streak);
    }

    public void ShowRunSummary()
    {
        Transform title = endMenuScreen.Find("Title");
        if (title)
        {
            title.GetComponent<TMP_Text>().text = "GAME OVER";
        }

        if (!summary)
        {
            var go = new GameObject("Summary", typeof(RectTransform), typeof(TextMeshProUGUI));
            go.transform.SetParent(endMenuScreen, false);
            summary = go.GetComponent<TextMeshProUGUI>();
            summary.font = levelText.font;
            summary.fontSharedMaterial = levelText.fontSharedMaterial;
            summary.fontSize = 70;
            summary.alignment = TextAlignmentOptions.Center;
            summary.raycastTarget = false;
            var rt = (RectTransform)go.transform;
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);
            rt.anchoredPosition = new Vector2(0f, -1380f);
            rt.sizeDelta = new Vector2(1400f, 300f);
        }
        int stars = Mathf.Max(0, Saved("StarsCollected_") - runStartStars);
        int rounds = Mathf.Max(0, Saved("RoundsCollected_") - runStartRounds);
        summary.text =
            "STARS +" + stars + "   ROUNDS +" + rounds
            + "\nQUESTIONS " + questionCount + "   BEST STREAK " + bestStreak;
    }

    static void ResetAnswerButton(Button button)
    {
        button.interactable = true;
        button.transform.localScale = Vector3.one;
        ((RectTransform)button.transform).anchoredPosition = Vector2.zero;
        button.transform.GetChild(0).GetComponent<Image>().color = UiColors.Primary;
    }

    // Shows which answer was right (and shakes the wrong one) for a moment, then continues.
    IEnumerator Feedback(bool correct, System.Action then)
    {
        for (int i = 0; i < 2; i++)
        {
            ansButton[i].onClick.RemoveAllListeners();
            ansButton[i].interactable = false;
        }

        Transform right = ansButton[correctIndex].transform;
        Transform wrong = ansButton[1 - correctIndex].transform;
        right.GetChild(0).GetComponent<Image>().color = UiColors.Correct;
        if (!correct)
        {
            wrong.GetChild(0).GetComponent<Image>().color = UiColors.Wrong;
        }

        const float duration = 0.55f;
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            float k = t / duration;
            if (correct)
            {
                right.localScale = Vector3.one * (1f + 0.12f * Mathf.Sin(k * Mathf.PI));
            }
            else
            {
                ((RectTransform)wrong).anchoredPosition = new Vector2(Mathf.Sin(t * 60f) * 28f * (1f - k), 0f);
            }
            yield return null;
        }

        right.localScale = Vector3.one;
        ((RectTransform)wrong).anchoredPosition = Vector2.zero;
        then();
    }

    // "+N" floats from the question to the star counter.
    IEnumerator StarPopup(int amount)
    {
        if (amount <= 0)
            yield break;

        if (!popup)
        {
            var go = new GameObject("StarPopup", typeof(RectTransform), typeof(TextMeshProUGUI));
            go.transform.SetParent(transform, false);
            popup = go.GetComponent<TextMeshProUGUI>();
            popup.font = levelText.font;
            popup.fontSharedMaterial = levelText.fontSharedMaterial;
            popup.fontSize = 170;
            popup.alignment = TextAlignmentOptions.Center;
            popup.enableWordWrapping = false;
            popup.raycastTarget = false;
            popup.color = UiColors.Star;
        }

        Vector3 start = transform.GetChild(3).GetChild(0).position;
        Vector3 end = moneyTransform.position;
        popup.text = "+" + amount;
        popup.transform.SetAsLastSibling();
        popup.gameObject.SetActive(true);

        const float duration = 0.85f;
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            float k = Mathf.SmoothStep(0f, 1f, t / duration);
            popup.transform.position = Vector3.Lerp(start, end, k);
            popup.transform.localScale = Vector3.one * Mathf.Lerp(1.2f, 0.6f, k);
            popup.alpha = t / duration > 0.7f ? Mathf.InverseLerp(1f, 0.7f, t / duration) : 1f;
            yield return null;
        }
        popup.gameObject.SetActive(false);
    }

    void BuildQuestion(UnityAction onCorrect, UnityAction onWrong)
    {
        int tier = QuestionGenerator.TierFor(questionCount);
        //change character / tile set once per tier
        if (tier > 0 && !enterLevel[tier])
        {
            enterLevel[tier - 1] = false;
            enterLevel[tier] = true;
        }

        FunMath.Question q = QuestionGenerator.Generate(rnd, level, questionCount, questions);
        operation = q.IsSubtraction ? 1 : 0;
        firstNum = q.First;
        secNum = q.Second;
        answers = q.Answer;
        randomAnswer = q.Distractor;
        questions = q.Text;

        Transform ui = transform.GetChild(3);
        ui.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = q.Text;

        ansButton[0] = ui.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>();
        ansButton[1] = ui.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>();
        ansButton[2] = ui.GetChild(2).GetChild(0).GetChild(0).GetComponent<Button>();
        ansButton[3] = ui.GetChild(2).GetChild(1).GetChild(0).GetComponent<Button>();

        //only two answer buttons are used
        ansButton[2].gameObject.SetActive(false);
        ansButton[3].gameObject.SetActive(false);

        correctIndex = rnd.Next(2);
        answering = false;
        for (int i = 0; i < 2; i++)
        {
            bool isCorrect = i == correctIndex;
            ResetAnswerButton(ansButton[i]);
            ansButton[i].onClick.RemoveAllListeners();
            ansButton[i].onClick.AddListener(isCorrect ? onCorrect : onWrong);
            ansButton[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (
                isCorrect ? q.Answer : q.Distractor
            ).ToString();
        }

        //play notidication sound
        Sfx.PlayIfIdle("notification");
        //activate background and show question also off center;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
        ui.gameObject.SetActive(true);

        isQuestionShowUp = true;
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
        bool unlocksInitialised =
            PlayerPrefs.GetInt("CharacterUnlocksInit") == 1
            || PlayerPrefs.GetInt("Character_No_0") == 1;
        PlayerPrefs.SetInt("CharacterUnlocksInit", 1);
        if (!unlocksInitialised)
        {
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
        levelText.text = "LEVEL " + level;
        runStartStars = Saved("StarsCollected_");
        runStartRounds = Saved("RoundsCollected_");
        streak = bestStreak = 0;

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
