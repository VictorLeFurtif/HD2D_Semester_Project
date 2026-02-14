using System;
using Manager;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private CameraSettings newCameraSettings;
    
    private void OnTriggerEnter(Collider other)
    {
        TryTriggerCamera(other);
    }

    private void TryTriggerCamera(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.TriggerCamera(newCameraSettings);
        }
    }
}
