using System;
using System.Collections;
using Enum;
using Manager;
using UnityEditor;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    
    [Header("Components")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerTransform;
    
    [Header("Settings Camera Fix")]
    [SerializeField] private float travelDuration = 1f;
    
    [Header("Settings Camera Follow")]
    [SerializeField] private Vector3 offsetCamera;
    [SerializeField] private float smoothTime = 0.3f;
    
    private CameraPlayerState cameraState = CameraPlayerState.FollowPlayer;
    private float cameraPositionY = 0f;
    
    private Coroutine cameraCoroutine;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        if (cameraTransform == null)
        {
            Debug.LogError($"{nameof(CameraManager)} : CameraTransform non assigné !");
            enabled = false;
        }
        
        cameraPositionY = cameraTransform.position.y;
    }

    private void OnEnable()
    {
        EventManager.OnCameraTrigger += TravelingCamera;
    }

    private void OnDisable()
    {
        EventManager.OnCameraTrigger -= TravelingCamera;
    }

    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void TravelingCamera(CameraSettings cameraSettings)
    {
        if (cameraCoroutine != null)
            StopCoroutine(cameraCoroutine);

        cameraCoroutine = StartCoroutine(TravelingCameraIE(cameraSettings));
    }

    private IEnumerator TravelingCameraIE(CameraSettings cameraSettings)
    {
        Vector3 startPosition = cameraTransform.position;

        float elapsedTime = 0f;

        while (elapsedTime < travelDuration)
        {
            elapsedTime += Time.deltaTime;
            float ratio = elapsedTime / travelDuration;

            float x = Mathf.Lerp(startPosition.x, cameraSettings.cameraPosition.x, ratio);
            float z = Mathf.Lerp(startPosition.z, cameraSettings.cameraPosition.y, ratio);
            
            cameraTransform.position = new Vector3(x, cameraTransform.position.y, z);
            yield return null;
        }

        cameraTransform.position = new Vector3(
            cameraSettings.cameraPosition.x,
            cameraTransform.position.y,
            cameraSettings.cameraPosition.y);
        
        cameraCoroutine = null;
    }

    private void FollowPlayer()
    {
        if (cameraState != CameraPlayerState.FollowPlayer) return;
        
        Vector3 targetPosition = playerTransform.position +  offsetCamera;
        
        Vector3 newPosition = Vector3.SmoothDamp(
            cameraTransform.position,
            targetPosition,
            ref velocity,
            smoothTime);;
        
        newPosition = new Vector3(newPosition.x, cameraPositionY, newPosition.z);
        
        cameraTransform.position = newPosition;
    }
}