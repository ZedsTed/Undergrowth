using UnityEngine;


public class CameraOrbit : MonoBehaviour
{
    [Header("Input")]
    public int orbitMouseButton = 1;

    [Header("Elevation")]
    public float elevation = 30f;
    public bool elevationInvert = false;
    public float elevationSpeed = 480f;
    public float elevationMin = 10f;
    public float elevationMax = 80f;
    public float elevationSmooth = 6f;

    [Header("Azimuth")]
    public float azimuth = 40f;
    public bool azimuthInvert = false;
    public float azimuthSpeed = 720f;
    public float azimuthSmooth = 6f;

    [Header("Distance")]
    public float distance = 20f;
    public bool distanceInvert = false;
    public float distanceSpeed = 2000f;
    public float distanceMin = 5f;
    public float distanceMax = 50f;
    public float distanceSmooth = 4f;

    [Header("Pan")]
    public Vector3 pan = Vector3.zero;
    public float panSpeedMax = 10;
    public float panSmooth = 8;
    public Vector3 panPlaneRotation = new Vector3(0, 0, 0);
    public float panPlaneOriginDistance = 0;

    [Header("Target")]
    public Transform target;
    public bool targetTrackPosition = false;
    public bool targetTrackRotationY = false;
    public Vector3 targetPositionOffset = Vector3.zero;


    protected float elevationCur;
    protected float azimuthCur;
    protected float distanceCur;
    protected Vector3 panCur;


    protected void Start()
    {
        elevationCur = elevation;
        azimuthCur = azimuth;
        distanceCur = distance;
    }

    protected void LateUpdate()
    {
        // inputs
        UpdateInputOrbit();
        UpdateInputPan();

        // lerps
        elevationCur = Mathf.Lerp(elevationCur, elevation, elevationSmooth * Time.unscaledDeltaTime);
        azimuthCur = Mathf.Lerp(azimuthCur, azimuth, azimuthSmooth * Time.unscaledDeltaTime);
        distanceCur = Mathf.Lerp(distanceCur, distance, distanceSmooth * Time.unscaledDeltaTime);
        panCur = Vector3.Slerp(panCur, pan, panSmooth * Time.unscaledDeltaTime);

        // rotation first
        Quaternion rot = Quaternion.AngleAxis(azimuthCur, Vector3.up) * Quaternion.AngleAxis(elevationCur, Vector3.right);
        if (target != null && targetTrackRotationY)
            rot = Quaternion.AngleAxis(target.transform.rotation.eulerAngles.y, Vector3.up) * rot;

        // then position
        Vector3 pos = rot * new Vector3(0f, 0f, -distanceCur);
        if (target != null && targetTrackPosition)
            pos += target.transform.position + targetPositionOffset;

        // add pan
        pos += panCur;


        // set transform
        transform.position = pos;
        transform.rotation = rot;
    }

    protected void UpdateInputOrbit()
    {
        float elevationInput = 0;
        float azimuthInput = 0;
        float distanceInput = 0;

        // grab inputs
        distanceInput += Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetMouseButton(orbitMouseButton))
        {
            azimuthInput += Input.GetAxis("Mouse X");
            elevationInput += Input.GetAxis("Mouse Y");
        }

        // use inputs
        if (elevationInput != 0)
        {
            if (elevationInvert)
                elevationInput = -elevationInput;

            elevation -= Mathf.Clamp(elevationInput, -1f, 1f) * elevationSpeed * Time.unscaledDeltaTime;
            elevation = Mathf.Clamp(elevation, elevationMin, elevationMax);
        }

        if (azimuthInput != 0)
        {
            if (azimuthInvert)
                azimuthInput = -azimuthInput;

            azimuth += Mathf.Clamp(azimuthInput, -1f, 1f) * azimuthSpeed * Time.unscaledDeltaTime;
        }

        if (distanceInput != 0)
        {
            if (!distanceInvert)
                distanceInput = -distanceInput;

            distance += Mathf.Clamp(distanceInput, -1f, 1f) * distanceSpeed * Time.unscaledDeltaTime;
            distance = Mathf.Clamp(distance, distanceMin, distanceMax);
        }
    }

    protected void UpdateInputPan()
    {
        // grab inputs
        Vector3 input = Vector3.zero;
        input += UpdateInputPanAxis();
        input += UpdateInputPanRaycast();

        // normalize if req
        float inputSqrMag = input.sqrMagnitude;
        if (inputSqrMag > 1)
            input = input / Mathf.Sqrt(inputSqrMag);

        // calc desired pan
        pan += input * panSpeedMax * Time.unscaledDeltaTime;
    }

    protected Vector3 UpdateInputPanAxis()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        // transform so it's along the plane
        Quaternion planeQuaternion = Quaternion.Euler(panPlaneRotation);

        input = planeQuaternion * input;

        input = Quaternion.AngleAxis(azimuthCur, Vector3.up) * input;

        return input;
    }

    protected Vector3 UpdateInputPanRaycast()
    {
        return Vector3.zero;
    }

    protected bool GetPlaneHitPoint(Vector3 rayOrigin, Vector3 rayDirection, out Vector3 hitPoint, out bool hitForward)
    {
        Plane p = new Plane(Quaternion.Euler(panPlaneRotation) * Vector3.back, panPlaneOriginDistance);

        hitPoint = Vector3.zero;
        hitForward = false;

        float hitDistance;
        if (p.Raycast(new Ray(rayOrigin, rayDirection), out hitDistance))
        {
            hitPoint = rayOrigin + rayDirection * hitDistance;
            hitForward = true;
            return true;
        }

        if (p.Raycast(new Ray(rayOrigin, -rayDirection), out hitDistance))
        {
            hitPoint = rayOrigin - rayDirection * hitDistance;
            hitForward = false;
            return true;
        }

        return false;
    }

    #region Gizmos

    Vector3 gizmoSize = new Vector3(10, 10, 1);
    Vector3[] gizmoPointsRaw = new Vector3[4]{
            new Vector3(1, 1, 0),
            new Vector3(1, -1, 0),
            new Vector3(-1, -1, 0),
            new Vector3(-1, 1, 0)
        };
    Vector3[] gizmoPoints = new Vector3[4];

    protected void OnDrawGizmosSelected()
    {
        // draw pan plane gizmo

        Quaternion planeQuaternion = Quaternion.Euler(panPlaneRotation) * Quaternion.AngleAxis(90, Vector3.right);

        Matrix4x4 planeMatrix = Matrix4x4.TRS(
            planeQuaternion * new Vector3(0, 0, -panPlaneOriginDistance),
            planeQuaternion,
            gizmoSize);

        for (int i = 0; i < 4; ++i)
            gizmoPoints[i] = planeMatrix.MultiplyPoint3x4(gizmoPointsRaw[i]);

        for (int i = 0; i < 4; ++i)
        {
            Gizmos.DrawLine(gizmoPoints[i], gizmoPoints[i == 3 ? 0 : i + 1]);
        }
    }

    #endregion
}
