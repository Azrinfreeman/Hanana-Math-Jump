using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [Header("PlayerInformation")]
    public Rigidbody rb;
    public Animator anim;
    public float jumpForce = 2.0f;
    public Vector3 jump;

    [SerializeField]
    private float moveSpeed;

    public float DistanceBetween;

    [Header("SteppingLands")]
    public string LandType;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        anim = transform.GetChild(0).GetComponent<Animator>();
        StartCoroutine(delayStartCode());
        //transform.position = Vector3.MoveTowards(transform.position, Points[pointIndex].transform.position, moveSpeed *Time.deltaTime);
    }

    IEnumerator delayStartCode()
    {
        transform.position = PathwayController
            .instance
            .Points[PathwayController.instance.pointIndex]
            .transform
            .position;
        yield return new WaitForSeconds(0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("roattioin : " + transform.localEulerAngles.y);
        if (PathwayController.instance.pointIndex <= PathwayController.instance.Points.Count - 1)
        {
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

            //calculate the distance between player and the first obstacles
            Vector3 player = transform.position;
            Vector3 obstacles = ObstaclesController.instance.ObstaclesList[0].position;
            DistanceBetween = (obstacles - player).magnitude;
            //Debug.Log("Distance: " + DistanceBetween);

            // compute what we want to move
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
            if (
                transform.position
                == PathwayController
                    .instance
                    .Points[PathwayController.instance.pointIndex]
                    .transform
                    .position
            )
            {
                PathwayController.instance.pointIndex += 1;
            }
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
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("jumpTrigger"))
        {
            // Debug.Log("exitjump");
            //ObstaclesController.instance.ObstaclesList.RemoveAt(0);
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
