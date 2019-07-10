using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : Singleton<GameTime>
{
    [SerializeField]
    protected bool paused;
    public bool Paused { get { return paused; } protected set { paused = value; } }

    [SerializeField]
    protected float timeScale = 1f;

    /// <summary>
    /// How much we want our in-game world to be scaled for time (i.e. one in-game minute is two real-world seconds).
    /// Lower number is faster.
    /// </summary>
    [SerializeField]
    protected float gameMinuteToRealSecond = 1f;
    public float GameMinuteToRealSecond { get { return gameMinuteToRealSecond; } }

    /// <summary>
    /// The current minute, default set to 720 so we start at midday.
    /// </summary>
    [SerializeField]
    protected int currentMinute = 720;
    public int MinuteCount => currentMinute;

    /// <summary>
    /// How many minutes are in our in-game days. Always should be 24 * 60.
    /// </summary>
    protected const int minutesInDay = 1440;
    public int MinutesInDay => minutesInDay;
    public float RealSecondsInDay { get { return minutesInDay * gameMinuteToRealSecond; } }

    /// <summary>
    /// What day we're on.
    /// </summary>
    protected int dayCount = 0;
    public int DayCount => dayCount;

    /// <summary>
    /// How many seconds since our last minute was ticked over.
    /// </summary>
    protected float timeSinceLastMinute;
    public float SecondCount => timeSinceLastMinute;

    /// <summary>
    /// Returns the current in-game second count of the current day. 
    /// Can be used to tell how far through the day you are.
    /// </summary>
    public float CurrentMinuteAndSeconds { get { return currentMinute + (timeSinceLastMinute/60); } }

    /// <summary>
    /// We use this struct to just hold the data that we need for a time warp enum state its accompanying speed.
    /// </summary>
    public struct TimeWarp
    {
        public enum TimeWarpState
        {
            Normal,
            Fast,
            SuperFast
        }

        public TimeWarpState State { get; private set; }

        public float Speed { get; private set; }

        public TimeWarp(TimeWarpState state, float speed)
        {
            State = state;
            Speed = speed;
        }
    }


    public List<TimeWarp> TimeWarpStates = new List<TimeWarp>()
    {
        new TimeWarp(TimeWarp.TimeWarpState.Normal, 1f),
        new TimeWarp(TimeWarp.TimeWarpState.Fast, 2f),
        new TimeWarp(TimeWarp.TimeWarpState.SuperFast, 5f)
    };

    public TimeWarp.TimeWarpState CurrentState { get; protected set; }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TogglePause();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetTimeWarp(TimeWarp.TimeWarpState.Normal);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SetTimeWarp(TimeWarp.TimeWarpState.Fast);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SetTimeWarp(TimeWarp.TimeWarpState.SuperFast);

        TrackTime();
    }

    public void TogglePause()
    {
        // Unpause if we're paused and pause if we're not.
        // Resumes to the previously stored timescale when unpaused.
        if (Paused)
        {
            Time.timeScale = timeScale;
            Paused = false;
        }
        else
        {
            Time.timeScale = 0f;
            Paused = true;
        }
    }

    protected void SetTimeWarp(TimeWarp.TimeWarpState state)
    {
        if (state != CurrentState)
        {
            for (int i = TimeWarpStates.Count; i-- > 0;)
            {
                if (TimeWarpStates[i].State == state)
                {
                    Time.timeScale = TimeWarpStates[i].Speed;
                    timeScale = Time.timeScale;
                    CurrentState = state;
                }
            }
        }
    }

    protected void TrackTime()
    {
        float t = Time.deltaTime / gameMinuteToRealSecond;

        if (timeSinceLastMinute >= gameMinuteToRealSecond)
        {
            timeSinceLastMinute = 0f;
            OnMinutePassed();
        }
        else
        {
            timeSinceLastMinute += t;
        }
    }

    protected void OnMinutePassed()
    {
        ++currentMinute;
        if (currentMinute >= minutesInDay)
        {
            // Next day!
            ++dayCount;
            currentMinute = 0;
            Debug.Log("Good morning!");
        }

        //int min = currentMinute % 60;
        //int hrs = currentMinute / 60;

        //Debug.Log("Next minute! " + hrs.ToString() + " : " + min.ToString());
    }
}
