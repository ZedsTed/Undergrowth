using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MathsUtils;


public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Distance from the screen edge for mouse movement of camera.
    /// </summary>
    public float ScreenEdgeBorderThickness = 5.0f;

    [Header("CameraMode")]
    [Space]
    public bool IsoMode = true;
    public bool FreeMode = false;


    [Header("Movement Speeds")]
    [Space]

    [Tooltip("Minimum panning speed of camera.")]
    public float minPanSpeed;

    [Tooltip("Maximum panning speed of camera.")]
    public float maxPanSpeed;

    [Tooltip("Seconds taken for camera to reach maximum panning speed.")]
    public float panAcceleration;

    [Tooltip("Speed of the zooming.")]
    public float zoomSpeed;


    [Header("Movement Limits")]
    [Space]

    [Tooltip("")]
    public bool enableMovementLimits;

    [Tooltip("")]
    public Vector2 heightLimit;

    [Tooltip("")]
    public Vector2 lengthLimit;

    [Tooltip("")]
    public Vector2 widthLimit;

    protected Vector2 zoomLimit;


    protected float panSpeed;
    protected Vector3 initialPosition;
    protected Vector3 panMovement;
    protected Vector3 position;

    protected Quaternion rotation;
    protected bool rotationActive = false;
    protected Vector3 mouseDelta;
    protected Vector3 lastMousePosition;
    protected Quaternion initialRotation;
    protected float panIncrease = 0.0f;


    [Header("Rotation")]
    [Space]
    public bool RotationEnabled;
    public float rotateSpeed;    
    public GameObject pivotPoint;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        zoomLimit.x = 15;
        zoomLimit.y = 65;
    }


    private void Update()
    {
        if (IsoMode)
            FreeMode = false;

        if (FreeMode)
            IsoMode = false;
        
        #region Movement

        panMovement = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - ScreenEdgeBorderThickness)
            panMovement += transform.forward * panSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= ScreenEdgeBorderThickness)
            panMovement -= transform.forward * panSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= ScreenEdgeBorderThickness)
            panMovement -= transform.right * panSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - ScreenEdgeBorderThickness)
            panMovement += transform.right * panSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q))
            panMovement += transform.up * panSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.E))
            panMovement -= transform.up * panSpeed * Time.deltaTime;


        #region Zoom

        panMovement += transform.forward * Input.mouseScrollDelta.y * zoomSpeed;
        //Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, zoomLimit.x, zoomLimit.y);

        #endregion


        transform.Translate(panMovement, IsoMode ? Space.World : Space.Self);

        #endregion


        #region Panning Speed

        if (IsCameraMovementBeingInput())
        {
            panIncrease += Time.deltaTime / panAcceleration;
            panSpeed = Mathf.Lerp(minPanSpeed, maxPanSpeed, panIncrease);
        }
        else
        {
            panIncrease = 0;
            panSpeed = minPanSpeed;
        }

        #endregion




        #region Mouse Rotation

        if (RotationEnabled)
        {
            if (Input.GetMouseButton(2))
            {
                rotationActive = true;

                if (lastMousePosition.x >= 0 
                    && lastMousePosition.y >= 0
                    && lastMousePosition.x <= Screen.width
                    && lastMousePosition.y < Screen.height)
                {
                    mouseDelta = Input.mousePosition - lastMousePosition;

                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                        pivotPoint = hit.transform.gameObject;
                }
                else
                {
                    mouseDelta = Vector3.zero;
                }
            }

            if (Input.GetMouseButtonUp(2))
            {
                rotationActive = false;               
            }

            lastMousePosition = Input.mousePosition;
        }

        #endregion


        #region Boundries

        if (enableMovementLimits)
        {
            position = transform.position;
            position.y = Mathf.Clamp(position.y, heightLimit.x, heightLimit.y);
            position.z = Mathf.Clamp(position.z, lengthLimit.x, lengthLimit.y);
            position.x = Mathf.Clamp(position.x, widthLimit.x, widthLimit.y);

            transform.position = position;
        }

        #endregion
    }


    private void LateUpdate()
    {
        if (rotationActive && pivotPoint != null)
        {
            float rotateDegrees = 0f;
            rotateDegrees += rotateSpeed * mouseDelta.y;

            Vector3 currentVector = transform.position - pivotPoint.transform.position;
            currentVector.x = 0;

            float angleDifference = Vector3.Angle(initialPosition, currentVector) * (Vector3.Cross(initialPosition, currentVector).x > 0 ? 1 : -1);
            float newAngle = Mathf.Clamp(angleDifference + rotateDegrees, -30f, 45f);
            rotateDegrees = newAngle - angleDifference;

            transform.RotateAround(pivotPoint.transform.position, Vector3.up, mouseDelta.x * rotateSpeed);
            transform.RotateAround(pivotPoint.transform.position, transform.right, rotateDegrees);
        }

        lastMousePosition = Input.mousePosition;
    }

    /// <summary>
    /// Are any of the movement key bindings currently being input?
    /// </summary>   
    protected bool IsCameraMovementBeingInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)
            || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q)
            || Input.mousePosition.y >= Screen.height - ScreenEdgeBorderThickness
            || Input.mousePosition.y <= ScreenEdgeBorderThickness
            || Input.mousePosition.x <= ScreenEdgeBorderThickness
            || Input.mousePosition.x >= Screen.width - ScreenEdgeBorderThickness)
        {
            return true;
        }
        return false;
    }
}
