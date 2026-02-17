using Enum;
using Manager;
using UnityEngine;

public class CameraTriggerCinematic : CameraTriggerBase
{
    [SerializeField] private Vector3 cameraPosition;
    [SerializeField] private float holdDuration = 2f;
    
    protected override void Trigger()
    {
        CameraSettings settings = new CameraSettings
        {
            CameraPosition = cameraPosition,
            CameraPlayerState = CameraPlayerState.Cinematic,
            holdDuration = holdDuration
        };
        
        EventManager.TriggerCamera(settings);
    }
}