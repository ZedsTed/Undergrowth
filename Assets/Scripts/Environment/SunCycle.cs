using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCycle : MonoBehaviour
{
    public Light sun;

   

    public float xSpeed = 0.5f;
    public float ySpeed = 0.5f;

    /// <summary>
    /// A pretty basic time acceleration multiplier for the moment.
    /// </summary>
    [Range(0, 5)]
    public float minToSecond = 1f;

    [SerializeField]
    protected int currentMinute;

    protected const int minutesInDay = 1440;

    protected int dayCount = 0;
    protected float timeSinceLastMinute;

    protected float totalSetTime = 1f;

    
    void Update()
    {
        float t = Time.deltaTime;

        transform.Rotate(new Vector3(xSpeed * t, ySpeed * t, 0f));
        // this whole minToSecond thing is a bit whacky
        // needs to be simplified and split into a better method.
        // If we've had a minute passed.
        if (timeSinceLastMinute >= minToSecond)
        {
            timeSinceLastMinute = 0f;
            MinutePassed();
        }
        else
        {
            timeSinceLastMinute += t;
        }
    }

    float setTime = 0f;
    protected void MinutePassed()
    {
        ++currentMinute;
        if (currentMinute >= minutesInDay)
        {
            // Next day!
            ++dayCount;
            currentMinute = 0;
            Debug.Log("Good morning!");
        }

        int min = currentMinute % 60;
        int hrs = currentMinute / 60;

        Debug.Log("Next minute! " + hrs.ToString() + " : " + min.ToString());

        // still quite fucked at > 18. Seems to work fine with > 6f though. perhaps set time is too low.
        if (hrs >  18)
        {
            setTime += Time.deltaTime / totalSetTime;
            sun.intensity = Mathf.Lerp(1f, 0f, setTime);
        }
        else if (hrs > 6)
        {
            setTime += Time.deltaTime / totalSetTime;
            sun.intensity = Mathf.Lerp(0f, 1f, setTime);
        }
        else
        {
            setTime = 0f;
        }
    }
}
