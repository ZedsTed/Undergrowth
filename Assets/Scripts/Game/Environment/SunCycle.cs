using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MathsUtils;

public class SunCycle : MonoBehaviour
{
    public Light sun;
    public AnimationCurve sunlightIntensity;

    protected Vector3 middayForward = new Vector3(0.0f, -0.9f, -0.4f);

    protected void Update()
    {
        TrackSunRotation();
        TrackSunIntensity();
    }

    /// <summary>
    /// Rotates the sun in accordance with how far through the day we are.
    /// </summary>
    protected void TrackSunRotation()
    {        
        transform.Rotate(Vector3.up, ((Time.deltaTime / GameTime.Instance.RealSecondsInDay) * 360.0f), Space.Self);
    }

    /// <summary>
    /// Changes the intensity of the sun in accordance with how far through the day we are.
    /// </summary>
    protected void TrackSunIntensity()
    {
        float time = Vector3.Dot(middayForward, transform.forward);
        sun.intensity = sunlightIntensity.Evaluate(time);
    }
}
