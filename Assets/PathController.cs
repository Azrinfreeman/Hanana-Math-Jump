using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public static PathController instance;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
    [Header("PlayerInformation")]
    public Rigidbody rb;
    public Animator anim;

    public List<Transform> TransformAnimList;
    public float jumpForce = 2.0f;
    public Vector3 jump;

    [SerializeField]
    public float moveSpeed;

    public bool isTriggerJump;
    public bool isTriggerEdge;
    public float DistanceBetween;
    public float DistanceBetweenEdge;

    public float OriginalDistanceBetween;
    public bool isOriginalDistance;

    [Header("PlayerBehaviours")]
    public bool isTripping;

    [Header("SteppingLands")]
    public string LandType;

    [Header("Time Remaining Transform")]
    public Transform timeToReach;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        anim = transform.GetChild(0).GetComponent<Animator>();
        for (int i = 0; i < transform.childCount; i++)
        {
            TransformAnimList.Add(transform.GetChild(i).GetComponent<Transform>());
        }
        StartCoroutine(delayStartCode());
        //transform.position = Vector3.MoveTowards(transform.position, Points[pointIndex].transform.position, moveSpeed *Time.deltaTime);
    }

    void DisableAllCharacter()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void SetFirstChildToLast(int listNum)
    {
        DisableAllCharacter();

        //disable first child and set to the very last child
        transform.GetChild(0).gameObject.SetActive(false);
        //transform.GetChild(0).SetAsLastSibling();


        //enable selected child
        transform.GetChild(listNum).gameObject.SetActive(true);
        //transform.GetChild(listNum).SetAsFirstSibling();
        anim = transform.GetChild(listNum).GetComponent<Animator>();
    }

    IEnumerator delayStartCode()
    {
        // Debug.Log("Points index : " + PathwayController.instance.pointIndex);
        transform.position = Vector3.MoveTowards(
            transform.position,
            PathwayController
                .instance
                .Points[PathwayController.instance.pointIndex]
                .transform
                .position,
            moveSpeed * Time.deltaTime
        );
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SetFirstChildToLast();
        }
        if (!GameController.instance.gameStart)
        {
            moveSpeed = 0f;
            anim.SetBool("isRunning", false);
        }
        else
        {
            moveSpeed = 3f;
        }
        // Debug.Log("roattioin : " + transform.localEulerAngles.y);
        if (PathwayController.instance.pointIndex <= PathwayController.instance.Points.Count - 1)
        {
            //if not tripping
            if (!isTripping && moveSpeed > 0f)
            { //running to the points
                anim.SetBool("isRunning", true);
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    PathwayController
                        .instance
                        .Points[PathwayController.instance.pointIndex]
                        .transform
                        .position,
                    moveSpeed * Time.deltaTime
                );
            }

            //calculate the distance between player and the first obstacles
            Vector3 player = transform.position;
            Vector3 obstacles = ObstaclesController.instance.ObstaclesList[0].position;
            DistanceBetween = (obstacles - player).magnitude;
            if (!isOriginalDistance)
            {
                OriginalDistanceBetween = DistanceBetween;
                //                Debug.Log("original : " + OriginalDistanceBetween);
                isOriginalDistance = true;
            }
            //Distance between land edge jump trigger
            Vector3 triggerEdge = ObstaclesController.instance.TriggerJumpList[0].position;
            DistanceBetweenEdge = (triggerEdge - player).magnitude;

            //Debug.Log("Distance: " + DistanceBetween);

            // compute what we want to move and turn the player rotattioin
            Vector3 towards =
                PathwayController
                    .instance
                    .Points[PathwayController.instance.pointIndex]
                    .transform
                    .position - transform.position;
            // is it big enough to be reliable?
            if (towards.magnitude > 0.1f)
            {
                // turn to face that way
                transform.forward = towards;
            }

            //if plaer already reached the points, then go to the next points
            if (
                transform.position
                == PathwayController
                    .instance
                    .Points[PathwayController.instance.pointIndex]
                    .transform
                    .position
            )
            {
                if (GameController.instance.questionCount >= 80)
                {
                    timeToReach.gameObject.SetActive(true);
                }

                PathwayController.instance.pointIndex += 1;
            }
        }
    }

    public IEnumerator displayTimer(float time)
    {
        yield return new WaitForSeconds(time);
        timeToReach.gameObject.SetActive(true);
        if (GameController.instance.questionCount >= 80)
        {
            isTriggerJump = true;
            GameController.instance.ShowQuestion80();
        }
    }

    IEnumerator startTripping()
    {
        GameController.instance.HideQuestionsButtons();
        //set the tripping animation
        anim.SetBool("isTripping", true);
        anim.SetBool("isRunning", false);
        //if player looking straight 0angle
        if (transform.eulerAngles.y > -10f && transform.eulerAngles.y < 90)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z + 0.4f
            );
        }
        else if (transform.eulerAngles.y >= 90f && transform.eulerAngles.y < 180)
        {
            transform.position = new Vector3(
                transform.position.x + 0.4f,
                transform.position.y,
                transform.position.z
            );
        }
        yield return new WaitForSeconds(0.5f);

        isTripping = true;
        if (!GameObject.Find("fellSfx").GetComponent<AudioSource>().isPlaying)
        {
            GameObject.Find("fellSfx").GetComponent<AudioSource>().Play();
        }
        if (!GameObject.Find("awwSfx").GetComponent<AudioSource>().isPlaying)
        {
            GameObject.Find("awwSfx").GetComponent<AudioSource>().Play();
        }

        //remove one health if there are any left
        HealthController.instance.getHurt();

        if (HealthController.instance.healthCount > 0)
        {
            //if health more than 0
            //stand up and continue
            yield return new WaitForSeconds(2f);

            anim.SetBool("isTripping", false);
            anim.SetBool("isRunning", true);
            //yield return new WaitForSeconds(1f);
            isTripping = false;

            //if quetsioin 80 above then
            if (GameController.instance.questionCount >= 80)
            {
                //GameController.instance.buttonFunction80();
                if (isTriggerJump)
                {
                    //remove the current distance of obstacles
                    ObstaclesController.instance.ObstaclesList.RemoveAt(0);
                    isTriggerJump = false;
                }
                isOriginalDistance = false;
                //timeToReach.gameObject.SetActive(false);
                GameController.instance.questionCount++;
                yield return new WaitForSeconds(0.5f);
                GameController.instance.isQuestionShowUp = false;
            }
            else if (GameController.instance.questionCount < 80)
            {
                timeToReach.gameObject.SetActive(true);
            }
        }
        else
        {
            int roundCollected = PlayerPrefs.GetInt("RoundsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"));
            roundCollected += CollectionController.instance.rounds;

            PlayerPrefs.SetInt("RoundsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"), roundCollected);

            int starsCollected = PlayerPrefs.GetInt("StarsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"));
            starsCollected += CollectionController.instance.stars;

            PlayerPrefs.SetInt("StarsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"), starsCollected);
            GameController.instance.endMenuScreen.gameObject.SetActive(true);
            GameController.instance.Bg.gameObject.SetActive(true);
            GameController.instance.isQuestionShowUp = true;
            //show end menu
        }
    }

    IEnumerator startJump()
    {
        //yield return new WaitForSeconds(1.0f);
        if (GameController.instance.questionCount >= 80)
        {
            if (isTriggerJump)
            {
                //remove the current distance of obstacles
                ObstaclesController.instance.ObstaclesList.RemoveAt(0);
                isTriggerJump = false;
            }
            //Collect the round count
            CollectionController.instance.rounds++;
            GameController.instance.roundTransform.GetComponent<Animator>().Play("collected");

            yield return new WaitForSeconds(0.45f);
            //play sound
            if (!GameObject.Find("collected").GetComponent<AudioSource>().isPlaying)
            {
                GameObject.Find("collected").GetComponent<AudioSource>().Play();
            }
            yield return new WaitForSeconds(0.25f);
            GameController.instance.roundTransform.GetComponent<Animator>().Play("afterCollected");

            isOriginalDistance = false;
            //timeToReach.gameObject.SetActive(false);
            GameController.instance.questionCount++;
            GameController.instance.isQuestionShowUp = false;
            Debug.Log("start Jump");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("jumpTrigger"))
        {
            // Debug.Log("jump");
            anim.SetTrigger("isJumping");
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
        }

        if (other.gameObject.tag.Equals("jumpTriggerEdge"))
        {
            // Debug.Log("jumpEdge");
            anim.SetTrigger("isJumping");
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
        }
        if (other.gameObject.tag.Equals("crashTrigger"))
        {
            // Debug.Log("jumpEdge");
            StartCoroutine(startTripping());
            //moveSpeed = 0f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("jumpTrigger"))
        {
            Debug.Log("jumpExit");
            if (GameController.instance.questionCount < 80)
            {
                timeToReach.gameObject.SetActive(true);
            }
            StartCoroutine(startJump());
            // Debug.Log("exitjump");
            //make question popup again after jumping for a few second

            //ObstaclesController.instance.ObstaclesList.RemoveAt(0);
        }

        if (other.gameObject.tag.Equals("crashTrigger"))
        {
            // Debug.Log("jumpEdge");

            //moveSpeed = 0f;
        }
        if (other.gameObject.tag.Equals("jumpTriggerEdge"))
        {
            // Debug.Log("jumpEdge");

            //moveSpeed = 0f;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("floorCollsion"))
        {
            //Debug.Log("GreenGrass");
            LandType = other.transform.parent.transform.parent.GetComponent<InitPath>().landType;
        }
    }
}
