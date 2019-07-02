using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCycle : MonoBehaviour
{
    public Light sun;

    public float xSpeed = 5f;
    public float ySpeed = 5f;

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

    
    void Update()
    {
        float t = Time.deltaTime * minToSecond;

        transform.Rotate(new Vector3(xSpeed * t, ySpeed * t, 0f));

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
    }
}
