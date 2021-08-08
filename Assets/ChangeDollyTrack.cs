using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDollyTrack : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
    public int enterPriority = 11;
    public int exitPriority = 9;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            vCam.Priority = enterPriority;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            vCam.Priority = exitPriority;
    }
}
