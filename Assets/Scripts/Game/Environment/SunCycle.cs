using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MathsUtils;

/// <summary>
/// Currently works but not optimally, lerping makes it jerk and we ideally want to track it better like the following:
/// https://wiki.unity3d.com/index.php/DayNightController
/// </summary>
public class SunCycle : MonoBehaviour
{
    public Light sun;
    

    protected Quaternion qMidday = new Quaternion(0.8251674f, 0f, 0f, 0.5648884f);
    protected Quaternion qNextDay = new Quaternion(-0.8251674f, 0f, 0f, -0.5648884f);

    protected Quaternion sunRotation = Quaternion.identity;

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
        float percHalfDay = 0f;

        // Figure out how far we are past midday, so we remove half a day and see how far we are through it
        percHalfDay = ((GameTime.Instance.CurrentMinuteAndSeconds - (GameTime.Instance.MinutesInDay / 2)) / (GameTime.Instance.MinutesInDay / 2));

        transform.rotation = Quaternion.Lerp(qMidday, qNextDay, percHalfDay);

        //// If we're more than halfway through the day.
        //if (GameTime.Instance.CurrentMinuteAndSeconds > (GameTime.Instance.MinutesInDay / 2))
        //{
        //    // Figure out how far we are past midday, so we remove half a day and see how far we are through it
        //    percHalfDay = ((GameTime.Instance.CurrentMinuteAndSeconds - (GameTime.Instance.MinutesInDay / 2)) / (GameTime.Instance.MinutesInDay / 2));        

        //    transform.rotation = Quaternion.Lerp(qMidday, qNextDay, percHalfDay);
        //}
        //else
        //{   // If we're less than halfway through the day
        //    // Figure out how far we are past midday, so we remove half a day and see how far we are through it
        //    percHalfDay = (GameTime.Instance.CurrentMinuteAndSeconds
        //                    / (GameTime.Instance.MinutesInDay / 2));

        //    transform.rotation = Quaternion.Lerp(qNextDay, qMidday, percHalfDay);
        //}
    }

    /// <summary>
    /// Changes the intensity of the sun in accordance with how far through the day we are.
    /// </summary>
    protected void TrackSunIntensity()
    {
       

        float intensity = Vector3.Dot(middayForward, transform.forward);
        sun.intensity = intensity;
    }
}
