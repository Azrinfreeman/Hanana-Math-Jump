using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;

public class DisableCameraRotation : MonoBehaviour
{
    public CinemachineFreeLook freeLook;

    private void Lock()
    {
        freeLook.m_YAxis.m_MaxSpeed = 0;
        freeLook.m_XAxis.m_MaxSpeed = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
        Lock();
    }

    // Update is called once per frame
    void Update() { }
}
