using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cm_playerCamera;
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float moveTime = 5f;
    [SerializeField] private float edgeScrollOffset = 20f;
    [SerializeField] private float dragRotationTime = 1f;
    [SerializeField] private float dragRotationFactor = 1/2f;
    [SerializeField] private float digitalZoomIntensity = 10f;
    [SerializeField] private float analogZoomIntensity = 5f;
    [SerializeField] private float followOffsetMin = 5f;
    [SerializeField] private float followOffsetMax = 50f;
    
    private CinemachineTransposer cm_playerCamera_Cm_Transposer;

    private Vector3 movementDampeningRef;
    private Vector3 rawRotationDir;
    private Vector3 dampedRotationDir;    
    private Vector3 dragRotationDampeningRef;
    private Vector3 followOffset;

	private Vector2 rawMovementInput;


    private float rotationInput;
    private float mouseVelocityX;
    private float previousMousePositionX;
    private float zoomInput;

    private bool isEdgeScrolling;
    private bool isPanning;
    private bool isDragRotating;
    private bool isDigitalZooming;
    private bool isAnalogZooming;

	private void Start()
	{
		List<GameConfiguration> gameConfig = FileHandler.ReadFromJson<GameConfiguration>("config.json");
		GameConfiguration gameConfigData;
		if (gameConfig.Count == 0)
			SaveCurrentCameraConfiguration();
		else
		{
			gameConfigData = gameConfig[0];
			ChangeCameraConfiguration(gameConfigData);
		}
	}

	private void SaveCurrentCameraConfiguration()
    {
        GameConfiguration gameConfig = new GameConfiguration(moveSpeed, dragRotationFactor, digitalZoomIntensity, analogZoomIntensity);
        FileHandler.SaveToJson<GameConfiguration>(new List<GameConfiguration>() { gameConfig }, "config.json");
    }

    public void RestoreCurrentCameraConfiguration()
    {
		List<GameConfiguration> gameConfig = FileHandler.ReadFromJson<GameConfiguration>("config.json");
		GameConfiguration gameConfigData;
		if (gameConfig.Count > 0)
		{
			gameConfigData = gameConfig[0];
			ChangeCameraConfiguration(gameConfigData);
		}
	}

    public void ChangeCameraConfiguration(GameConfiguration gameConfigData)
    {
        moveSpeed = gameConfigData.cameraConfiguration.moveSpeed;
		dragRotationFactor = gameConfigData.cameraConfiguration.dragRotationFactor;
		digitalZoomIntensity = gameConfigData.cameraConfiguration.digitalZoomIntensity;
        analogZoomIntensity = gameConfigData.cameraConfiguration.analogZoomIntensity;
	}

    public void OnCameraMove(InputAction.CallbackContext context) //Move Camera Using Keyboard
    {
        rawMovementInput = context.ReadValue<Vector2>();
    }

    public void OnCameraEdgeScroll(InputAction.CallbackContext context) //Move Camera using Mouse - Edge Scroll
    {
        isEdgeScrolling = context.performed;
    }

    public void OnCameraDragRotate(InputAction.CallbackContext context)
    {
        rotationInput = context.ReadValue<float>() > 0 ? 1 : context.ReadValue<float>() < 0 ? -1 : 0;
        isDragRotating = context.performed;
    }

    public void OnCameraZoomAnalog(InputAction.CallbackContext context)
    {
        isAnalogZooming = context.performed;
        zoomInput = context.ReadValue<float>();
    }

    public void OnCameraZoomDigital(InputAction.CallbackContext context)
    {
        isDigitalZooming = context.performed;
        zoomInput = Mathf.Lerp(zoomInput, context.ReadValue<float>(), Time.deltaTime * 10f);
    }

	private void Awake()
	{
        cm_playerCamera_Cm_Transposer = cm_playerCamera.GetCinemachineComponent<CinemachineTransposer>();
        followOffset = cm_playerCamera_Cm_Transposer.m_FollowOffset;
	}

	private void Update()
	{
        //HandleEdgeScrollMovement();
        HandleCameraMovement();
        HandleWorldDragCameraRotation();
        HandleCameraZoom();
	}

	private void HandleCameraMovement()
    {
        Vector3 moveDir = transform.forward * rawMovementInput.y + transform.right * rawMovementInput.x;
        transform.position = Vector3.SmoothDamp(transform.position, transform.position + moveDir.normalized * moveSpeed, ref movementDampeningRef, Time.deltaTime * moveTime);
    }

    private void HandleEdgeScrollMovement()
    {
        if (isEdgeScrolling && !isPanning && !isDragRotating)
        {
            if (Mouse.current.position.ReadValue().x < edgeScrollOffset) rawMovementInput.x = -1f;
            else if (Mouse.current.position.ReadValue().x > Screen.width - edgeScrollOffset) rawMovementInput.x = +1f;
            else rawMovementInput.x = 0f;
            if (Mouse.current.position.ReadValue().y < edgeScrollOffset) rawMovementInput.y = -1f;
            else if (Mouse.current.position.ReadValue().y > Screen.height - edgeScrollOffset) rawMovementInput.y = +1f;
            else rawMovementInput.y = 0f;
        }
    }    

    private void HandleWorldDragCameraRotation()
    {    
        rawRotationDir = Vector3.up * rotationInput * CalculateMouseVelocityX() * dragRotationFactor;
        dampedRotationDir = Vector3.SmoothDamp(dampedRotationDir, rawRotationDir, ref dragRotationDampeningRef, Time.deltaTime * dragRotationTime);
        transform.Rotate(dampedRotationDir);        
	}

    private float CalculateMouseVelocityX()
    {
        mouseVelocityX = (Mouse.current.position.ReadValue().x - previousMousePositionX) / Time.deltaTime;

        previousMousePositionX = Mouse.current.position.ReadValue().x;

        return Mathf.Abs(mouseVelocityX);
    }

    private void HandleCameraZoom()
    {
        float zoomIntensity = 0f;
        if (isDigitalZooming) zoomIntensity = digitalZoomIntensity;
        else if (isAnalogZooming) zoomIntensity = analogZoomIntensity;
        Vector3 zoomDir = followOffset.normalized;
        followOffset += zoomIntensity * zoomDir * (zoomInput > 0 ? -1 : zoomInput < 0 ? 1 : 0);
        if (followOffset.magnitude < followOffsetMin) followOffset = zoomDir * followOffsetMin;
        if (followOffset.magnitude > followOffsetMax) followOffset = zoomDir * followOffsetMax;

        cm_playerCamera_Cm_Transposer.m_FollowOffset = Vector3.Lerp(cm_playerCamera_Cm_Transposer.m_FollowOffset, followOffset, Time.deltaTime * 15f);
        zoomInput = 0;
    }
}
