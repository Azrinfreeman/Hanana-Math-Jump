using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;

public class DisableCameraRotation : MonoBehaviour
{
    public CinemachineFreeLook freeLook;
    public PathController player;

    public float timetaken;

    private void Lock()
    {
        freeLook.m_YAxis.m_MaxSpeed = 0;
        freeLook.m_XAxis.m_MaxSpeed = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        timetaken = 0.4f;
        player = GameObject.Find("Player").GetComponent<PathController>();
        freeLook = GetComponent<CinemachineFreeLook>();
        Lock();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.eulerAngles.y >= -10 && player.transform.eulerAngles.y < 80)
        {
            if (timetaken > 0)
            {
                timetaken -= Time.timeScale;
            }
            else
            {
                if (freeLook.m_Heading.m_Bias >= 34 && freeLook.m_Heading.m_Bias < 74)
                {
                    freeLook.m_Heading.m_Bias--;
                    timetaken = 0.4f;
                }
            }
        }
        else if (player.transform.eulerAngles.y >= 80 && player.transform.eulerAngles.y < 180)
        {
            if (timetaken > 0)
            {
                timetaken -= Time.timeScale;
            }
            else
            {
                if (freeLook.m_Heading.m_Bias != 73)
                {
                    freeLook.m_Heading.m_Bias++;
                    timetaken = 0.4f;
                }
            }
        }
    }
}
