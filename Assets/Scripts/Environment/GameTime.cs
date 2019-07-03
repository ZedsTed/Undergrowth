using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : Singleton<MonoBehaviour>
{
    [SerializeField]
    protected bool paused;
    public bool Paused { get { return paused; } protected set { paused = value; } }

    [SerializeField]
    protected float timeScale = 1f;

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
        new TimeWarp(TimeWarp.TimeWarpState.SuperFast, 4f)
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
}
